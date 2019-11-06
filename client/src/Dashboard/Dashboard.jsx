import React from 'react';
import '../index.css';

class Dashboard extends React.Component {
    constructor(props) {
        super(props);

    }

    render() {
        return (
            <div>
                <header class="top-nav">
                <h1>
                    <i class="material-icons">supervised_user_circle</i>
                    User Management Dashboard
                </h1>
                <button class="button is-border">Logout</button>
                </header>

                <div class="alert is-error">You have exceeded the maximum number of users for your account, please upgrade your plan to increaese the limit.</div>
                <div class="alert is-success">Your account has been upgraded successfully!</div>

                <div class="plan">
                <header>Startup Plan - $100/Month</header>

                <div class="plan-content">
                    <div class="progress-bar">
                    <div style={{width: 35 + '%'}} class="progress-bar-usage"></div>
                    </div>

                    <h3>Users: 35/100</h3>
                </div>

                <footer>
                    <button class="button is-success">Upgrade to Enterprise Plan</button>
                </footer>
                </div>
            </div>
        )
    }
}

export { Dashboard }; 