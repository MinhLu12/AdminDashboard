import React from 'react';
import '../index.css';
import { accountRepository } from '@/_services';
import { authenticationService } from '@/_services';
const signalR = require('@aspnet/signalr')

class Dashboard extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            currentPlan: null,
            numberOfUsers: null,
            maximumNumberOfUsersAllowed: null,
            pricePerMonth: null,
            hubConnection: null
        };

        this.logout = this.logout.bind(this);
    }

    componentDidMount() {
        const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:44333/userHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();  // You have a // in your server
        this.setState({ hubConnection });

        hubConnection.on("AddedUser", data => {
            console.log(data);
        });
        // hubConnection.start().then(function () {
        //     console.log("connected");
        // });

        console.log(hubConnection);

        accountRepository.create()
        .then(res => accountRepository.get(res))
        .then(res => {
            this.setState({ currentPlan: res.currentPlan, 
                numberOfUsers: res.users.length, 
                maximumNumberOfUsersAllowed: res.maximumNumberOfUsersAllowed,
            pricePerMonth: res.pricePerMonth })
        });
    }

    renderUserLimitExceededMessage() {
        return this.state.numberOfUsers == this.state.maximumNumberOfUsersAllowed;
    }

    renderUpgradeButton() {
        if (this.state.currentPlan === 1) {
            return (
                <button className="button is-success">Upgrade to Enterprise Plan</button>
            );
        }
    }

    renderHeader() {
        if (this.state.currentPlan === 1) {
            return (
                <header>Startup Plan - ${this.state.pricePerMonth}/Month</header>
            );
        }
        else if (this.state.currentPlan === 2) {
            return (
                <header>Enterprise Plan - ${this.state.pricePerMonth}/Month</header>
            );
        }
    }

    renderUpgradeSuccessMessage() {
        if (this.state.currentPlan === 2) {
            return (
                <div className="alert is-success">Your account has been upgraded successfully!</div>
            );
        }
    }

    logout() {
        authenticationService.logout()
        this.props.history.push('/');
    }
    
    render() {
        return (
            <div>
                <header className="top-nav">
                <h1>
                    <i className="material-icons">supervised_user_circle</i>
                    User Management Dashboard
                </h1>
                <button onClick={this.logout} className="button is-border">Logout</button>
                </header>
                
                {this.renderUserLimitExceededMessage()}
                {this.renderUpgradeSuccessMessage()}

                <div className="plan">
                    {this.renderHeader()}

                    <div className="plan-content">
                        <div className="progress-bar">
                        <div style={{width: (this.state.numberOfUsers/this.state.maximumNumberOfUsersAllowed) + '%'}} className="progress-bar-usage"></div>
                        </div>

                        <h3>Users: {this.state.numberOfUsers}/{this.state.maximumNumberOfUsersAllowed}</h3>
                    </div>

                    <footer>
                        {this.renderUpgradeButton()}
                    </footer>
                </div>
            </div>
        )
    }
}

export { Dashboard }; 