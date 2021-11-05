define(['backbone', 'globalConfig', 'mupetPages/mupetView.Init', 'text!templates/menu/admin.html', /*'text!templates/menu/planillas.html',*/ ],
function (Backbone, globalConfig, MupetView, menuAdminTemplate/*, menuPlanillasTemplate*/) {

    var initialize = function () {


        MupetView.initialize();
        
        if (!globalConfig.auth.enabled) {
            $('#top_menu').html(menuAdminTemplate)
        } else if (globalConfig.getItem("role") != undefined) {
            switch (globalConfig.getItem("role")) {
                case "Admin":
                    $('#top_menu').html(menuAdminTemplate);
                    break;
                //case "Planillas":
                //    $('#top_menu').html(menuPlanillasTemplate);
                //    break;
            }
        }

        require(['router'], function (Router) {
            //Router.initialize();
            Backbone.history.start();
        });
    };

    return {
        initialize: initialize
    };
});

