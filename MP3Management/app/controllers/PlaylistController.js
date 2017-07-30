angular.module("PlaylistController", ['ngMaterial'])
    .controller("PlaylistCtrl", ["$scope", "$http", "$mdDialog", function ($scope, $http, $mdDialog) {
        $scope.model = {};
        $scope.new = {
            Playlist: {}
        }
        $scope.states = {
            allowEditPlaylist: false
        };
        $scope.status = '  ';
        $scope.customFullscreen = false;
        //get all playlists
        $http.get("/Playlists/Index").then(function (data) {
            $scope.model = data;
        });
        // create new playlist
        $scope.createPlaylist = function () {
            // check non empty object
            if (Object.keys($scope.new.Playlist).length > 0 ) {
                $http.post('/Playlists/Create', $scope.new.Playlist).then(function (response) {
                    $scope.model.data.push(response.data);
                    $scope.cancelEdit();
                });
            }
        };
        // edit playlist info
        $scope.updatePlaylist = function () {
            $http.post('/Playlists/Edit', $scope.new.Playlist).then(function (response) {
                // delete old playlist from the list and push edited
                var index = $scope.model.data.map(function (element) {
                    return element.PlaylistID;
                }).indexOf($scope.new.Playlist.PlaylistID);
                $scope.model.data.splice(index, 1);

                $scope.model.data.push(response.data);
                $scope.cancelEdit();
            });
        };
        // fill edit form with playlist info
        $scope.editPlaylist = function (id, name, description) {
            $scope.new.Playlist = { PlaylistID: id, Name: name, Description: description };
            $scope.states.allowEditPlaylist = true;
        };
        // hide edit and cancel button
        $scope.cancelEdit = function () {
            $scope.new.Playlist = {};
            $scope.states.allowEditPlaylist = false;
        };
        // delete playlist
        $scope.deletePlaylist = function (id) {
            $http.post("/Playlists/Delete", { id: id }).then(function (response) {
                $http.get("/Playlists/Index").then(function (data) {
                    $scope.model = data;
                });
                $scope.showAlert("Playlist successfully deleted!", "")
            }).catch(function onError(response) {
                $scope.showAlert("Error deleting playlist", response.status + " " + response.statusText);
            });
        };
        //confirm dialog
        $scope.showConfirm = function (ev, PlaylistID, PlaylistName) {
            var confirm = $mdDialog.confirm()
                .title("Are you sure you want to delete the playlist \"" + PlaylistName+"\"?")
                .textContent('The action is irreversible!')
                .ariaLabel('Delete playlist')
                .targetEvent(ev)
                .ok('Delete')
                .cancel('Cancel');

            $mdDialog.show(confirm).then(function () {
                $scope.deletePlaylist(PlaylistID);
            });
        };
        //alert dialog
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
