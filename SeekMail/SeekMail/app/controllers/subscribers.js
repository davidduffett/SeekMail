angular.module("seekMailApp").controller("subscribersController", [
    "$scope", "$http", function ($scope, $http) {
        $scope.subscribers = [];
        $scope.newSubscriber = {};
        $scope.loading = true;

        // Add a subscriber to the list
        $scope.addSubscriber = function (form) {
            if (form.$valid) {
                $http.post('/api/subscribers', $scope.newSubscriber).then(
                    function (result) {
                        // Success
                        $scope.subscribers.push(result.data);
                        $scope.newSubscriber = {};
                    },
                    function () {
                        // TODO: Log to exception service
                        console.log("Error occurred saving new subscriber.");
                    });
            }
        };

        // Remove a subscriber
        $scope.removeSubscriber = function (index) {
            var subscriber = $scope.subscribers[index];
            $scope.subscribers.splice(index, 1);
            $http.delete('/api/subscribers?emailAddress=' + subscriber.EmailAddress).then(
                function () {
                    // Success
                },
                function (data, status) {
                    // If a 404, subscriber has already been deleted
                    if (status != 404) {
                        // TODO: Log to exception service
                        console.log("Error occurred removing a subscriber.");
                        // Put him back!
                        $scope.subscribers.push(subscriber);
                    }
                });
        };

        // Get list of subscribers
        $http.get('/api/subscribers').then(
            function (result) {
                // Success
                angular.copy(result.data, $scope.subscribers);
            },
            function () {
                // TODO: Log to exception service
                console.log("Error occurred loading subscribers.");
            }).then(
            function () {
                $scope.loading = false;
            });
    }
]);