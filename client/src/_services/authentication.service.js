import config from 'config';

export const authenticationRepository = {
    login,
    logout
};

function login(username, password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
        body: JSON.stringify({ username, password })
    };
    
    return fetch(`${config.apiUrl}/api/login/authenticate`, requestOptions)
        .then(response => response.json()
        .then(token => sessionStorage.setItem('Token', token)))
        .catch(err => alert(err));
}

function logout() {
    sessionStorage.removeItem("Token");
}