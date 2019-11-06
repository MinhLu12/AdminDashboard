import React from 'react';
import '../index.css';
import { accountRepository } from '@/_services';

class Dashboard extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            currentPlan: 1,
            numberOfUsers: 0
        };

    }

    componentDidMount() {
        accountRepository.create()
        .then(res => accountRepository.get(res))
        .then(res => {
            this.setState({ currentPlan: res.currentPlan, numberOfUsers: res.users})
        })
        .then(console.log(this.state));

    }
    // Make account, get it, display start up plan, number of users, etc.
    // Then we add users. Refreshs to see them.
    // Upgrade
    // Logout


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

                <div className="alert is-error">You have exceeded the maximum number of users for your account, please upgrade your plan to increaese the limit.</div>
                <div className="alert is-success">Your account has been upgraded successfully!</div>

                <div className="plan">
                <header>Startup Plan - $100/Month</header>

                <div className="plan-content">
                    <div className="progress-bar">
                    <div style={{width: 35 + '%'}} className="progress-bar-usage"></div>
                    </div>

                    <h3>Users: 35/100</h3>
                </div>

                <footer>
                    <button className="button is-success">Upgrade to Enterprise Plan</button>
                </footer>
                </div>
            </div>
        )
    }
}

export { Dashboard }; 