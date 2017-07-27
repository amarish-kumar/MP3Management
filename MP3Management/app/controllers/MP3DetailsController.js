angular.module("MP3DetailsController", ['ngMaterial'])
    .controller("MP3DetailsCtrl", ["$scope", "$http", "$routeParams", function ($scope, $http, $routeParams) {
        $scope.mp3details = {};
        $scope.imagePath = "Content/img/mp3.png";
        $http({
            url: "/MP3File/MP3Details",
            params: { id: $routeParams.id },
            method: "get"
        })
            .then(function (response) {
                $scope.mp3details = response.data;
            });
    }])
    .config(function ($mdThemingProvider) {
        $mdThemingProvider.theme('dark-grey').backgroundPalette('grey').dark();
        $mdThemingProvider.theme('dark-orange').backgroundPalette('orange').dark();
        $mdThemingProvider.theme('dark-purple').backgroundPalette('deep-purple').dark();
        $mdThemingProvider.theme('dark-blue').backgroundPalette('blue').dark();
    });
