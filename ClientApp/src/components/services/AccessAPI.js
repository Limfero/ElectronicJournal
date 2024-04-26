import SessionManager from "../auth/SessionManager";

export function getData(endPoint) {

    let token=SessionManager.getToken();

    let payload = {
        method: 'GET',
        headers: {   
            "access-control-allow-origin" : "*", 
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
         },
    }
    return fetch(endPoint, payload)
    .then(function(response) {
        if (!response.ok) {
            if(response.statusText === "Unauthorized")
                window.location.assign("/logout");

            throw Error(response.statusText);
        }
        return response.json();
    }).then(function(result) {
        return result;
    }).catch(function(error) {
        console.log(error);
    });
}

export function postDataForLogin(type, userData) {
    let payload = {
        method: 'POST',
        headers: {   
            "access-control-allow-origin" : "*",
            'Content-Type': 'application/json' 
        },
        body: JSON.stringify(userData)

    }
    return fetch(type, payload)
    .then(function(response) {
        return response.json();
    }).then(function(result) {
        return result;
    }).catch(function(error) {
        console.log(error);
    });
}

export function postData(endPoint, inputObj) {
    let token=SessionManager.getToken();
    let payload = {
        method: 'POST',
        headers: {   
            "access-control-allow-origin" : "*", 
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
        body: JSON.stringify(inputObj)

    }
    return fetch(endPoint, payload)
    .then(function(response) {
        return response.json();
    }).then(function(result) {
        return result;
    }).catch(function(error) {
        console.log(error);
    });
}

export function postDataWithImage(endPoint, inputObj) {
    let token=SessionManager.getToken();
    let payload = {
        method: 'POST',
        headers: {   
            'Accept': 'application/json',
            'Authorization': 'Bearer ' + token,
        },
        body: inputObj

    }
    return fetch(endPoint, payload)
    .then(function(response) {
        return response.json();
    }).then(function(result) {
        return result;
    }).catch(function(error) {
        console.log(error);
    });
}

export function deleteData(endPoint) {
    let token=SessionManager.getToken();
    let payload = {
        method: 'DELETE',
        headers: {   
            "access-control-allow-origin" : "*", 
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
         },
    }
    return fetch(endPoint, payload)
    .then(function(response) {
        if (!response.ok) {
            throw Error(response.statusText);
        }
        return response.json();
    }).then(function(result) {
        return result;
    }).catch(function(error) {
        console.log(error);
    });
}

export function putData(endPoint, obj) {
    let token=SessionManager.getToken();
    let payload = {
        method: 'PUT',
        headers: {   
            "access-control-allow-origin" : "*", 
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
        },
        body: JSON.stringify(obj)

    }
    return fetch(endPoint, payload)
    .then(function(response) {
        return response.json();
    }).then(function(result) {
        return result;
    }).catch(function(error) {
        console.log(error);
    });
}