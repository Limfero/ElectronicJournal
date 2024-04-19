const SessionManager = {

    getToken() {
        const token = sessionStorage.getItem('token');
        if (token) return token;
        else return null;
    },

    setUserSession(userName, token, userId, userRole) {
        sessionStorage.setItem('userName', userName);
        sessionStorage.setItem('token', token);
        sessionStorage.setItem('userId', userId);
        sessionStorage.setItem('userRole', userRole);
    },

    removeUserSession(){
        sessionStorage.removeItem('userName');
        sessionStorage.removeItem('token');
        sessionStorage.removeItem('userId');
        sessionStorage.removeItem('userRole');
    },

    getUser(){
        const user = {
            userName: sessionStorage.getItem('userName'),
            userId: sessionStorage.getItem('userId'),
            userRole: sessionStorage.getItem('userRole')
        };

        return user;
    }
}

export default SessionManager;