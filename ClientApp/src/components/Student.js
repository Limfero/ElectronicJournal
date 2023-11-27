import { useEffect } from "react";
import { useState } from "react";
import { useParams } from "react-router-dom";

const URL = '/api/students'

const Student = () =>
{
    const params = useParams();
    const idStudent = Number(params.id);
    const [student, setStudent] = useState();

    const getStudent = async () => {
        const options = {
            method: 'GET',
            headers: new Headers()
        }

        const result = await fetch(URL + `/getStudent/${idStudent}`, options);
        if(result.ok){
            const student = await result.json();
            setStudent(student)
            return student;
        }
        return [];
    }

    function getScores(allScores){
        const newScores = new Map();

        allScores.forEach(element => {
            if(newScores.get(element.subject.name))
                newScores[element.subject.name] = `${newScores[element.subject.name]} ${element.grade}`
            else
                newScores.set(element.subject.name, element)
        });

        return newScores;
    }

    useEffect(() => {
        getStudent();
    });

    if(student!==undefined){
        return(
            <div>
                <h2>{student.firstName}</h2>
                <h2>{student.lastName}</h2>
                <h2>{student.middleName}</h2>

                <p>Класс: {student.class.name}</p>
                <div className ="container">
                    <div className ="row g-3 p-2">
                        <table className ="table border bg-light">
                            <thead className ="thead-dark">
                                <tr>
                                    <th scope="col" className ="col-md-1">Предмет</th>
                                    <th scope="col">Оценки</th>
                                </tr>
                            </thead>
                            <tbody>
                                {Object.entries(getScores(student.scores)).map((name, score) => 
                                <tr>
                                    <th>{name}</th>
                                    <th>{score}</th>
                                </tr>)}
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
                <h2>Такого ученика нет!</h2>
            </div>
        )
    }
}

export default Student;