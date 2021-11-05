// Author: Jimmy Madrigal <jimmymadrigal@gmail.com>
// Filename: main.js
if (typeof window.require_add == 'undefined')
    window.require_add = { paths: {}, shim: {}, urlArgs: "" };

(function (obj) {

    require.config({
        urlArgs: obj.urlArgs,
        shim: obj.shim,
        paths: obj.paths,
        waitSeconds: 0
        //enforceDefine: true

    });

    require([
      //'loginPage',
      // Load our app module and pass it to our definition function
      'app.Init'

      // Some plugins have to be loaded in order due to their non AMD compliance
      // Because these scripts are not "modules" they do not pass any values to the definition function below
    ], function (App) {
        // The "app" dependency is passed in as "App"
        // Again, the other dependencies passed in are not "AMD" therefore don't pass a parameter to this function
        $(document).ready(function () {
            console.log('Main!');
            App.initialize();
        });
    });
})(window.require_add);
