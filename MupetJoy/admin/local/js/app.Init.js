
define(['backbone', 'mupetPages/mupetView.Init', 'text!templates/menu.html'],
function (Backbone, MupetView, menuTemplate) {

    var initialize = function () {
        MupetView.initialize();
        $('#top_menu').html(menuTemplate);

        require(['router'], function (Router) {
            //Router.initialize();
            Backbone.history.start();
        });
    };

    return {
        initialize: initialize
    };
});


