angular.module("seekMailApp").controller("templateDetailController", [
    "$scope", "$http", "$window", "$routeParams", function ($scope, $http, $window, $routeParams) {
        $scope.templateId = $routeParams.templateId;
        $scope.data = {};
        $scope.loading = false;

        // Save new template
        $scope.post = function (form) {
            if (form.$valid) {
                $http.post('/api/templates', $scope.data).then(
                    function () {
                        // Success
                        $window.location = "#/templates";
                    },
                    function () {
                        // TODO: Log to exception service
                        console.log("Error occurred saving new template.");
                    });
            }
        };

        // Save existing template
        $scope.put = function (form) {
            if (form.$valid) {
                $http.put('/api/templates/' + $scope.templateId, $scope.data).then(
                    function () {
                        // Success
                        $window.location = "#/templates";
                    },
                    function () {
                        // TODO: Log to exception service
                        console.log("Error occurred saving new template.");
                    });
            }
        };

        // Load existing template
        if ($scope.templateId) {
            $scope.loading = true;
            $http.get('api/templates/' + $scope.templateId).then(
                function(result) {
                    angular.copy(result.data, $scope.data);
                },
                function() {
                    console.log("Error occurred loading template.");
                }).then(
                function() {
                    $scope.loading = false;
                });
        }
    }
]);