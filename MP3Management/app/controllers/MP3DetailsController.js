angular.module("MP3DetailsController", ['ngMaterial'])
    .controller("MP3DetailsCtrl", ["$scope", "$http", "$routeParams", "$mdDialog", function ($scope, $http, $routeParams, $mdDialog) {
        $scope.mp3details = {};
        $scope.status = null;
        $scope.playlists = {};
        $scope.states = {
            allowEditMP3File: false
        };
        $scope.new = {
            MP3File: {}
        }
        // routing parametar
        $http({
            url: "/MP3File/MP3Details",
            params: { id: $routeParams.id },
            method: "get"
        })
            .then(function (response) {
                $scope.mp3details = response.data;
            }).catch(function onError(response) {
                //$scope.status = response.status + " " + response.statusText;
                $scope.status = "Record doesn't exist or deleted!";
            });
        // get all playlists
        $http.get("/Playlists/Index").then(function (data) {
            $scope.playlists = data;
        });
        // show and fill edit form
        $scope.editMP3File = function (id, name, author, albumName) {
            $scope.new.MP3File = { MP3FileID: id, Name: name, Author: author, AlbumName: albumName };
            $scope.states.allowEditMP3File = true;
        };
        // edit confirm
        $scope.updateMP3File = function () {
            $http.post('/MP3File/Edit', $scope.new.MP3File).then(function (response) {
                $scope.mp3details = response.data;
                $scope.cancelEdit();
            });
        };
        // hide edit form
        $scope.cancelEdit = function () {
            $scope.new.MP3File = {};
            $scope.states.allowEditMP3File = false;
        };
        // delete mp3 file
        $scope.deleteMP3File = function (id) {
            $http.post("/MP3File/Delete", { id: id }).then(function onSuccess(response) {
                // display message on the screen
                $scope.status = "Record doesn't exist or deleted!";
                //alert message
                $scope.showAlert("Record successfully deleted!", "")
            }).catch(function onError(response) {
                $scope.showAlert("Error deleting record", response.status + " " + response.statusText);
                });
        };
        //add to playlist
        $scope.announceClick = function (mp3Id, playlistId) {
            $http({
                method: 'POST',
                url: '/MP3File/AddToPlaylist',
                headers: {
                    'Content-Type': undefined
                },
                params: { mp3Id: mp3Id, playlistId: playlistId }
            }).then(function successCallback(response) {
                //push playlist to mp3file
                $scope.mp3details.Playlists.push(response.data);
                $scope.showAlert("Record successfully added to the playlist!", "");
            }, function errorCallback(response) {
                // only distinct elements allowed in playlist
                if (response.status == 400) {
                    $scope.showAlert("Error adding record", "Record already exists!");
                } else {
                    $scope.showAlert("Error adding record", response.status + " " + response.statusText);
                }
            });
        };
        // confirm dialog
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
        // alert dialog
        $scope.showAlert = function (title, description, ev ) {
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
    }]) // card themes
    .config(function ($mdThemingProvider) {
        $mdThemingProvider.theme('dark-grey').backgroundPalette('grey').dark();
        $mdThemingProvider.theme('dark-orange').backgroundPalette('orange').dark();
        $mdThemingProvider.theme('dark-purple').backgroundPalette('deep-purple').dark();
        $mdThemingProvider.theme('dark-blue').backgroundPalette('blue').dark();
    });

