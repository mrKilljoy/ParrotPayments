app.service('AuthService', function ($http) {

    this.register = function (userInfo) {
        var resp = $http({
            url: "/api/Account/Register",
            method: "POST",
            data: userInfo,
        });
        return resp;
    };

    this.login = function (userlogin) {

        var resp = $http({
            url: "/TOKEN",
            method: "POST",
            data: $.param({ grant_type: 'password', username: userlogin.username, password: userlogin.password }),
            headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        });

        return resp;
    };

    this.getPublicName = function (token, login) {
        var rsp = $http({
            url: "/api/Account/GetPublicName/",
            method: "GET",
            params:{user_login: login},
            headers: { 'Content-Type': 'application/x-www-form-urlencoded', 'Authorization': 'Bearer ' + token },
        });

        return rsp;
    };

    this.getUserBalance = function (token, login) {
        var rsp = $http({
            url: "/api/Account/GetCurrentBalance/",
            method: "GET",
            params: { user_login: login },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded', 'Authorization': 'Bearer ' + token },
        });

        return rsp;
    };

});


app.service('PayService', function ($http) {

    this.sendPayment = function (token, paymentInfo) {
        var resp = $http({
            url: "/api/Payments/Send",
            method: "POST",
            params: { sender_login: paymentInfo.SenderId, receiver_login: paymentInfo.CorrespondentId, amount: paymentInfo.Amount },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded', 'Authorization': 'Bearer ' + token },
        });

        return resp;
    };

    this.getHistory = function (token, user_login, type, count) {
        var resp = $http({
            url: "/api/Payments/history/",
            method: "POST",
            params: { login: user_login, type: type, count: count },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded', 'Authorization': 'Bearer ' + token },
        });

        return resp;
    };

    this.getUserBalance = function (token, login) {
        var rsp = $http({
            url: "/api/Account/GetCurrentBalance/",
            method: "GET",
            params: { user_login: login },
            headers: { 'Content-Type': 'application/x-www-form-urlencoded', 'Authorization': 'Bearer ' + token },
        });

        return rsp;
    };
});