import { Component } from "react";
import { getData } from "../services/AccessAPI";
import { useParams } from "react-router-dom";

const URL = '/api/students'

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
                else{
                    window.location.assign("/logout");
                }
            }
        )
    }

    getScores(allScores){
        const newScores = new Map();

        allScores.forEach(element => {
            if(newScores.get(element.subject.name))
                newScores[element.subject.name] = `${newScores[element.subject.name]} ${element.grade}`
            else
                newScores.set(element.subject.name, element)
        });

        return newScores;
    }

    render() {
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
                        <table className ="table border bg-light">
                            <thead className ="thead-dark">
                                <tr>
                                    <th scope="col" className ="col-md-1">Предмет</th>
                                    <th scope="col">Оценки</th>
                                </tr>
                            </thead>
                            <tbody>
                                {Object.entries(this.getScores(this.state.scores)).map((name, score) => 
                                <tr>
                                    <th>{name}</th>
                                    <th>{score}</th>
                                </tr>)}
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        );
    }

}

export default withParams(Student);