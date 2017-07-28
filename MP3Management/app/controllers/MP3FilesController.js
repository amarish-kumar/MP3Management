angular.module('MP3FilesController', ['ngMaterial'])
    .controller('MP3FilesCtrl', ['$scope', '$http', '$mdDialog', function ($scope, $http, $mdDialog) {
        $scope.model = {};
        $scope.playlists = {};
        $scope.states = {          
            allowEditMP3File: false       
        };
        $scope.new = {
            MP3File: {}
        }
        $http.get("/MP3File/Index").then(function (data) { 
            $scope.model = data;
        });
        $http.get("/MP3File/Playlists").then(function (data) {
            $scope.playlists = data;
        });
        $scope.addMP3File = function () {
            $http.post('/MP3File/Create', $scope.new.MP3File).then(function (data) {
                $http.get("/MP3File/Index").then(function (data) {
                    $scope.model = data;
                });
                $scope.cancelEdit();
            });
            
            $scope.showMP3Form(false);
            $scope.new.MP3File = {};
        };
        $scope.updatePlaylist = function () {
            $http.post('/MP3File/Edit', $scope.new.MP3File).then(function (response) {
                $http.get("/MP3File/Index").then(function (data) {
                    $scope.model = data;
                });
                $scope.cancelEdit();
            });
        };
        $scope.deleteMP3File = function (id) {
            $http.post("/MP3File/Delete", { id: id }).then(function (response) {
                $http.get("/MP3File/Index").then(function (data) {
                    $scope.model = data;
                });
            });
        };
        $scope.cancelEdit = function () {
            $scope.new.MP3File = {};
            $scope.states.allowEditMP3File = false;
        };
        $scope.editMP3File = function (id, name, author, albumName) {
            $scope.new.MP3File = { MP3FileID: id, Name: name, Author: author, AlbumName: albumName };
            $scope.states.allowEditMP3File = true;
        };
        $scope.announceClick = function (mp3Id, playlistId) {
            $http.post('/MP3File/AddToPlaylist', { mp3Id: mp3Id, playlistId : playlistId}).then(function (response) {
                //todo 
            });
        };
        $scope.searchMP3files = function (searchString, searchBy) {
            $http({
                url: "/MP3File/Search",
                params: { SearchString: searchString, SearchBy: searchBy },
                method: "get"
            })
                .then(function (response) {
                $scope.model = response;
                });
        };
        $scope.showConfirm = function (ev, MP3FileID) {
            var confirm = $mdDialog.confirm()
                .title('Are you sure you want to delete this mp3 record?')
                .textContent('The action is irreversible!')
                .ariaLabel('Delete record')
                .targetEvent(ev)
                .ok('Delete')
                .cancel('Cancel');

            $mdDialog.show(confirm).then(function () {
                $scope.deleteMP3File(MP3FileID);
            });
        };
    }])

