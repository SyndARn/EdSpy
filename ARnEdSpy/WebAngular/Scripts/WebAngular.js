var WebAngular = angular.module('WebAngular', []);
webAngular.Controller('GetQuoteAngularCtrl', GetQuoteAngularCtrl);
webAngular.Controller('RefreshQuoteCtrl', RefreshQuoteCtrl);

var GetQuoteAngularCtrl = 
   function ($scope, $http) {
       $http.get('~/WebAngular/GetQuote?quote=USD')
       .then(function (response) {
           $scope.getquoteUSD = response.data;
       }
       );
   };
GetQuoteAngularCtrl.$inject = ['$scope,$http'];

var RefreshQuoteCtrl = 
  function loadData($scope, $http) {
      $http.get('~/WebAngular/GetQuote?quote=USD')
      .then(function mySucces(response) {
          $scope.getquoteUSD = response.data;
      }
      );
  } ;

RefreshQuoteCtrl.$inject = ['$scope,$http'];


