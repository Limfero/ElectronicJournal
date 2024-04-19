import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import SessionManager from './auth/SessionManager';

export class Layout extends Component {
  static displayName = Layout.name;

  render() {
    return (
      SessionManager.getToken() ? (
      <div>
        <NavMenu />
        <Container tag="main">
          {this.props.children}
        </Container>
      </div>
      ) : (
        <div>
          <Container tag="main">
            {this.props.children}
          </Container>
        </div>
      )
    );
  }
}
