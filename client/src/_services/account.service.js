import config from 'config';

export const accountRepository = {
    create,
    get
};

function create() {
    const requestOptions = {
        method: 'POST',
        withCredentials: true,
        headers: { 'Content-Type': 'application/json',
                   'Accept': 'application/json',
                   'Authorization': `Bearer ${sessionStorage.getItem("Token")}` },
        body: JSON.stringify({ "plan": 1 })
    };
    

    return fetch(`${config.apiUrl}/api/account`, requestOptions)
        .then(response => response.json())
}

function get(id) {
    const requestOptions = {
        method: 'GET',
        withCredentials: true,
        headers: { 'Content-Type': 'application/json',
                   'Accept': 'application/json',
                   'Authorization': `Bearer ${sessionStorage.getItem("Token")}` }
    };

    return fetch(`${config.apiUrl}/api/account/${id}`, requestOptions)
        .then(response => response.json());
}