angular.module("seekMailApp").controller("messagesController", [
    "$scope", "$http", "$routeParams", "parsePageLink", function ($scope, $http, $routeParams, parsePageLink) {
        $scope.messages = [];
        $scope.page = $routeParams.page || 1;
        $scope.loading = true;
        $scope.pages = {};

        // Gets the next/previous page number from API URL
        $scope.getPageNumber = function(url) {
            if (url)
                return url.substr(url.lastIndexOf('/') + 1);
            return null;
        };

        // Load page of messages
        $http.get('/api/messages/' + $scope.page).then(
            function(result) {
                // Success
                angular.copy(result.data, $scope.messages);

                // Next and previous links are in the headers
                $scope.pages = parsePageLink(result.headers("Link"));

                // This could certainly be better
                $scope.pages.next = $scope.getPageNumber($scope.pages["rel=next"]);
                $scope.pages.previous = $scope.getPageNumber($scope.pages["rel=previous"]);
            },
            function() {
                // TODO: Log to exception service
                console.log("An error occurred retrieving messages.");
            }).then(
            function() {
                $scope.loading = false;
            });
    }
]);