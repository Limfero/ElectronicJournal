import { useEffect } from "react";
import { useState } from "react";
import ModalButton from "./ModalBtn";
import { Button, Form, FormControl, InputGroup, Accordion } from "react-bootstrap";

const URLClass = `/api/classes`;
const URLStudent = `/api/students`;

const Classes = () => {
    const [allClases, setClasses] = useState([]);
    const [allStudents, setStudents] = useState([]);

    const getClasses = async () => {
        const options = {
            method: 'GET',
            headers: new Headers()
        }

        const result = await fetch(URLClass + `/getClasses`, options);

        if(result.ok){
            const classes = await result.json();
            setClasses(classes)
            return classes;
        }
        return [];
    };

    const addClass = async () => {
        const newClass = {
            name: `${document.getElementById('name').value}`,
        };

        const headers = new Headers();
        headers.set('Content-Type', 'application/json');

        const options = {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(newClass)
        };

        const result = await fetch(URLClass + "/createClass", options);
        if (result.ok){
            const _class = await result.json();
            allClases.push(_class);
            setClasses(allClases.slice());
        }
    }

    const deleteClass = (id) => {
        const options = {
            method: 'DELETE',
            headers: new Headers()
        };

        const result = fetch(URLClass + `/deleteClass/${id}`, options);
        if (result.ok){
            setClasses(allClases.filter(x => x.id !== id));
        }
    }

    const getStudents = async () => {
        const options = {
            method: 'GET',
            headers: new Headers()
        }

        const result = await fetch(URLStudent + `/getStudents`, options);

        if(result.ok){
            const students = await result.json();
            setStudetns(students)
            return students;
        }
        return [];
    };
    
    const addStudent = async () => {
        const newStudent = {
            firsName: `${document.getElementById('firsName').value}`,
            lastName: `${document.getElementById('lastName').value}`,
            middleName: `${document.getElementById('middleName').value}`,
            login: `S${1000000000 + (allStudents[allStudents.length - 1]?.id !== undefined ? allStudents[allStudents.length - 1].id : 0)}`,
            password: `${document.getElementById('password').value}`,
            idClass: `${document.getElementById('class').value}`,
        };

        const headers = new Headers();
        headers.set('Content-Type', 'application/json');

        const options = {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(newStudent)
        };

        const result = await fetch(URLStudent + "/createStudent", options);
        if (result.ok){
            const student = await result.json();
            allStudents.push(student);
            setClasses(allStudents.slice());
        }
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
            {allClases.map(x => <ClassItem key={x.id} _class={x} deleteAction={deleteClass} addAction={addStudent} allStudents={allStudents}/>)}
        </div>
    )
}

export default Classes;

const ClassItem = ({_class, deleteAction, addAction, allStudents}) => {
    return(
        <div style={{backgroundColor: 'whitesmoke', margin: '10px', borderRadius: '10px', padding: '10px'}}>
            <h3 className="mb-3">{_class.name}</h3>
            <Accordion className="mb-3">
                <Accordion.Item eventKey="0">
                    <Accordion.Header>Студенты</Accordion.Header>
                        <Accordion.Body>
                            <ul className="list-group">
                                {_class.students?.map((s) => 
                                    <StudentItem student={s}></StudentItem>)}
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
                                        <InputGroup.Text>Класс</InputGroup.Text>
                                        <FormControl id="class" type="text" disabled placeholder={_class.name} value={_class.id}></FormControl>
                                    </InputGroup>
                                    <Button type="submit" onClick={() => addAction()}>Создать!</Button>
                                </Form>                  
                                </div>
                            }
                            />
                        </Accordion.Body>
                </Accordion.Item>
            </Accordion> 
            <Button onClick={() => deleteAction(_class.id)}>Удалить</Button>        
        </div>
    )
};

const StudentItem = (student) => {
    return(
        <div style={{backgroundColor: 'whitesmoke', margin: '10px', borderRadius: '10px', padding: '10px'}}>
            <NavLink tag={Link} className="text-dark" to={`/student/${s.id}`}>
                <li className="list-group-item" value={s} id={s.id}>{s.firstName} {s.lastName} {s.middleName}</li>
            </NavLink>
        </div>
    )
}