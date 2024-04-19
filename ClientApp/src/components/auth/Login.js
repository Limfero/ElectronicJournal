import { Component } from "react";
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Toast from 'react-bootstrap/Toast';
import ToastContainer from 'react-bootstrap/ToastContainer';
import { postDataForLogin } from "../services/AccessAPI";
import SessionManager from "./SessionManager";


export default class Login extends Component {
    constructor() {
        super();
        this.state = {
            userName: "",
            password: "",
            loading: false,
            failed: false,
            show: false
        };

        this.login = this.login.bind(this);
        this.onChange = this.onChange.bind(this);
    }


    onChange(e) {
        this.setState({ [e.target.name]: e.target.value });
    }

    onKeyDown = (e) => {
        if (e.key === 'Enter') {
            this.login();
        }
    }

    login() {
        let userInfo = this.state;
        this.setState({
            loading: true
        });

        postDataForLogin('api/auth/login', userInfo).then((result) => {
            if (result?.token) {

                SessionManager.setUserSession(result.userName, result.token, result.userId, result.userRole)

                if (SessionManager.getToken()) {
                    this.setState({
                        loading: false
                    });

                    window.location.href = `/home`;
                }
            }
            else {
                this.setState({
                    loading: false,
                    show: true
                });
            }
        });
    }

    render() {
        let content = "Вход";
        if (this.state.loading) {
            content = <div>Загрузка...</div>;
        }

        return (
            <Form>
                <div className="mb-3">
                    <h3 className="text-center">Войдите в систему</h3>
                </div>
                <Form.Group className="mb-3">
                    <Form.Label>Логин</Form.Label>
                    <Form.Control 
                        type="login"
                        placeholder="Введите логин"
                        name="userName"
                        onChange={this.onChange}
                            onKeyDown={this.onKeyDown}/>
                </Form.Group>
                <Form.Group className="mb-3">
                    <Form.Label>Пароль</Form.Label>
                    <Form.Control 
                        type="password"
                        placeholder="Введите пароль"
                        name="password"
                        onChange={this.onChange}
                        onKeyDown={this.onKeyDown}/>
                </Form.Group>
                <Button variant="primary" onClick={this.login}>
                    {content}
                </Button>

                <ToastContainer position="top-end">
                    <Toast onClose={() => this.setState({show: false})} show={this.state.show} className="d-inline-block m-1" bg="danger" delay={3000} autohide>
                        <Toast.Header>
                            <strong className="me-auto">Ошибка</strong>
                        </Toast.Header>
                        <Toast.Body className='text-white'>Неверный логин или пароль!</Toast.Body>
                    </Toast>
                </ToastContainer>
            </Form>
        );
    }
}