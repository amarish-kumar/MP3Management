angular.module("RouteController", ["ngRoute"])
    .config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "Templates/mp3files.html",
            controller: "MP3FilesCtrl"
        })
        .when("/mp3files", {
            templateUrl: "Templates/mp3files.html",
            controller: "MP3FilesCtrl"
        })
        .when("/playlist", {
            templateUrl: "Templates/playlist.html",
            controller: "PlaylistCtrl"
        })
        .when("/mp3details/:id", {
            templateUrl: "Templates/mp3details.html",
            controller: "MP3DetailsCtrl"
        })
        .otherwise({ redirectTo: '/' });
});