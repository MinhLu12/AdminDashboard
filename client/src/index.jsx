import React from 'react';
import { render } from 'react-dom';

import { LoginPage } from './LoginPage';

// setup fake backend
// import { configureFakeBackend } from './_helpers';
// configureFakeBackend();

render(
    <LoginPage />,
    document.getElementById('app')
);