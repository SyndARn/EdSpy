var WebAngular = angular.module('WebAngular', ['ngRoute', 'ui.bootstrap']);

WebAngular.controller('LandingPageController', LandingPageController);

var configFunction = function ($routeProvider) {
    $routeProvider.
        when('/WebAngularUsd/:quote', {
            templateUrl: function (params)
            {
                return '/WebAngular/Usd?quote=' + params.quote;;
            }
            , resolve: {}
        }
            );
}
configFunction.$inject = ['$routeProvider'];

WebAngular.config(configFunction);