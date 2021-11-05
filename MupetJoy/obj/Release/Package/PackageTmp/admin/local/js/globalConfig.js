
var less = {};

define([
  'jquery',
  'globalConfigOld',
], function ($, globalConfigOld) {
    return $.extend(globalConfigOld, {
        dev: true,
        cliente: "Elabs Joy",
        auth: {
            enabled: false,
            ROLE_ADMIN: 'Admin'
            /*ROLE_PLANILLAS: 'Planillas',*/
    }
    });
});