import { useEffect } from "react";
import { useState } from "react";
import ModalButton from "./ModalBtn";
import { Button, Form, FormControl, InputGroup, Accordion } from "react-bootstrap";

const URL = `/api/subjects`;

const Subjects = () => {
    const [allSubjects, setSubjects] = useState([]);
    const [allTeachers, setAllTeacher] = useState([]);
    const [teachers, setTeachers] = useState([]);

    const getSubjects = async () => {
        const options = {
            method: 'GET',
            headers: new Headers()
        }

        const result = await fetch(URL + `/getSubjects`, options);

        if(result.ok){
            const subjects = await result.json();
            setSubjects(subjects)
            return subjects;
        }
        return [];
    }

    const addSubject = async () => {
        const newSubject = {
            name: document.getElementById('name').value,
            description: document.getElementById('description').value,
            teachers: teachers
        };

        const headers = new Headers();
        headers.set('Content-Type', 'application/json');

        const options = {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(newSubject)
        };

        const result = await fetch(URL + "/createSubject", options);
        if (result.ok){
            const subject = await result.json();
            allSubjects.push(subject);
            setSubjects(allSubjects.slice());
        }
    }

    const deleteSubject = (id) =>{
        const options = {
            method: 'DELETE',
            headers: new Headers()
        };

        const result = fetch(URL + `/deleteSubject/${id}`, options);
        if (result.ok){
            setSubjects(allTeachers.filter(x => x.id !== id));
        }
    }

    const getTeachers = async () => {
        const options = {
            method: 'GET',
            headers: new Headers()
        }

        const result = await fetch('/api/teachers/getTeachers', options);

        if(result.ok){
            const teachers = await result.json();
            setAllTeacher(teachers)
            return teachers;
        }
        return [];
    }

    const changeTeachers = ({target: {value}}) => {
        teachers.push(allTeachers.find(t => `${t.id}` === value));
        setTeachers(teachers.slice());
    }

    const clearList = () => setTeachers([]);

    useEffect(() => {
        getSubjects();
        getTeachers();
    }, [])

    return(
        <div>     
            <ModalButton 
            btnName={'Добавить предмет'} 
            title={'Создание предмета'}
            clearList={clearList}
            modalContent={
                <div>
                    <Form>
                    <InputGroup className="mb-3">
                        <InputGroup.Text>Название</InputGroup.Text>
                        <FormControl id="name" type="text"></FormControl>
                    </InputGroup>
                    <InputGroup className="mb-3">
                        <InputGroup.Text>Описание</InputGroup.Text>
                        <FormControl id="description" type="textarea"></FormControl>
                    </InputGroup>
                    <InputGroup className="mb-3">
                        <InputGroup.Text>Учителя</InputGroup.Text>
                        <Form.Select onChange={changeTeachers} id="idTeacher" aria-label="Учитель" >   
                            <option selected disabled>Учитель</option>
                            {allTeachers.map((t) => 
                            <option value={t.id} id={t.id}>{`${t.firstName} ${t.lastName} ${t.middleName}`}</option>)} 
                        </Form.Select>
                    </InputGroup>
                    <ul className="list-group mb-3">
                        {teachers.map((t) => 
                        <li className="list-group-item" value={t} id={t.id}>{`${t.firstName} ${t.lastName} ${t.middleName}`}</li>)}
                    </ul>
                    <Button type="submit" onClick={() => addSubject()}>Создать!</Button>
                </Form>                  
                </div>
            }
            />
            {allSubjects.map(x => <SubjectItem key={x.id} subject={x} deleteAction={deleteSubject}/>)}
        </div>
    )
}

export default Subjects;

const SubjectItem = ({subject, deleteAction}) => {
    return(
        <div style={{backgroundColor: 'whitesmoke', margin: '10px', borderRadius: '10px', padding: '10px'}}>
            <h3>{subject.name}</h3>
            <p>{subject.description}</p>
            <Accordion className="mb-3">
                <Accordion.Item eventKey="0">
                    <Accordion.Header>Преподаватели</Accordion.Header>
                        <Accordion.Body>
                            <ul className="list-group mb-3">
                                {subject.teachers?.map((t) => 
                                    <li className="list-group-item" value={t} id={t.id}>{`${t.firstName} ${t.lastName} ${t.middleName}`}</li>)}
                            </ul>
                        </Accordion.Body>
                </Accordion.Item>
            </Accordion>
            <Button variant="danger" onClick={() => deleteAction(subject.id)}>Удалить</Button>
        </div>
    )
}
