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
            userCount: null,
            userLimit: null,
            pricePerMonth: null,
            showUpgradedSuccessMessage: false
        };

        this.logout = this.logout.bind(this);
        this.upgradePlan = this.upgradePlan.bind(this);
        this.hideUpgradedSuccessMessage = this.hideUpgradedSuccessMessage.bind(this);
    }

    componentDidMount() {
        const hub = this.configureUserHub();
        this.start(hub);

        this.getAccount();
    }

    getAccount() {
        accountRepository.get(this.props.location.state.id)
            .then(account => {
                this.setState({ accountId: account.id,
                    currentPlan: account.currentPlan, 
                    userCount: account.users.length, 
                    userLimit: account.maximumNumberOfUsersAllowed,
                    pricePerMonth: account.pricePerMonth })
        });
    }

    start(hub) {
        hub.start().catch(() => alert("Error establishing handshake with server"));

        hub.on("AddedUser", () => {
            this.setState({ userCount: this.state.userCount + 1 })
        });
    }

    configureUserHub() {
        return new signalR.HubConnectionBuilder()
            .withUrl(`${config.apiUrl}/userHub`)
            .build();
    }

    renderUserLimitExceededMessage() {
        if (this.state.userCount == this.state.userLimit) {
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
        this.setState({ currentPlan: PlanTypes.ENTERPRISE_PLAN, userLimit: 1000, pricePerMonth: 1000});
        this.setState({ showUpgradedSuccessMessage: true });
    }

    renderHeader() {
        return (
            <header>{this.getTextFrom(this.state.currentPlan)} Plan - ${this.state.pricePerMonth}/Month</header>
        );
    }

    getTextFrom(currentPlan) {
        if (currentPlan == PlanTypes.STARTUP_PLAN)
            return "Startup";
        else
            return "Enterprise";
    }

    renderUpgradeSuccessMessage() {
        if (this.state.currentPlan == PlanTypes.ENTERPRISE_PLAN && this.state.showUpgradedSuccessMessage) {
            return (
                <div className="alert is-success" onClick={this.hideUpgradedSuccessMessage}>Your account has been upgraded successfully!</div>
            );
        }
    }

    hideUpgradedSuccessMessage() {
        this.setState({ showUpgradedSuccessMessage: false});
    }

    logout() {
        authenticationRepository.logout()
        this.props.history.push('/');
    }
    
    render() {
        const { accountId, userCount, userLimit } = this.state;
        const progress = `${userCount / userLimit * 100}%`;

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
                        <div style={{width: progress}} className="progress-bar-usage"></div>
                        </div>

                        <h3>Users: {userCount}/{userLimit}</h3>
                        <h3>Account Id: {accountId}</h3>
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