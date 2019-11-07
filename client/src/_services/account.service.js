import config from 'config';

export const accountRepository = {
    create,
    get,
    upgradePlan
};

function create() {
    const requestOptions = getOptions('POST', JSON.stringify({ "plan": 1 }));

    return fetch(`${config.apiUrl}/api/account`, requestOptions)
        .then(response => response.json())
}

function upgradePlan(id) {
    const requestOptions = getOptions('PUT', JSON.stringify({ "plan": 2 }));
    
    return fetch(`${config.apiUrl}/api/account/${id}`, requestOptions);
}

function get(id) {
    return fetch(`${config.apiUrl}/api/account/${id}`, getOptions('GET'))
        .then(response => response.json());
}

function getOptions(method, body) {
    return {
        method: method,
        withCredentials: true,
        headers: { 'Content-Type': 'application/json',
                   'Accept': 'application/json',
                   'Authorization': `Bearer ${sessionStorage.getItem("Token")}` },
        body: body
    };
}