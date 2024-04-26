import { useEffect } from "react";
import { useState } from "react";
import ModalButton from "../ModalBtn";
import { Button, Form, FormControl, InputGroup, Accordion} from "react-bootstrap";
import { getData, postDataWithImage, deleteData } from "../services/AccessAPI";

const URL = `/api/teachers`;

const Teachers = () => {
    const [allTeachers, setTeachers] = useState([]);
    const [file, setFile] = useState()

    const getTeachers = async () => {
        getData(URL + '/getTeachers').then(
            (result) => {
                if(result){
                    setTeachers(result)
                    return result;
                }
            }
        )
    }

    const deleteTeacher = (id) =>{
        deleteData(URL + `/deleteTeacher/${id}`).then(
            (result) =>{
                if(result){
                    setTeachers(allTeachers.filter(x => x.id !== id));
                }
            }
        )
    }

    const addTeacher = async () => {
        let formData = new FormData()

        const newTeacher = {
            firstName: document.getElementById('firstName').value,
            lastName: document.getElementById('lastName').value,
            middleName: document.getElementById('middleName').value,
            image: file,
            login: `T${1000000000 + (allTeachers[allTeachers.length - 1]?.id !== undefined ? allTeachers[allTeachers.length - 1].id : 0)}`,
            password: document.getElementById('password').value,
            role: 1,
            subjects: []
        };

        Object.keys(newTeacher).forEach(function (key) {
            formData.append(key, newTeacher[key]);
        });

        postDataWithImage(URL + "/createTeacher", formData).then(
            (result) =>{
                if(result){
                    allTeachers.push(result);
                    setTeachers(allTeachers.slice());
                }
            }
        )
    }

    useEffect(() => {
        getTeachers();
    }, [])

    return(
        <div>     
            <ModalButton 
            btnName={'Добавить учителя'} 
            title={'Создание учителя'}
            clearList={() => {}}
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
                        <FormControl id="login" placeholder={`T${1000000000 + (allTeachers[allTeachers.length - 1]?.id !== undefined ? allTeachers[allTeachers.length - 1].id : 0)}`} type="text" disabled></FormControl>
                    </InputGroup>
                    <InputGroup className="mb-3">
                        <InputGroup.Text>Пароль</InputGroup.Text>
                        <FormControl id="password" type="text"></FormControl>
                    </InputGroup>
                    <InputGroup className="mb-3">
                        <InputGroup.Text>Аватарка</InputGroup.Text>
                        <input className="form-control form-control-sm" id="image" type="file" accept="image/*" onChange={e => setFile(e.target.files[0])} multiple/>
                    </InputGroup>
                    <Button type="submit" onClick={() => addTeacher()}>Создать!</Button>
                </Form>                  
                </div>
            }
            />
            {allTeachers.map(x => <TeacherItem key={x.id} teacher={x} deleteAction={deleteTeacher}/>)}
        </div>
    )
}

export default Teachers;

const TeacherItem = ({teacher, deleteAction}) => {
    return(
        <div style={{backgroundColor: 'whitesmoke', margin: '10px', borderRadius: '10px', padding: '10px'}}>
            <h3 className="mb-3">{teacher.firstName} {teacher.lastName} {teacher.middleName}</h3>
            <Accordion className="mb-3">
                <Accordion.Item eventKey="0">
                    <Accordion.Header>Предметы</Accordion.Header>
                        <Accordion.Body>
                            <ul className="list-group">
                                {teacher.subjects?.map((s) => 
                                    <li className="list-group-item" value={s} id={s.id}>{s.name}</li>)} 
                            </ul>
                        </Accordion.Body>
                </Accordion.Item>
            </Accordion> 
            <Button variant="danger" onClick={() => deleteAction(teacher.id)}>Удалить</Button>        
        </div>
    )
}