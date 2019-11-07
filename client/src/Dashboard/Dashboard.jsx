import React from 'react';
import '../index.css';
import { accountRepository } from '@/_services';
import { authenticationService } from '@/_services';
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

        this.createAndGetAccount();
    }

    createAndGetAccount() {
        accountRepository.create()
            .then(id => accountRepository.get(id))
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
            .withUrl("https://localhost:44333/userHub")
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
        if (this.state.currentPlan === 1) {
            return (
                <button onClick={this.upgradePlan} className="button is-success">Upgrade to Enterprise Plan</button>
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

    upgradePlan() {
        accountRepository.upgradePlan(this.state.accountId);
        this.setState({ currentPlan: 2, maximumNumberOfUsersAllowed: 1000, pricePerMonth: 1000});
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