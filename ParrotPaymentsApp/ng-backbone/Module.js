var app;
(function () {
    app = angular.module('AuthModule', ['ngRoute'])
        .config(function ($routeProvider, $locationProvider) {

        $routeProvider.when('/',
        {
            redirectTo: '/main'
        });
        $routeProvider.when('/history',
        {
            templateUrl: 'ng-backbone/views/history_page.html',
        });
        $routeProvider.when('/payment',
        {
            templateUrl: 'ng-backbone/views/payment_page.html',
        });
        $routeProvider.when('/operations',
        {
            templateUrl: 'ng-backbone/views/operations_page.html',
        });
        $routeProvider.otherwise({ redirectTo: '/operations' });

    });
})();