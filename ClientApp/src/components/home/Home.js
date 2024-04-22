import { Component } from "react";
import { getData } from "../services/AccessAPI";
import SessionManager from "../auth/SessionManager";
import { Button } from "reactstrap";

const user = SessionManager.getUser();

export default class Home extends Component {
    constructor(props) {
      super(props);
      this.state = {
          lastName: "",
          firstName: "",
          middleName: "",
          image: ""
      };
    }

    componentDidMount(){
      let response = user.userName[0] === "T" ? `/api/teachers/getTeacher/${user.userId}` : `/api/students/getStudent/${user.userId}`

      getData(response).then(
        (result) => {
            if(result){
                this.setState({
                    firstName: result.firstName,
                    lastName: result.lastName,
                    middleName: result.middleName,
                    image: result.imagePath
                })
            }
            else{
               window.location.assign("/logout");
            }
          }
      )
    }

    logout(){
      if (SessionManager.getToken()){
        window.location.assign("/logout");
      }
    }

    render() {
      return(
          <div>
              <div className="d-flex justify-content-between">
              <img src={this.image} class="img-thumbnail" alt="avatar"/>
                <div>
                  <h2>{this.state.firstName}</h2>
                  <h2>{this.state.lastName}</h2>
                  <h2>{this.state.middleName}</h2>
                </div>
                <Button type="button" className="align-self-start btn btn-danger" onClick={(e) => this.logout(e)}>Выход</Button>
              </div>
          </div>
      );
  }
}

