angular.module('MP3FilesController', ['ngMaterial'])
    .controller('MP3FilesCtrl', ['$scope', '$http', '$mdDialog', '$window',  function ($scope, $http, $mdDialog, $window) {
        $scope.model = {};
        $scope.playlists = {};
        // show/hide edit and cancel buttons
        $scope.states = {
            allowEditMP3File: false,
            isLoading: true,
            search: false
        };
        $scope.new = {
            MP3File: {}
        };
        // get all mp3 files
        $http.get("/MP3File/Index").then(function (data) {
            $scope.model = data;
        });
        //get only list of playlists
        $http.get("/Playlists/Index").then(function (data) {
            $scope.playlists = data;
            $scope.states.isLoading = false;
        });
        // new mp3 file
        $scope.addMP3File = function () {
            $http.post('/MP3File/Create', $scope.new.MP3File).then(function (response) {
                $scope.model.data.push(response.data);
                $scope.cancelEdit();
            });
            $scope.cancelEdit();
        };
        // edit mp3 file
        $scope.updateMP3File = function () {
            $http.post('/MP3File/Edit', $scope.new.MP3File).then(function (response) {
                $http.get("/MP3File/Index").then(function (data) {
                    $scope.model = data;
                });
                // sets states.allowEditMP3File: false 
                $scope.cancelEdit();
            });
        };
        // delete mp3 file 
        $scope.deleteMP3File = function (id) {
            $http.post("/MP3File/Delete", { id: id }).then(function (response) {
                $http.get("/MP3File/Index").then(function (data) {
                    $scope.model = data;
                });
                $scope.showAlert("Record successfully deleted!", "");
            }).catch(function onError(response) {
                $scope.showAlert("Error deleting record", response.status + " " + response.statusText);
            });
        };
        // hide edit and cancel buttons; show button "New"
        $scope.cancelEdit = function () {
            $scope.new.MP3File = {};
            $scope.states.allowEditMP3File = false;
            $scope.inputForm.$setUntouched();
            $scope.inputForm.$setPristine();
        };
        // shows edit form, filled with mp3 data
        $scope.editMP3File = function (id, name, author, albumName) {
            $scope.new.MP3File = { MP3FileID: id, Name: name, Author: author, AlbumName: albumName };
            $scope.states.allowEditMP3File = true;
            $window.scrollTo(0, 0);
        };
        // add mp3file to playlist
        $scope.announceClick = function (mp3Id, playlistId) {
            $http.post('/MP3File/AddToPlaylist', { mp3Id: mp3Id, playlistId: playlistId }).then(function (response) {
                //push playlist to mp3file
                var index = $scope.model.data.map(function (element) {
                    return element.MP3FileID;
                }).indexOf(mp3Id);
                $scope.model.data[index].Playlists.push(response.data);
                $scope.showAlert("Record successfully added to the playlist!", "");
            }).catch(function onError(response) {
                if (response.status === 400) {
                    $scope.showAlert("Error adding record", "Record already exists!");
                } else {
                    $scope.showAlert("Error adding record", response.status + " " + response.statusText);
                }
            });
        };
        // search, parametars are searching string and searchBy selection. Possible implementation without SearchBy selection in .cs controller
        $scope.searchMP3files = function (searchString, searchBy) {
            $scope.states.search = true;
            $http({
                url: "/MP3File/Search",
                params: { SearchString: searchString, SearchBy: searchBy },
                method: "get"
            })
                .then(function (response) {
                    $scope.model = response;
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
    }]);