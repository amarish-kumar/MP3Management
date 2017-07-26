angular.module("MP3DetailsController", [])
    .controller("MP3DetailsCtrl", ["$scope", "$http", "$routeParams", function ($scope, $http, $routeParams) {
        $scope.mp3details = {};
        $http({
            url: "/MP3File/MP3Details",
            params: { id: $routeParams.id },
            method: "get"
        })
            .then(function (response) {
                $scope.mp3details = response.data;
            })
    }])
