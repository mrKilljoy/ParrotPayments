app.controller('AuthController', function ($scope, AuthService) {

    // fields
    $scope.responseData = "";

    $scope.userEmail = sessionStorage.getItem('userEmail') || "";

    $scope.userRegistrationEmail = "";
    $scope.userRegistrationPassword = "";
    $scope.userRegistrationConfirmPassword = "";
    $scope.userRegistrationName = "";

    $scope.userLoginEmail = "";
    $scope.userLoginPassword = "";

    $scope.accessToken = sessionStorage.getItem('accessToken') || "";
    $scope.refreshToken = "";

    $scope.userPublicName = "";
    $scope.userBalance = "";


    // functions
    $scope.registerUser = function () {

        switchSpinner();

        $scope.responseData = "";

        var userRegistrationInfo = {

            Email: $scope.userRegistrationEmail,
            Username: $scope.userRegistrationName,
            Password: $scope.userRegistrationPassword,
            RepeatedPassword: $scope.userRegistrationConfirmPassword
        };

        var promiseregister = AuthService.register(userRegistrationInfo);

        promiseregister.then(function (resp) {

            // prepare for login procedure
            $scope.userLoginEmail = $scope.userRegistrationEmail;
            $scope.userLoginPassword = $scope.userRegistrationPassword;

            $scope.responseData = "Success!";
            $scope.userRegistrationEmail = "";
            $scope.userRegistrationPassword = "";
            $scope.userRegistrationConfirmPassword = "";
            $scope.userRegistrationName = "";

            switchSpinner();

            $scope.login();

        }, function (err) {
            $scope.responseData = "Error: " + err.status;
            var msg = err.data.error_description || err.data.Message || err.data.ModelState[''];

            alertify.error('Ошибка: ' + msg || $scope.responseData);
            switchSpinner();
        });
    };

    $scope.redirect = function () {
        window.location.href = '/';
    };

    $scope.login = function () {

        // spinner on
        switchSpinner();

        var userLogin = {
            grant_type: 'password',
            username: $scope.userLoginEmail,
            password: $scope.userLoginPassword
        };

        var promiselogin = AuthService.login(userLogin);

        promiselogin.then(function (resp) {

            $scope.userEmail = userLogin.username;
            $scope.accessToken = resp.data.access_token;

            sessionStorage.setItem('userEmail', userLogin.username);
            sessionStorage.setItem('accessToken', resp.data.access_token);

            // get public name
            $scope.publicName();

            // get current balance
            $scope.showBalance();

            //spinner off
            switchSpinner();

            $scope.redirect();

        }, function (err) {

            $scope.responseData = "Error " + err.status;
            var msg = err.data.error_description || err.data.Message;

            alertify.error('Ошибка: ' + msg || $scope.responseData);

            switchSpinner();
        });
    };

    $scope.logout = function () {
        sessionStorage.clear();
        window.location.href = '/login';
    };

    $scope.isAuthenticated = function () {
        var value = sessionStorage.getItem('accessToken');
        if (value)
            return true;
        else
            return false;
    };

    $scope.publicName = function(){

        var login = sessionStorage.getItem('userEmail');
        var token = sessionStorage.getItem('accessToken');

        var promiseName = AuthService.getPublicName(token, login);

        promiseName.then(function (resp) {

            var name = resp.data;
            sessionStorage.setItem('publicName', name);

        }, function (err) {

            $scope.responseData = "Error " + err.status;
            var msg = err.data.error_description || err.data.Message;

            alertify.error('Ошибка: ' + msg || $scope.responseData);

        });
    }

    $scope.gotPublicName = function () {
        var name = sessionStorage.getItem('publicName');
        $scope.userPublicName = name;

        if (name)
            return true;
        else
            return false;
    }

    $scope.showBalance = function () {

        var token = sessionStorage.getItem('accessToken');
        var login = sessionStorage.getItem('userEmail');

        var promiseBalance = AuthService.getUserBalance(token, login);

        promiseBalance.then(function (resp) {
            $scope.userBalance = resp.data;
            sessionStorage.setItem('userBalance', resp.data);

            var x = 1;
        }, function (err) {

            $scope.responseData = "Error " + err.status;
            var msg = err.data.error_description || err.data.Message;

            alertify.error('Ошибка: ' + msg || $scope.responseData);
        });
    }

});


app.controller('indexController', function ($scope, $http) {

    $scope.isAuthenticated = function () {
        var value = sessionStorage.getItem('accessToken');
        if (value)
            return true;
        else
            return false;
    };

});


app.controller('HistoryController', function ($scope, PayService) {

    // fields
    $scope.paymentsHistoryResponse = "";
    $scope.paymentRelation = 2;
    $scope.paymentsCount = 20;

    $scope.paymentsList = [];

    // functions
    $scope.isAuthenticated = function () {
        var value = sessionStorage.getItem('accessToken');
        if (value)
            return true;
        else
            return false;
    };

    $scope.getHistoryType = function(){
        var element = $('#payment-type-variant')[0];
        if (!element)
            return;

        return (element.checked === true ? 1 : 2);
    }

    $scope.history = function () {

        $scope.paymentRelation = $scope.getHistoryType();

        var valid_token = sessionStorage.getItem('accessToken');
        var user_email = sessionStorage.getItem('userEmail');
        var payment_type = $scope.paymentRelation;
        var payments_count = $scope.paymentsCount;

        switchNavbarSpinner();

        var response = PayService.getHistory(valid_token, user_email, payment_type, payments_count);

        response.then(function (msg) {

            // change date format
            if (msg.data.length > 0) {
                for (var i = 0; i < msg.data.length; i++) {
                    var date_str = msg.data[i].TransactionDate;
                    msg.data[i].TransactionDate = new Date(date_str).toLocaleDateString();
                }
            }

            $scope.paymentsList = msg.data || [];

            switchNavbarSpinner();
            alertify.success('Операция выполнена успешно!');
        }, function (err) {
            $scope.paymentsHistoryResponse = "Error " + err.Status;
            var msg = $scope.paymentOperationResponse || err.data.ModelState[''] || err.data.Message;

            switchNavbarSpinner();
            alertify.error('Ошибка: ' + msg);

        });
    };
});


app.controller('PaymentsController', function ($scope, PayService) {

    // fields
    $scope.paymentSenderLogin = sessionStorage.getItem('userEmail') || "";
    $scope.paymentReceiverLogin = "";
    $scope.paymentReceiverName = "";
    $scope.paymentAmount = "";

    $scope.paymentOperationResponse = "";

    // functions
    $scope.isAuthenticated = function () {
        var value = sessionStorage.getItem('accessToken');
        if (value)
            return true;
        else
            return false;
    };

    $scope.send = function () {

        var valid_token = sessionStorage.getItem('accessToken');

        var payInfo = {
            Id: 0,
            SenderId: $scope.paymentSenderLogin,
            CorrespondentId: $scope.paymentReceiverLogin,
            Date: new Date(),
            Amount: $scope.paymentAmount,
            PostBalance: 0
        };

        switchNavbarSpinner();

        var response = PayService.sendPayment(valid_token, payInfo);

        response.then(function (msg) {

            $scope.paymentReceiverLogin = "";
            $scope.paymentAmount = "";
            
            $scope.updateBalance();
            switchNavbarSpinner();

            alertify.success('Операция выполнена успешно!');

        }, function (err) {

            $scope.paymentOperationResponse = "Error " + err.Status;
            var msg = $scope.paymentOperationResponse || err.data.ModelState[''][0] || err.data.Message;

            switchNavbarSpinner();
            alertify.error('Ошибка: ' + msg);
        });
    };

    $scope.updateBalance = function () {

        var token = sessionStorage.getItem('accessToken');
        var login = sessionStorage.getItem('userEmail');

        var promiseBalance = PayService.getUserBalance(token, login);

        promiseBalance.then(function (resp) {

            sessionStorage.setItem('userBalance', resp.data);

            var x = 1;
        }, function (err) {

            $scope.responseData = "Error " + err.status;
            var msg = err.data.error_description || err.data.Message;

            alertify.error('Ошибка: ' + msg || $scope.responseData);
        });
    }

});