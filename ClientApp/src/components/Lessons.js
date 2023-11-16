import { useEffect } from "react";
import { useState } from "react";
import ModalButton from "./ModalBtn";
import { Button, Form, FormControl, InputGroup } from "react-bootstrap";


const URL = `/api/lessons`;
const fullDate = new Date();
const currentDate = fullDate.getFullYear() + '-' + fullDate.getMonth() + '-' + fullDate.getDay();

const Lessons = () => {
    const [allLessons, setLessons] = useState([]);
    const [allClasses, setClasses] = useState([]);
    const [allSubjects, setSubjects] = useState([]);
    const [allTeachers, setTeachers] = useState([]);
    const [subject, setSubject] = useState();
    const [teacher, setTeacher] = useState();

    const getSchedule = async (date, idClass) => {
        const headers = new Headers();
        headers.set('Content-Type', 'application/json');

        const options = {
            method: 'GET',
            headers: headers,
        }

        const result = await fetch(URL + `/schedule/${date}/${idClass}`, options);
        if(result.ok){
            const lessons = await result.json();
            setLessons(lessons);
            return lessons;
        }
        return [];
    }

    const addLesson = async () => {
        const newLesson = {
            startTime: document.getElementById('startTime').value,
            endTime: document.getElementById('endTime').value,
            idClass: document.getElementById("classId").value,
            date: document.getElementById('date').value,
            classRoom: document.getElementById('classRoom').value,
            description: document.getElementById('description').value,
            idSubject: document.getElementById('idSubject').value,
            idTeacher: document.getElementById('idTeacher').value
        };

        const untilWhatDate = document.getElementById('untilWhatDate').value;

        const headers = new Headers();
        headers.set('Content-Type', 'application/json');

        const options = {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(newLesson, untilWhatDate)
        };

        const result = await fetch(URL + "createLessons", options);
        if (result.ok){
            const lesson = await result.json();
            allLessons.push(lesson);
            setLessons(allLessons.slice());
        }
    }

    const getClasses = async () => {
        const options = {
            method: 'GET',
            headers: new Headers()
        }

        const result = await fetch(`/api/classes/getClasses`, options);

        if(result.ok){
            const classes = await result.json();
            setClasses(classes);
            return classes;
        }
        return [];
    }

    const getSubjects = async () =>  {    
        const options = {
            method: 'GET',
            headers: new Headers()
        }

        const result = await fetch(`/api/subjects/getSubjects`, options);

        if(result.ok){
            const subjects = await result.json();
            setSubjects(subjects)
            return subjects;
        }
        return [];
    }

    const getTeachers = async () => {
        const options = {
            method: 'GET',
            headers: new Headers()
        }

        const result = await fetch(URL + '/getTeachers', options);

        if(result.ok){
            const teachers = await result.json();
            setTeachers(teachers)
            return teachers;
        }
        return [];
    }

    const changeTeacher = ({target: {value}}) => setTeacher(value)

    const changeSubject = ({target: {value}}) => {
        setSubject(value);
        setTeacher("");
    }

    useEffect(() => {
        getSchedule();
        getClasses();
        getSubjects();
        getTeachers();
    }, [])

    return(
        <div> 
            <div style={{display: 'flex'}}>
                <InputGroup className="ms-3">
                    <InputGroup.Text>Дата</InputGroup.Text>
                    <Form.Control type='date' id="date" defaultValue={currentDate}/>
                </InputGroup>
                 <Form.Select className="ms-3" id="classId" defaultValue="">
                    <option selected>Класс</option>
                    {allClasses.map(_class => (
                        <option value={_class.id}>{_class.name}</option>
                    ))}
                </Form.Select>
                <Button className="ms-3" onClick={() => getSchedule(document.getElementById("date").value, document.getElementById("classId").value)}>
                    Получить информацию
                </Button>            
            </div>

            <div className ="container">
                <div className ="row g-3 p-2">
                    <table className ="table border bg-light">
                        <thead className ="thead-dark">
                            <tr>
                                <th scope="col" className ="col-md-1">Дата</th>
                                <th scope="col">Расписание</th>
                            </tr>
                        </thead>
                        <tbody>
                            {Object.entries(allLessons).map((value) => <LessonsItem key={value[1].id} 
                                date={value[0]} 
                                lessons={value[1]} 
                                addAction={addLesson} 
                                allClasses={allClasses} 
                                allSubjects={allSubjects} 
                                allTeachers = {allTeachers}
                                teacher={teacher} 
                                subject={subject} 
                                changeSubject={changeSubject} 
                                changeTeacher={changeTeacher}
                            />)}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    )
}

export default Lessons;

const LessonsItem = ({lessons, date, addAction, allTeachers, allClasses, allSubjects, teacher, subject, changeSubject, changeTeacher}) => {
    return(
        <tr>
            <th scope="row">{date}</th>
            <table className ="table table table-sm bg-light">
                <thead className ="thead-dark">
                    <tr>
                        <th scope="col" className ="col-md-1">Время</th>
                        <th scope="col">Предмет</th>
                        <th scope="col">Кабинет</th>
                        <th scope="col">Преподаватель</th>
                    </tr>
                </thead>
                <tbody>
                    {lessons.map(lesson => (
                        <tr>
                            <th>{lesson.startTime} - {lesson.endTime}</th>
                            <td>{lesson.subject?.name}</td>
                            <td>{lesson.classRoom}</td>
                            <td>{lesson.teacher?.firstname} {lesson.teacher?.lastname} {lesson.teacher?.middlename}</td>
                        </tr>
                    ))} 
                    <tr>
                        <td colSpan="4">
                            <ButtonCreateLesson 
                                date={date} 
                                addAction={addAction} 
                                allClasses={allClasses} 
                                allSubjects={allSubjects} 
                                allTeachers = {allTeachers}
                                teacher={teacher} 
                                subject={subject} 
                                changeSubject={changeSubject} 
                                changeTeacher={changeTeacher}
                            />
                        </td>
                    </tr> 
                </tbody>
            </table>
        </tr>
    )
}

const ButtonCreateLesson = ({date, addAction, allClasses, allTeachers, allSubjects, teacher, subject, changeSubject, changeTeacher}) => {
    return(
        <ModalButton 
            btnName={'Добавить урок'} 
            title={'Создание урока'}
            modalContent={
                <Form>
                <fieldset disabled>
                    <InputGroup className="mb-3">
                        <InputGroup.Text>Дата</InputGroup.Text>
                        <Form.Control id="disabledTextInput" placeholder={date} />
                    </InputGroup>
                    <InputGroup className="mb-3">
                        <InputGroup.Text>Класс</InputGroup.Text>
                        <Form.Control id="disabledTextInput" placeholder={allClasses.find(value => value.id === document.getElementById("classId").value).name}/>
                    </InputGroup>
                </fieldset>
                    <InputGroup className="mb-3">
                        <InputGroup.Text>Начало урока</InputGroup.Text>
                        <FormControl id="startTime" type="time"></FormControl>
                    </InputGroup>
                    <InputGroup className="mb-3">
                        <InputGroup.Text>Конец урока</InputGroup.Text>
                        <FormControl id="endTime" type="time"></FormControl>
                    </InputGroup>
                    <InputGroup className="mb-3">
                        <InputGroup.Text>Кабинет</InputGroup.Text>
                        <Form.Control id="classroom" type="number" placeholder="Номер кабинета"/>
                    </InputGroup>
                    <InputGroup className="mb-3">
                        <InputGroup.Text>Описание</InputGroup.Text>
                        <Form.Control id="description" type="number" as="textarea" aria-label="Описание"/>
                    </InputGroup>
                    <InputGroup className="mb-3">
                        <Form.Select onChange={changeSubject} id="idSubject" aria-label="Предмет">
                            {(subject === null ? allSubjects : teacher.subjects).map((s) =>
                            <option value={s} key={s.id}>{s.name}</option>) }            
                        </Form.Select>
                    </InputGroup>
                    <InputGroup className="mb-3" id="teachers" disabled>
                        <Form.Select onChange={changeTeacher} id="idTeacher" aria-label="Учитель" >  
                            {(teacher === null ? allTeachers : subject.teachers).map((t) => 
                            <option value={t} id={t.id}>{`${t.firstName} ${t.lastName} ${t.middleName}`}</option>)} 
                        </Form.Select>
                    </InputGroup>
                    <InputGroup className="mb-3">                       
                        <InputGroup.Text>До какого числа</InputGroup.Text>
                        <Form.Control type='date' id="untilWhatDate" defaultValue={fullDate}/>
                    </InputGroup>   
                    <Button type="submit" onClick={() => addAction()}>Создать!</Button>
                </Form>                              
            }
        />
    )
}

