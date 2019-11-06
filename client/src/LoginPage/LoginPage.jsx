import React from 'react';
import '../index.css';

import { authenticationService } from '@/_services';

class LoginPage extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            username: '',
            password: ''
        };

        this.handleUsernameChange = this.handleUsernameChange.bind(this);
        this.handlePasswordChange = this.handlePasswordChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleUsernameChange(event) {
        // split up the @...
        this.setState({ username: event.target.value });
    }

    handlePasswordChange(event) {
        this.setState({ password: event.target.value });
    }

    handleSubmit(event) {
        event.preventDefault();

        authenticationService.login(this.state.username, this.state.password)
            .then(isAuthenticated => {
                console.log(isAuthenticated);
            })
      }

    render() {
        return (
            <div>
                <form className="login-form" onSubmit={this.handleSubmit}>
                    <h1>Sign Into Your Account</h1>

                    <div>
                        <label htmlFor="email">Email Address</label>
                        <input type="text" id="email" value={this.state.username} onChange={this.handleUsernameChange} className="field" />
                    </div>

                    <div>
                        <label htmlFor="password">Password</label>
                        <input type="password" id="password" value={this.state.password} onChange={this.handlePasswordChange} className="field" />
                    </div>

                    <input type="submit" value="Login to my Dashboard" className="button block" />
                </form>
            </div>
        )
    }
}

export { LoginPage }; 