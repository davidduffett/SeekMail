// Our application, dependent on the Angular routes module
angular.module("seekMailApp", ['ngRoute']);

// Configure routes
angular.module("seekMailApp").config(["$routeProvider", function ($routeProvider) {
    $routeProvider.when("/", {
        controller: "homeController",
        templateUrl: "/app/views/home.html"
    });

    $routeProvider.when("/templates", {
        controller: "templatesController",
        templateUrl: "/app/views/templates.html"
    });

    $routeProvider.when("/templates/new", {
        controller: "templateDetailController",
        templateUrl: "/app/views/newTemplate.html"
    });

    $routeProvider.when("/templates/:templateId", {
        controller: "templateDetailController",
        templateUrl: "/app/views/editTemplate.html"
    });

    $routeProvider.when("/subscribers", {
        controller: "subscribersController",
        templateUrl: "/app/views/subscribers.html"
    });

    $routeProvider.when("/messages", {
        controller: "messagesController",
        templateUrl: "/app/views/messages.html"
    });

    $routeProvider.when("/messages/:page", {
        controller: "messagesController",
        templateUrl: "/app/views/messages.html"
    });
}]);
