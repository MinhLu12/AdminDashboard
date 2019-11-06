import config from 'config';

export const authenticationService = {
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
            .then(data => sessionStorage.setItem('Token', data))
        )
}

function logout() {
    sessionStorage.removeItem("Token");
}