import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import SessionManager from './components/auth/SessionManager';
import Login from './components/auth/Login';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      SessionManager.getToken() ? (
        <Layout>
          <Routes>
            {AppRoutes.map((route, index) => {
              const { element, ...rest } = route;
              return <Route key={index} {...rest} element={element} />;
            })}
          </Routes>
        </Layout>
       ) : (
        <Layout>
          <Routes>
            <Route path='*' element={<Login />} />
          </Routes>
        </Layout>
       )
    );
  }
}
