import { getData} from "../services/AccessAPI";
import { Accordion } from "react-bootstrap";
import SessionManager from "../auth/SessionManager";
import { Component } from "react";

const URL = `/api/scores`;
const URLClass = `/api/classes`;
const URLSubject = `/api/subjects`;
const typeUser = SessionManager.getUser();

ВСЕ ПРЕПИСАТЬ НА CONST

export default class Scores extends Component {
    constructor(props) {
      super(props);
      this.state = {
        user: Object,
        scores: [],
        classes: [],
        subjects: []
      };
    }

    componentDidMount(){
        let response = typeUser.userName[0] === "T" ? `/api/teachers/getTeacher/${typeUser.userId}` : `/api/students/getStudent/${typeUser.userId}`

        getData(response).then(
            (result) => {
                if(result){
                    this.setState({
                        user: result
                    });
                }
            }
        )

        if((typeUser.userName[0] === "T") == false){
            getData(URL + `/getScoresByIdStudent/${this.state.user.id}`).then(
                (result) => {
                    if(result){
                        this.setState({
                            scores: result
                        });
                    }
                }
            )

            console.log(this.state.user)
       
            getData(URLSubject + `/getSubjects`).then(
                (result) => {
                    if(result){
                        this.setState({
                            subjects: result
                        })
                    }
                }
            )
        }
        else{
            getData(URLClass + `/getClasses`).then(
                (result) => {
                    if(result){
                        this.setState({
                            classes: result
                        });
                    }
                }
            )
        }
    }

    getStudent = async (idStudent) => {
        getData(URL + `/getStudent/${idStudent}`).then(
            (result) => {
                return result;
            }
        ) 
    }

    getTableScores(){
        const allClasses = this.state.classes;

        if(typeUser.userName[0] === "T"){
            return(
                <div>
                  {allClasses.map(x => this.ClassItem(x.id, x, this.user.subject))}
                </div>
            )
        }
        else{
            return(
                <table className="table table-bordered">
                    <thead>
                        <tr>
                        <th scope="col">Предмет</th>
                        <th scope="col">Оценки</th>
                        </tr>
                    </thead>
                    <tbody>
                        {this.state.subjects.map(subject => (
                            <tr>
                                <td>{`${subject.name}`}</td>
                                <td>{this.GetStringScoresBySubject(subject)}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )
        }
    }

    ClassItem = ({_class, subjects}) => {
        return(
            <div style={{backgroundColor: 'whitesmoke', margin: '10px', borderRadius: '10px', padding: '10px'}}>
                <h3 className="mb-3">{_class.name}</h3>
                <Accordion className="mb-3">
                    <Accordion.Item eventKey="0">
                        <Accordion.Header>Предметы</Accordion.Header>
                            <Accordion.Body>
                                {subjects.map(subject => (
                                    (this.SubjectItem(subject, _class))
                                ))}
                            </Accordion.Body>
                    </Accordion.Item>
                </Accordion>     
            </div>
        )
    }
    
    SubjectItem = ({subject, _class}) => {
        return(
            <div style={{backgroundColor: 'whitesmoke', margin: '10px', borderRadius: '10px', padding: '10px'}}>
            <h3 className="mb-3">{_class.name}</h3>
            <Accordion className="mb-3">
                <Accordion.Item eventKey="0">
                    <Accordion.Header>Студенты</Accordion.Header>
                        <Accordion.Body>
                        <table className="table table-bordered">
                                <thead>
                                    <tr>
                                    <th scope="col">ФИО</th>
                                    <th scope="col">Оценки</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {_class.students?.map(student => (
                                        <tr>
                                            <td>{`${student.firstName} ${student.lastName} ${student.middleName}`}</td>
                                            <td>{this.GetStringScores(_class, subject)}</td>
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
    
    GetStringScores = (_class, subject) => {
        const scores = "";
    
        _class.students?.map(student =>
            this.getStudent(student.id).scores.filter((item) => item.idSubject === subject.id).array.forEach(score => {
                scores += score.grade + " "
            })
        )
    
        return scores;
    }
    
    GetStringScoresBySubject = (subject) => {
        const scores = "";
    
        this.state.scores.forEach(score => {
            if(score.idSubject == subject.id)
                scores += score.grade + " "
        });
    
        return scores;
    }

    render() {
        return(
            <div>
                {this.getTableScores()}
            </div>
        )
    }
}