angular.module("PlaylistController", ['ngMaterial'])
    .controller("PlaylistCtrl", ["$scope", "$http", "$mdDialog", function ($scope, $http, $mdDialog) {
        $scope.model = {};
        $scope.new = {
            Playlist: {}
        }
        $scope.states = {
            showPlaylistForm: false,
            allowEditPlaylist: false
        };
        $scope.status = '  ';
        $scope.customFullscreen = false;

        $http.get("/Playlists/Index").then(function (data) {
            $scope.model = data;
        });
        $scope.createPlaylist = function () {
            if (Object.keys($scope.new.Playlist).length > 0 ) {
                $http.post('/Playlists/Create', $scope.new.Playlist).then(function (response) {
                    //to do
                    $scope.model.data.push(response.data);
                    $scope.cancelEdit();
                });
            }
        };
        $scope.updatePlaylist = function () {
            $http.post('/Playlists/Edit', $scope.new.Playlist).then(function (response) {
                var index = $scope.model.data.map(function (element) {
                    return element.PlaylistID;
                }).indexOf($scope.new.Playlist.PlaylistID);
                $scope.model.data.splice(index, 1);

                $scope.model.data.push(response.data);
                $scope.cancelEdit();
            });
        };
        $scope.editPlaylist = function (id, name, description) {
            $scope.new.Playlist = { PlaylistID: id, Name: name, Description: description };
            $scope.states.allowEditPlaylist = true;
        };
        $scope.cancelEdit = function () {
            $scope.new.Playlist = {};
            $scope.states.allowEditPlaylist = false;
        };
        $scope.deletePlaylist = function (id) {
            $http.post("/Playlists/Delete", { id: id }).then(function (response) {
                $http.get("/Playlists/Index").then(function (data) {
                    $scope.model = data;
                });
            });
        };

    }])
