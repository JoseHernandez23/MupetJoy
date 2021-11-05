// Filename: router.js
define([
  'jquery',
  'backbone',
  'globalConfig',
  'mupetRouter'
], function ($, Backbone, globalConfig , mupetRouter) {
    
    var AppRouter = Backbone.Router.extend({
        routes: {
            // Define some URL routes
            '': 'showHome',
            /*'planillas': 'showPlanillas',*/
        },
        clear: function () {
            mupetRouter.clear();
        },
        showHome: function () {
        },
        showPlanillas: function () {
            if (!mupetRouter.verifyUser([globalConfig.auth.ROLE_ADMIN/*, globalConfig.auth.ROLE_PLANILLAS*/])) return false;
            /*require(['pages/planillaPago'], function (view) { view.show(); });*/
        },
        

        //before all
        execute: function (callback, args, name) {
            this.clear();
            if (globalConfig.auth.enabled && globalConfig.getItem("role") == undefined) {
                mupetRouter.showLogin();
                return false;
            }
            //args.push(parseQueryString(args.pop()));
            if (callback) callback.apply(this, args);
        }
    });

    return new AppRouter;
});
