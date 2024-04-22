import { Component } from "react";
import { getData } from "../services/AccessAPI";
import { useParams } from "react-router-dom";

const URL = '/api/students'
const URLSubjects = `/api/subjects`;

function withParams(Component) {
    return (props) => <Component {...props} params={useParams()} />;
  }

class Student extends Component {
    constructor(props) {
        super(props);
        this.state = {
            lastName: "",
            firstName: "",
            middleName: "",
            className: "",
            scores: [],
            allSubject: [],
            scores: [],
            loading: true
        };

        this.getScores = this.getScores.bind(this);
    }

    componentDidMount(){
        const idStudent = this.props.params.id;

        getData(URL + `/getStudent/${idStudent}`).then(
            (result) => {
                if(result){
                    this.setState({
                        firstName: result.firstName,
                        lastName: result.lastName,
                        middleName: result.middleName,
                        className: result.class.name,
                        scores: result.scores,
                        loading: false
                    })
                }
            }
        )

        getData(URLSubjects + `/getSubjects`).then(
            (result) => {
                if(result){
                    this.setState({
                        allSubject: result
                    })
                }
            }
        )
    }


    getScores(scores){
        const newScores = new Map();

        this.state.allSubject.forEach(element => {
            newScores.set(element.name, [])
        })

        scores.forEach(element => {
            if(newScores.has(element.lesson.subject.name))
                newScores.get(element.lesson.subject.name).push(element.grade)
        });

        return newScores
    }

    getStringScore(scores){
        let allScores = ""

        scores.map(value => allScores += value + " ")

        return allScores;
    }

    getAwerrageScore(scores){
        let averrageScore = 0;

        scores.map(value => averrageScore += value)

        return averrageScore;
    }

    render() {
        const scores = Object.fromEntries(this.getScores(this.state.scores));

        return(
            <div>
                <div>
                    <h2>{this.state.firstName}</h2>
                    <h2>{this.state.lastName}</h2>
                    <h2>{this.state.middleName}</h2>{' '}
                </div>

                <p>Класс: {this.state.className}</p>
                <div className ="container">
                    <div className ="row g-3 p-2">
                        <table className ="table table-bordered">
                            <thead className ="thead-dark">
                                <tr>
                                    <th scope="col" className ="col-md-2">Предмет</th>
                                    <th scope="col">Оценки</th>
                                    <th scope="col" className ="col-md-1">Средний балл</th>
                                </tr>
                            </thead>
                            <tbody>
                                {Object.entries(scores).map(item => {
                                    return <tr>
                                        <td>{item[0]}</td>
                                        <td>{this.getStringScore(item[1])}</td>
                                        <td>{this.getAwerrageScore(item[1])}</td>
                                    </tr>
                                })}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        );
    }

}

export default withParams(Student);