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

        const headerFromUser = document.querySelector('#header').value;
        const textFromUser = document.querySelector('#text').value;

        const newLesson = {
            header: headerFromUser,
            text: textFromUser
        };

        const headers = new Headers();
        headers.set('Content-Type', 'application/json');

        const options = {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(newLesson)
        };

        const result = await fetch(URL + "createLesson", options);
        if (result.ok){
            const lesson = await result.json();
            allLessons.push(lesson);
            setLessons(allLessons.slice());
        }
    }

    const getClases = async () => {

        const options = {
            method: 'GET',
            headers: new Headers()
        }
        const result = await fetch(`/api/classes/getClasses`, options);
        if(result.ok){
            const clases = await result.json();
            setClasses(clases);
            return clases;
        }
        return [];
    }

    useEffect(() => {
        getClases();
        getSchedule(currentDate, allClasses[allClasses.length - 1]);
    }, [])

    return(
        <div> 
            <div style={{display: 'flex'}}>
                <InputGroup className="ms-3">
                    <InputGroup.Text>Дата</InputGroup.Text>
                    <Form.Control type='date' id="date" defaultValue={currentDate}/>
                </InputGroup>
                 <Form.Select className="ms-3" id="classId" value={allClasses[allClasses.length]}>
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
                            {Object.entries(allLessons).map((value) => <LessonsItem key={value[1].id} date={value[0]} lessons={value[1]} addAction={addLesson}/>)}
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    )
}

export default Lessons;

const LessonsItem = ({lessons, date, addAction}) => {
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
                        <td colspan="4">
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
                                            <Form.Control id="disabledTextInput" placeholder={document.getElementById("classId").value}/>
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
                                            <Form.Control id="classroom" placeholder="Номер кабинета"/>
                                        </InputGroup>
                                        <InputGroup className="mb-3">
                                            <InputGroup.Text>Описание</InputGroup.Text>
                                            <Form.Control id="description" as="textarea" aria-label="Описание"/>
                                        </InputGroup>
                                        <InputGroup className="mb-3">
                                            <Form.Select aria-label="Предмет">

                                            </Form.Select>
                                        </InputGroup>
                                      <Button type="submit" onClick={() => addAction()}>Создать!</Button>
                                  </Form>                              
                                }
                            />
                        </td>
                    </tr> 
                </tbody>
            </table>
        </tr>
    )
}

