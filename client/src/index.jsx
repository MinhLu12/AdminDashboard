import React from 'react';
import { render } from 'react-dom'

import { LoginPage } from './LoginPage';
import { Dashboard } from './Dashboard';
import { Switch, Route, BrowserRouter as Router } from 'react-router-dom'

const routing = (
  <Router>
      <Switch>
        <Route exact path="/" component={LoginPage} />
        <Route path="/Dashboard" component={Dashboard} />
      </Switch>
  </Router>
)

render(
    routing,
    document.getElementById('app')
);