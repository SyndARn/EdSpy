var WebAngular = angular.module('WebAngular', ['ngRoute']);

WebAngular.controller('LandingPageController', LandingPageController);

var configFunction = function ($routeProvider) {
    $routeProvider.
        when('/WebAngularOne', {
            templateUrl: 'WebAngular/one'
        });
}
configFunction.$inject = ['$routeProvider'];

WebAngular.config(configFunction);