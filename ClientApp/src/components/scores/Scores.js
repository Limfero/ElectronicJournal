import { getData} from "../services/AccessAPI";
import { Accordion } from "react-bootstrap";
import SessionManager from "../auth/SessionManager";
import { useEffect } from "react";
import { useState } from "react";

const URL = `/api/scores`;
const URLClass = `/api/classes`;
const URLSubject = `/api/subjects`;
const URLTeachers= `/api/teachers`;
const URLStudents = `/api/students`;
const User = SessionManager.getUser();

const Scores = () => {
    const [allClasses, setClasses] = useState([]);
    const [allSubjects, setAllSubjects] = useState([]);
    const [allScores, setAllScores] = useState([]);
    const [teacher, setTeacher] = useState();
    const [student, setStudent] = useState(); 

    const getSubjects = async () => {   
        getData(URLSubject + `/getSubjects`).then(
            (result) => {
                if(result){
                    setAllSubjects(result);
                }
            }
        )
    }

    const getAllScores = async () => {   
        getData(URL + `/getScores`).then(
            (result) => {
                if(result){
                    setAllScores(result);
                }
            }
        )
    }

    const getClasses = async () => { 
        getData(URLClass + `/getClasses`).then(
            (result) => {
                if(result){
                    setClasses(result);
                }
            }
        )
    }

    const getStudent = (id) => { 
        getData(URLStudents + `/getStudent/${id}`).then(
            (result) => {
                if(result){
                    setStudent(result)
                }
            }
        )
    }


    const getTeacherById = async (id) => { 
        getData(URLTeachers + `/getTeacher/${id}`).then(
            (result) => {
                if(result){
                    setTeacher(result)
                }
            }
        )
    }

    const getTableScores = () => {
        if(User.userRole === "2"){
            return(
                <div>
                    {allClasses.map(x => <ClassItem key={x.id} _class={x} subjects={allSubjects} getStringScores={GetStringScores}/>)}
                </div>
            )
        }
        else if(User.userRole === "1"){
            getTeacherById(User.userId);
            return(
                <div>
                  {allClasses.map(x => <ClassItem key={x.id} _class={x} subjects={teacher.subjects} getStringScores={GetStringScores}/>)}
                </div>
            )
        }
        else{
            return(
                <table className="table table-bordered">
                    <thead>
                        <tr>
                        <th scope="col" className ="col-md-2">Предмет</th>
                        <th scope="col">Оценки</th>
                        <th scope="col" className ="col-md-1">Средний балл</th>
                        </tr>
                    </thead>
                    <tbody>
                        {allSubjects.map(subject => (
                            <tr>
                                <td>{`${subject.name}`}</td>
                                <td>{GetStringScores(student, subject)}</td>
                                <td>{GetAverrageScore(student, subject)}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )
        }
    }

    const GetStringScores = (student, subject) => {
        var currentScores = "";

        if(student !== undefined)
            student.scores.forEach((score) => {
                score = allScores.find(s => s.id === score.id)

                if(score !== undefined && score.lesson.idSubject === subject.id)
                    currentScores += score.grade + " "
            })

    
        return currentScores;
    }

    const GetAverrageScore = (student, subject) =>{
        var averrageScore = 0;

        if(student !== undefined)
            student.scores.forEach((score) => {
                score = allScores.find(s => s.id === score.id)

                if(score !== undefined && score.lesson.idSubject === subject.id)
                    averrageScore += score.grade
            })

    
        return averrageScore;
    }

    useEffect(() => {
        getSubjects();
        getClasses();
        getAllScores();
        getStudent(User.userId);
    }, [])

    return(
        <div>
            {getTableScores()}
        </div>
    )

}

export default Scores;

const ClassItem = ({_class, subjects, getStringScores}) => {
    return(
        <div style={{backgroundColor: 'whitesmoke', margin: '10px', borderRadius: '10px', padding: '10px'}}>
            <h3 className="mb-3">{_class.name}</h3>
            <Accordion className="mb-3">
                <Accordion.Item eventKey="0">
                    <Accordion.Header>Предметы</Accordion.Header>
                        <Accordion.Body>
                            {subjects.map(subject => (<SubjectItem subject={subject} _class={_class} getStringScores={getStringScores}/>))}
                        </Accordion.Body>
                </Accordion.Item>
            </Accordion>     
        </div>
    )
}

const SubjectItem = ({subject, _class, getStringScores}) => {
    return(
        <div style={{backgroundColor: 'whitesmoke', margin: '10px', borderRadius: '10px', padding: '10px'}}>
        <h3 className="mb-3">{subject.name}</h3>
        <Accordion className="mb-3">
            <Accordion.Item eventKey="0">
                <Accordion.Header>Студенты</Accordion.Header>
                    <Accordion.Body>
                    <table className="table table-bordered">
                            <thead>
                                <tr>
                                    <th scope="col"className ="col-md-3">ФИО</th>
                                    <th scope="col">Оценки</th>
                                </tr>
                            </thead>
                            <tbody>
                                {_class.students?.map(student => (
                                    <tr>
                                        <td>{`${student.firstName} ${student.lastName} ${student.middleName}`}</td>
                                        <td>{getStringScores(student, subject)}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </Accordion.Body>
            </Accordion.Item>
        </Accordion>     
    </div>
    )
}