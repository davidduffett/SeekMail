// Service for parsing rel=next and rel=previous links from headers
// Nuts and bolts sourced here: https://gist.github.com/niallo/3109252
angular.module("seekMailApp").factory("parsePageLink", function() {
    return function(header) {
        // Split parts by comma
        var parts = header.split(',');
        var links = {};

        // Parse each part into a named link
        angular.forEach(parts, function (p) {
            var section = p.split(';');
            if (section.length != 2) {
                throw new Error("section could not be split on ';'");
            }
            var url = section[0].replace(/<(.*)>/, '$1').trim();
            var name = section[1].replace(/rel="(.*)"/, '$1').trim();
            links[name] = url;
        });

        return links;
    };
});