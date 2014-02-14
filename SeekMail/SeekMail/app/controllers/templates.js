angular.module("seekMailApp").controller("templatesController", [
    "$scope", "$http", function ($scope, $http) {
        $scope.templates = [];
        $scope.loading = true;
        $scope.testEmailAddress = null;

        // Send a test message for the given template
        $scope.testMessage = function (index) {
            $scope.testEmailAddress = prompt("Send to email address:", $scope.testEmailAddress);
            if ($scope.testEmailAddress) {
                var template = $scope.templates[index];
                template.sending = true;
                $http.post('/api/templates/' + template.Id + '/send', { emailAddress: $scope.testEmailAddress }).then(
                    function() {
                        alert("Test message sent.");
                    },
                    function() {
                        alert("An error occurred sending your test message.");
                    }).then(
                    function() {
                        template.sending = false;
                    });
            }
        };

        // Load template list
        $http.get('/api/templates').then(
            function (result) {
                // Success
                angular.copy(result.data, $scope.templates);
            },
            function () {
                // TODO: Log to exception service
                console.log("Error occurred loading templates.");
            }).then(
            function () {
                $scope.loading = false;
            });
    }
]);