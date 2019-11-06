import React from 'react';
import '../index.css';
import { accountRepository } from '@/_services';

class Dashboard extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            currentPlan: null,
            numberOfUsers: null,
            maximumNumberOfUsersAllowed: null,
            pricePerMonth: null
        };
    }

    componentDidMount() {
        accountRepository.create()
        .then(res => accountRepository.get(res))
        .then(res => {
            this.setState({ currentPlan: res.currentPlan, numberOfUsers: res.users.length, maximumNumberOfUsersAllowed: res.maximumNumberOfUsersAllowed,
            pricePerMonth: res.pricePerMonth })
        });
    }

    // So, every now and then, 

    // May want API to return isFull...and max number of users allowed, and cost
    // This is a bandaid for now
    hasReachedUserLimit() {
        return this.state.numberOfUsers == 100;
    }

    // Reached maximum...That will be a component lifecycle check to constantly compare isFull = true?
    // Once upgraded calling API upgrade, we turn currentPlan to two, in which case upgrade text is invoked
    // Show upgrade to enterprise if startupplan = 1

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

    //<div className="alert is-error" >You have exceeded the maximum number of users for your account, please upgrade your plan to increaese the limit.</div>
    //<div className="alert is-success">Your account has been upgraded successfully!</div>
    
    render() {
        return (
            <div>
                <header className="top-nav">
                <h1>
                    <i className="material-icons">supervised_user_circle</i>
                    User Management Dashboard
                </h1>
                <button className="button is-border">Logout</button>
                </header>
                
                

                <div className="plan">
                    {this.renderHeader()}

                    <div className="plan-content">
                        <div className="progress-bar">
                        <div style={{width: 35 + '%'}} className="progress-bar-usage"></div>
                        </div>

                        <h3>Users: {this.state.numberOfUsers}/100</h3>
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