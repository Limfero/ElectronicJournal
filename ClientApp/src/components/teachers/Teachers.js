import { useEffect } from "react";
import { useState } from "react";
import ModalButton from "../ModalBtn";
import { Button, Form, FormControl, InputGroup, Accordion} from "react-bootstrap";
import { getData, postData, deleteData } from "../services/AccessAPI";

const URL = `/api/teachers`;

const Teachers = () => {
    const [allTeachers, setTeachers] = useState([]);
    const [allSubjects, setAllSubjects] = useState([]);
    const [subjects, setSubjects] = useState([]);
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

        const formData = new FormData()
        formData.append("image", file)

        const newTeacher = {
            firstName: document.getElementById('firstName').value,
            lastName: document.getElementById('lastName').value,
            middleName: document.getElementById('middleName').value,
            login: `T${1000000000 + (allTeachers[allTeachers.length - 1]?.id !== undefined ? allTeachers[allTeachers.length - 1].id : 0)}`,
            password: document.getElementById('password').value,
            image: formData,
            role: 1,
            subjects: subjects
        };

        postData(URL + "/createTeacher", newTeacher).then(
            (result) =>{
                if(result){
                    allTeachers.push(result);
                    setTeachers(allTeachers.slice());
                }
            }
        )
    }
    
    const getSubjects = async () => {
        getData(`/api/subjects/getSubjects`).then(
            (result) => {
                if(result){
                    setAllSubjects(result)
                    return result;
                }
            }
        )
    }

    const changeSubjects = ({target: {value}}) => {
        const subject = allSubjects.find(s => `${s.id}` === value)
        subjects.push(subject);
        setSubjects(subjects.slice());
    }

    const clearList = () => setSubjects([]);

    useEffect(() => {
        getTeachers();
        getSubjects();
    }, [])

    return(
        <div>     
            <ModalButton 
            btnName={'Добавить учителя'} 
            title={'Создание учителя'}
            clearList={clearList}
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
                        <input className="form-control form-control-sm" id="image" type="file" accept="image/*" onChange={e => setFile(e.target.files[0])}/>
                    </InputGroup>
                    <InputGroup className="mb-3">
                        <InputGroup.Text>Предметы</InputGroup.Text>                            
                        <Form.Select onChange={changeSubjects} id="idSubject" aria-label="Предмет" >   
                            <option selected disabled>Предмет</option>
                            {allSubjects.map(s => 
                            <option value={s.id} id={s.id}>{s.name}</option>)} 
                        </Form.Select>
                    </InputGroup>
                    <ul className="list-group mb-3">
                        {subjects.map((s) => 
                        <li className="list-group-item" value={s} id={s.id}>{s.name}</li>)}
                    </ul>
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