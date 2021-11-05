
define([
  'jquery',
  'globalConfigOld',
], function ($, globalConfigOld) {
    return $.extend(true, globalConfigOld, {
        dev: true,
        cliente: "Establishment Labs Joy",
        sapVersion: 'hana',
        auth: {
            enabled: true
        }
    });

});