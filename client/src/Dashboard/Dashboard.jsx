import React from 'react';
import '../index.css';
import { authenticationRepository, accountRepository } from '@/_services';
import config from 'config';
import { PlanTypes } from './plantypes';

const signalR = require('@aspnet/signalr')

class Dashboard extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            accountId: null,
            currentPlan: null,
            numberOfUsers: null,
            maximumNumberOfUsersAllowed: null,
            pricePerMonth: null,
            hub: null
        };

        this.logout = this.logout.bind(this);
        this.upgradePlan = this.upgradePlan.bind(this);
    }

    componentDidMount() {
        const hub = this.configureUserHub();
        this.start(hub);

        this.getAccount();
    }

    getAccount() {
        accountRepository.get(this.props.location.state.id)
            .then(createdAccount => {
                this.setState({ accountId: createdAccount.id,
                    currentPlan: createdAccount.currentPlan, 
                    numberOfUsers: createdAccount.users.length, 
                    maximumNumberOfUsersAllowed: createdAccount.maximumNumberOfUsersAllowed,
                    pricePerMonth: createdAccount.pricePerMonth })
        });
    }

    start(hub) {
        this.setState({ hub: hub }, () => {
            this.state.hub.start()

            this.state.hub.on("AddedUser", () => {
                this.setState({ numberOfUsers: this.state.numberOfUsers + 1 })
            });
        });
    }

    configureUserHub() {
        return new signalR.HubConnectionBuilder()
            .withUrl(`${config.apiUrl}/userHub`)
            .build();
    }

    renderUserLimitExceededMessage() {
        if (this.state.numberOfUsers == this.state.maximumNumberOfUsersAllowed) {
            return (
                <div className="alert is-error">You have exceeded the maximum number of users for your account, please upgrade your plan to increase the limit.</div>
            );
        }
    }

    renderUpgradeButton() {
        if (this.state.currentPlan == PlanTypes.STARTUP_PLAN) {
            return (
                <button onClick={this.upgradePlan} className="button is-success">Upgrade to Enterprise Plan</button>
            );
        }
    }

    upgradePlan() {
        accountRepository.upgradePlan(this.state.accountId);
        this.setState({ currentPlan: PlanTypes.ENTERPRISE_PLAN, maximumNumberOfUsersAllowed: 1000, pricePerMonth: 1000});
    }

    renderHeader() {
        return (
            <header>${this.state.currentPlan} Plan - ${this.state.pricePerMonth}/Month</header>
        );
    }

    renderUpgradeSuccessMessage() {
        if (this.state.currentPlan == PlanTypes.ENTERPRISE_PLAN) {
            return (
                <div className="alert is-success">Your account has been upgraded successfully!</div>
            );
        }
    }

    logout() {
        authenticationRepository.logout()
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
                        <div style={{width: (this.state.numberOfUsers/this.state.maximumNumberOfUsersAllowed) * 100 + '%'}} className="progress-bar-usage"></div>
                        </div>

                        <h3>Users: {this.state.numberOfUsers}/{this.state.maximumNumberOfUsersAllowed}</h3>
                        <h3>Account Id: {this.state.accountId}</h3>
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