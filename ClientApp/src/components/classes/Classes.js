import { useEffect } from "react";
import { useState } from "react";
import ModalButton from "../ModalBtn";
import { Button, Form, FormControl, InputGroup, Accordion } from "react-bootstrap";
import { NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { getData, postData, deleteData, postDataWithImage } from "../services/AccessAPI";

const URLClass = `/api/classes`;
const URLStudent = `/api/students`;

const Classes = () => {
    const [allClases, setClasses] = useState([]);
    const [allStudents, setStudents] = useState([]);
    const [file, setFile] = useState()

    const getClasses = async () => {
        getData(URLClass + `/getClasses`).then(
            (result) => {
                if(result){
                    setClasses(result)
                    return result;
                }
            }
        )
    };

    const addClass = async () => {
        const newClass = {
            name: `${document.getElementById('name').value}`,
        };

        postData(URLClass + `/createClass`, newClass).then(
            (result) =>{
                if(result){
                    allClases.push(result);
                    setClasses(allClases.slice());
                }
            }
        )
    }

    const deleteClass = (id) => {
        deleteData(URLClass + `/deleteClass/${id}`).then(
            (result) =>{
                if(result){
                    setClasses(allClases.filter(x => x.id !== id));
                }
            }
        )
    }

    const getStudents = async () => {
        getData(URLStudent + `/getStudents`).then(
            (result) => {
                if(result){
                    setStudents(result)
                    return result;
                }
            }
        )
    };
    
    const addStudent = async () => {
        let formData = new FormData()

        const newStudent = {
            firstName: `${document.getElementById('firstName').value}`,
            lastName: `${document.getElementById('lastName').value}`,
            middleName: `${document.getElementById('middleName').value}`,
            image: file,
            login: `S${1000000000 + (allStudents[allStudents.length - 1]?.id !== undefined ? allStudents[allStudents.length - 1].id : 0)}`,
            password: `${document.getElementById('password').value}`,
            idClass: allClases.find(s => `${s.name}`=== `${document.getElementById('class').value}`).id,
        };

        Object.keys(newStudent).forEach(function (key) {
            formData.append(key, newStudent[key]);
        });

        postDataWithImage(URLStudent + `/createStudent`, formData).then(
            (result) =>{
                if(result){
                    allStudents.push(result);
                    setStudents(allStudents.slice());
                }
            }
        )
    }

    useEffect(() => {
        getClasses();
        getStudents();
    }, []);

    return(
        <div>
            <ModalButton 
            btnName={'Добавить класс'} 
            title={'Создание класса'}
            clearList={getClasses}
            modalContent={
                <div>
                    <Form>
                    <InputGroup className="mb-3">
                        <InputGroup.Text>Название</InputGroup.Text>
                        <FormControl id="name" type="text"></FormControl>
                    </InputGroup>
                    <Button type="submit" onClick={() => addClass()}>Создать!</Button>
                </Form>                  
                </div>
            }
            />
            {allClases.map(x => <ClassItem key={x.id} _class={x} deleteAction={deleteClass} addAction={addStudent} allStudents={allStudents} getClasses={getClasses} setFile={setFile}/>)}
        </div>
    )
}

export default Classes;

const ClassItem = ({_class, deleteAction, addAction, allStudents, getClasses, setFile}) => {
    return(
        <div style={{backgroundColor: 'whitesmoke', margin: '10px', borderRadius: '10px', padding: '10px'}}>
            <h3 className="mb-3">{_class.name}</h3>
            <Accordion className="mb-3">
                <Accordion.Item eventKey="0">
                    <Accordion.Header>Студенты</Accordion.Header>
                        <Accordion.Body>
                            <ul className="list-group mb-3">
                                {_class.students?.map(s => 
                                    <NavLink tag={Link} className="text-dark" to={`/student/${s.id}`}>
                                        <li className="list-group-item" value={s} id={s.id}> {s.firstName} {s.lastName} {s.middleName}</li>
                                    </NavLink>)}
                            </ul>
                            <ModalButton 
                            btnName={'Добавить cтудента'} 
                            title={'Создание студента'}
                            clearList={getClasses}
                            modalContent={
                                <div>
                                    <Form>
                                    <InputGroup className="mb-3">
                                        <InputGroup.Text>Имя</InputGroup.Text>
                                        <FormControl id="firstName" type="text"></FormControl>
                                    </InputGroup>
                                    <InputGroup className="mb-3">
                                        <InputGroup.Text>Фамилия</InputGroup.Text>
                                        <FormControl id="lastName" type="text"></FormControl>
                                    </InputGroup>
                                    <InputGroup className="mb-3">
                                        <InputGroup.Text>Отчество</InputGroup.Text>
                                        <FormControl id="middleName" type="text"></FormControl>
                                    </InputGroup>
                                    <InputGroup className="mb-3">
                                        <InputGroup.Text>Логин</InputGroup.Text>
                                        <FormControl id="login" type="text" disabled placeholder={`S${1000000000 + (allStudents[allStudents.length - 1]?.id !== undefined ? allStudents[allStudents.length - 1].id : 0)}`}></FormControl>
                                    </InputGroup>
                                    <InputGroup className="mb-3">
                                        <InputGroup.Text>Пароль</InputGroup.Text>
                                        <FormControl id="password" type="text"></FormControl>
                                    </InputGroup>
                                    <InputGroup className="mb-3">
                                        <InputGroup.Text>Аватарка</InputGroup.Text>
                                        <input className="form-control form-control-sm" id="image" type="file" accept="image/*" onChange={e => setFile(e.target.files[0])} multiple/>
                                    </InputGroup>
                                    <InputGroup className="mb-3">
                                        <InputGroup.Text>Класс</InputGroup.Text>
                                        <FormControl id="class" type="text" value={_class.name} disabled></FormControl>
                                    </InputGroup>
                                    <Button type="submit" onClick={() => addAction()}>Создать!</Button>
                                </Form>                  
                                </div>
                            }
                            />
                        </Accordion.Body>
                </Accordion.Item>
            </Accordion> 
            <Button variant="danger" onClick={() => deleteAction(_class.id)}>Удалить</Button>        
        </div>
    )
};