import React from 'react';
import '../index.css';
import { authenticationRepository, accountRepository } from '@/_services';

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
        this.setState({ username: event.target.value });
    }

    handlePasswordChange(event) {
        this.setState({ password: event.target.value });
    }

    handleSubmit(event) {
        event.preventDefault();

        authenticationRepository.login(this.state.username.split('@')[0], this.state.password)
            .then(() => {
                accountRepository.create()
                .then(id => {
                    this.props.history.push({
                        pathname: `/Dashboard`,
                        state: { id: id }
                    })
                })
            });
      }

    render() {
        const { username, password } = this.state;

        return (
            <div>
                <form className="login-form" onSubmit={this.handleSubmit}>
                    <h1>Sign Into Your Account</h1>

                    <div>
                        <label htmlFor="email">Email Address</label>
                        <input type="text" id="email" value={username} onChange={this.handleUsernameChange} className="field" />
                    </div>

                    <div>
                        <label htmlFor="password">Password</label>
                        <input type="password" id="password" value={password} onChange={this.handlePasswordChange} className="field" />
                    </div>

                    <input type="submit" value="Login to my Dashboard" className="button block" />
                </form>
            </div>
        )
    }
}

export { LoginPage }; 