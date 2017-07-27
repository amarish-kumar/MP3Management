angular.module("PlaylistDetailsController", [])
    .controller("PlaylistDetailsCtrl", ["$scope", "$http", "$routeParams", function ($scope, $http, $routeParams) {
        $scope.playlistDetails = {};
        $scope.allmp3files = {};
        $http({
            url: "/Playlists/PlaylistDetails",
            params: { id: $routeParams.id },
            method: "get"
        })
            .then(function (response) {
                $scope.playlistDetails = response.data;
            });

        $http.get("/MP3File/Index").then(function (data) {
            $scope.allmp3files = data;
        });
        $scope.announceClick = function (mp3Id, playlistId) {
            $http.post('/MP3File/AddToPlaylist', { mp3Id: mp3Id, playlistId: playlistId }).then(function (response) {
                //todo error catch
                $scope.playlistDetails.MP3Files.push(response.data);
            });
        };
        $scope.removeMp3FromPlaylist = function (mp3Id, playlistId) {
            $http.post('/Playlists/RemoveMp3FromPlaylist', { mp3Id: mp3Id, playlistId: playlistId }).then(function (response) {
                //get index of deleted item in $scope
                var index = $scope.playlistDetails.MP3Files.map(function (element) {
                    return element.MP3FileID;
                }).indexOf(mp3Id);
                $scope.playlistDetails.MP3Files.splice(index, 1);
            });
        };
    }])
