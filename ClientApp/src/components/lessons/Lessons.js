import { useEffect } from "react";
import { useState } from "react";
import ModalButton from "../ModalBtn";
import { Button, Form, FormControl, InputGroup } from "react-bootstrap";
import { getData, postData } from "../services/AccessAPI";
import SessionManager from "../auth/SessionManager";

const URL = `/api/lessons`;
const fullDate = new Date();
const User = SessionManager.getUser();

const Lessons = () => {
    const [allLessons, setLessons] = useState([]);
    const [allClasses, setClasses] = useState([]);
    const [allSubjects, setSubjects] = useState([]);
    const [allTeachers, setTeachers] = useState([]);
    const [allStudents, setStudents] = useState([]);
    const [allScoreToAdd, setScore] = useState([]);
    const [subject, setSubject] = useState();
    const [teacher, setTeacher] = useState();
    const [idClasses, setIdClass] = useState();

    const getSchedule = async () => {
        let date = document.getElementById("currentDate").value 
        let idClass

        if(User.userRole === "0"){
            idClass = idClasses;
        }
        else
            idClass = document.getElementById("classId").value

        getData(URL + `/schedule/${date}/${idClass}`).then(
            (result) => {
                if(result){
                    setLessons(result);
                    return result;
                }
            }
        )
    }

    const getIdClass = async () => {
        getData(`/api/students/getStudent/${User.userId}`).then(
            (result) => {
                if(result){
                    setIdClass(result.class.id)
                }
            }
        )
    }

    const addLesson = async () => {
        const newLesson = {
            startTime: `${document.getElementById('startTime').value}:00`,
            endTime: `${document.getElementById('endTime').value}:00`,
            idClass:  Number(document.getElementById("classId").value),
            date: document.getElementById('correctDate').value,
            classRoom:  Number(document.getElementById('classRoom').value),
            description: document.getElementById('description').value,
            idSubject:  Number(document.getElementById('idSubject').value),
            idTeacher:  Number(document.getElementById('idTeacher').value),
            untilWhatDate: document.getElementById('untilWhatDate').value
        };

        postData(URL + `/createLessons`, newLesson).then(
            (result) =>{
                if(result){
                    allLessons.push(result);
                    setLessons(allLessons.slice());
                } 
            }
        )
    }

    const getClasses = async () => {
        getData(`/api/classes/getClasses`).then(
            (result) => {
                if(result){
                    setClasses(result);
                    return result;
                } 
            }
        )
    }

    const getStudents = async () => {
        getData(`/api/students/getStudents`).then(
            (result) => {
                if(result){
                    setStudents(result);
                    return result;
                } 
            }
        )
    }

    const getSubjects = async () =>  {   
        getData(`/api/subjects/getSubjects`).then(
            (result) => {
                if(result){
                    setSubjects(result);
                    return result;
                } 
            }
        ) 
    }

    const getTeachers = async () => {
        getData('/api/teachers/getTeachers').then(
            (result) => {
                if(result){
                    setTeachers(result);
                    return result;
                } 
            }
        ) 
    }

    const createScore = async () => {
        postData(`/api/scores/createScores`, allScoreToAdd).then(
            (result) => {
                if(result){
                    return result;
                } 
            }
        )
    }

    const changeTeacher = (value) => {
        if(value !== undefined){
            setTeacher(allTeachers.find(s => `${s.id}` === value));
        }
    }

    const changeSubject = (value) => {
        if(value !== undefined){
            setSubject(allSubjects.find(s => `${s.id}` === value));
            setTeacher(undefined);
        }
    }

    const getTable = () => {
        if(User.userRole === "2"){
            return(
                <div> 
                    <div style={{display: 'flex'}}>
                        <InputGroup className="ms-3">
                            <InputGroup.Text>Дата</InputGroup.Text>
                            <Form.Control onChange={getSchedule} type='date' id="currentDate"/>
                        </InputGroup>
                        <Form.Select onChange={getSchedule} className="ms-3" id="classId" defaultValue="">
                            <option selected>Класс</option>
                            {allClasses.map(_class => (
                                <option key={_class.id} value={_class.id}>{_class.name}</option>
                            ))}
                        </Form.Select>        
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
                                        addLesson={addLesson}
                                        addScore={createScore}
                                        getButtonAdd={getButtonAdd}
                                        buttonAddScore={buttonAddScore}
                                    />)} 
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            )
        }
        else if(User.userRole === "1"){
            let lessons = allLessons;

            if(Object.entries(allLessons).length > 1){
                lessons = Object.entries(allLessons).map((value => value[1]))
                let lesson = lessons.map(lessons => lessons.filter((item) => item.idTeacher === Number(User.userId)))
                lessons = Object.entries(allLessons)[1] = lesson
            }

            return(
                <div> 
                    <div style={{display: 'flex'}}>
                        <InputGroup className="ms-3">
                            <InputGroup.Text>Дата</InputGroup.Text>
                            <Form.Control onChange={getSchedule} type='date' id="currentDate"/>
                        </InputGroup>
                        <Form.Select onChange={getSchedule} className="ms-3" id="classId">
                            <option selected>Класс</option>
                            {allClasses.map(_class => (
                                <option value={_class.id}>{_class.name}</option>
                            ))}
                        </Form.Select>        
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
                                    {Object.entries(lessons).map((value) => <LessonsItem key={value[1].id} 
                                        date={value[0]} 
                                        lessons={value[1]} 
                                        addLesson={addLesson}
                                        addScore={createScore} 
                                        getButtonAdd={getButtonAdd}
                                        buttonAddScore={buttonAddScore}
                                    />)}
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            )
        }
        else{
            return(
                <div> 
                <div style={{display: 'flex'}}>
                    <InputGroup className="ms-3">
                        <InputGroup.Text>Дата</InputGroup.Text>
                        <Form.Control onChange={getSchedule} type='date' id="currentDate"/>
                    </InputGroup>
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
                                    addLesson={addLesson}
                                    addScore={createScore}
                                    getButtonAdd={getButtonAdd}
                                    buttonAddScore={buttonAddScore}
                                />)}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div> 
            )
        }
    }
    const getButtonAdd = (date, addAction) => {
        if(User.userRole === "2"){
            return(
                <tr>
                    <td colSpan="4">
                        <ModalButton 
                            btnName={'Добавить урок'} 
                            title={'Создание урока'}
                            clearList={() => {}}
                            modalContent={
                                <Form>
                                <fieldset disabled>
                                    <InputGroup className="mb-3">
                                        <InputGroup.Text>Дата</InputGroup.Text>
                                        <Form.Control id="correctDate" defaultValue={date} />
                                    </InputGroup>
                                    <InputGroup className="mb-3">
                                        <InputGroup.Text>Класс</InputGroup.Text>
                                        <Form.Control id="disabledTextInput" placeholder={(allClasses.find(value => `${value.id}` === document.getElementById("classId").value)).name}/>
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
                                        <Form.Control id="classRoom" type="number" placeholder="Номер кабинета"/>
                                    </InputGroup>
                                    <InputGroup className="mb-3">
                                        <InputGroup.Text>Описание</InputGroup.Text>
                                        <Form.Control id="description" type="text" as="textarea" aria-label="Описание"/>
                                    </InputGroup>
                                    <InputGroup className="mb-3">
                                        <Form.Select onChange={(e) => changeSubject(e.target.value)} id="idSubject" aria-label="Предмет">
                                            <option selected>Предмет</option>
                                            {(teacher === undefined ? allSubjects : teacher.subjects).map((s) =>
                                            <option value={s.id} key={s.id}>{s.name}</option>) }            
                                        </Form.Select>
                                    </InputGroup>
                                    <InputGroup className="mb-3" id="teachers" disabled>
                                        <Form.Select onChange={(e) => changeTeacher(e.target.value)} id="idTeacher" aria-label="Учитель" > 
                                            <option selected>Учитель</option> 
                                            {(subject === undefined ? allTeachers : subject.teachers).map((t) => 
                                            <option value={t.id} id={t.id}>{`${t.firstName} ${t.lastName} ${t.middleName}`}</option>)} 
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
                    </td>
                </tr> 
            )
        }
    }

    const buttonAddScore = (date, addAction, lesson) => {
        if(User.userRole !== "0"){
            let students = allStudents.filter((item) => item.idClass === (allClasses.find(value => `${value.id}` === document.getElementById("classId").value)).id)
            return(
                <ModalButton 
                    btnName={'Добавить оценку'} 
                    title={'Создание оценки'}
                    clearList={() => {}}
                    modalContent={
                        <Form>
                        <fieldset disabled>
                            <InputGroup className="mb-3">
                                <InputGroup.Text>Дата</InputGroup.Text>
                                <Form.Control id="correctDate" defaultValue={date} />
                            </InputGroup>
                            <InputGroup className="mb-3">
                                <InputGroup.Text>Класс</InputGroup.Text>
                                <Form.Control id="disabledTextInput" placeholder={(allClasses.find(value => `${value.id}` === document.getElementById("classId").value)).name}/>
                            </InputGroup>
                        </fieldset>
                            <table className="table table-bordered">
                                <thead>
                                    <tr>
                                        <th scope="col">ФИО</th>
                                        <th scope="col">Оценка</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {students.map(student => (
                                        <tr>
                                            <td>{`${student.firstName} ${student.lastName} ${student.middleName}`}</td>
                                            <td>{choiceScore(student.id, lesson.id)}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                            <Button type="submit" onClick={() => addAction()}>Cохранить!</Button>
                        </Form>                              
                    }
                />
            )
        }
        else{
            var score =lesson.scores.find(item => item.idStudent == User.userId);

            return(score === undefined ? "" : score.grade)
        }
    }

    const choiceScore = (idStudent, lessonId) =>{
        return(
            <Form.Select onChange={(e) => addScore(idStudent, lessonId, e.target.value)} id="score" aria-label="Оценка">
                <option selected value="0">Выберете оценку</option>
                <option value="2">2</option>
                <option value="3">3</option>
                <option value="4">4</option>
                <option value="5">5</option>
            </Form.Select>
        )
    }

    function addScore(idStudent, lessonId, value){
        setScore(allScoreToAdd.filter((item) => !item.includes(`${idStudent}/${lessonId}/`)))

        allScoreToAdd.push(`${idStudent}/${lessonId}/${value}`)

        setScore(allScoreToAdd);
    }
 
    useEffect(() => {
        getClasses();
        getSubjects();
        getTeachers();
        getStudents();
        getIdClass(User.userId);
    }, [])

    return(
        getTable()
    )
}

export default Lessons;

const LessonsItem = ({lessons, date, addLesson, addScore, getButtonAdd, buttonAddScore}) => {
    return(
        <tr>
            <th scope="row">{date}</th>
            <table className ="table table table-sm bg-light">
                <thead className ="thead-dark">
                    <tr>
                        <th scope="col" className ="col-md-1">Время</th>
                        <th scope="col" className ="col-md-2">Предмет</th>
                        <th scope="col" className ="col-md-1">Кабинет</th>
                        <th scope="col" className ="col-md-3">Преподаватель</th>
                        <th scope="col" className ="col-md-1">Оценка</th>
                    </tr>
                </thead>
                <tbody>
                    {lessons.map(lesson => (
                        <tr>
                            <th>{lesson.startTime} - {lesson.endTime}</th>
                            <td>{lesson.subject?.name}</td>
                            <td>{lesson.classRoom}</td>
                            <td>{lesson.teacher?.lastName} {lesson.teacher?.firstName} {lesson.teacher?.middleName}</td>
                            <td>{buttonAddScore(date, addScore, lesson)}</td>
                        </tr>
                    ))}
                    {getButtonAdd(date, addLesson)}                 
                </tbody>
            </table>
        </tr>
    )
}