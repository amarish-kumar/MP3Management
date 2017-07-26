angular.module("PlaylistController", [])
    .controller("PlaylistCtrl", ["$scope", "$http", function ($scope, $http) {
        $scope.message = "Hello Playlist Controller!";
    }])
