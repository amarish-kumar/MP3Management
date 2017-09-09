    app.controller("PlaylistDetailsCtrl", ["$scope", "$http", "$routeParams", "$mdDialog", function ($scope, $http, $routeParams, $mdDialog) {
        $scope.playlistDetails = {};
        $scope.allmp3files = {};
        $scope.states = {
            isLoading: true
        };
        // routing parametar
        $http({
            url: "/Playlists/PlaylistDetails",
            params: { id: $routeParams.id },
            method: "get"
        })
            .then(function (response) {
                $scope.playlistDetails = response.data;
                $scope.states.isLoading = false;
            });
        //all mp3 files
        $http.get("/MP3File/Index").then(function (data) {
            $scope.allmp3files = data;
        });
        // add mp3 file to playlist
        $scope.announceClick = function (mp3Id, playlistId) {
            $http({
                method: 'POST',
                url: '/Playlists/AddToPlaylist',
                headers: {
                    'Content-Type': undefined
                },
                params: { mp3Id: mp3Id, playlistId: playlistId }
            }).then(function successCallback(response) {
                $scope.playlistDetails.MP3Files.push(response.data);
            }, function errorCallback(response) {
                // only distinct elements allowed in playlist
                if (response.status == 400) {
                    $scope.showAlert("Error adding record", "Record already exists!");
                } else {
                    $scope.showAlert("Error adding record", response.status + " " + response.statusText);
                }
            });
        };
        // remove mp3 from list
        $scope.removeMp3FromPlaylist = function (mp3Id, playlistId) {
            $http.post('/Playlists/RemoveMp3FromPlaylist', { mp3Id: mp3Id, playlistId: playlistId }).then(function (response) {
                //get index of deleted item in $scope
                var index = $scope.playlistDetails.MP3Files.map(function (element) {
                    return element.MP3FileID;
                }).indexOf(mp3Id);
                $scope.playlistDetails.MP3Files.splice(index, 1);
                $scope.showAlert("Record successfully removed!", "")
            }).catch(function onError(response) {
                $scope.showAlert("Error removing record", response.status + " " + response.statusText);
            });
        };
        $scope.showConfirm = function (ev, mp3Id, playlistId) {
            var confirm = $mdDialog.confirm()
                .title('Are you sure you want to remove this record from the playlist \"' + $scope.playlistDetails.Name+"\"?")
                .textContent('You can always add this record back to the playlist!')
                .ariaLabel('Remove record')
                .targetEvent(ev)
                .ok('Remove')
                .cancel('Cancel');

            $mdDialog.show(confirm).then(function () {
                $scope.removeMp3FromPlaylist(mp3Id, playlistId);
            });
        };
        $scope.showAlert = function (title, description, ev) {
            $mdDialog.show(
                $mdDialog.alert()
                    .parent(angular.element(document.querySelector('#popupContainer')))
                    .clickOutsideToClose(true)
                    .title(title)
                    .textContent(description)
                    .ariaLabel('Alert Dialog')
                    .ok('Got it!')
                    .targetEvent(ev)
            );
        };
    }])
