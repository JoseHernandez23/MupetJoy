// Filename: router.js
define([
  'jquery',
  'backbone',
  'mupetRouter',
  'globalConfig'
], function ($, Backbone, mupetRouter, globalConfig) {

    var AppRouter = Backbone.Router.extend({
        routes: {
            // Define some URL routes
            '': 'showHome'
        },
        clear: function () {
            mupetRouter.clear();
        },
        showHome: function () {
        },
        //before all
        execute: function (callback, args, name) {
            this.clear();
            if (globalConfig.getItem("role") == undefined) {
                mupetRouter.showLogin();
            }
            if (callback) callback.apply(this, args);
        }
    });


    return new AppRouter;
});
