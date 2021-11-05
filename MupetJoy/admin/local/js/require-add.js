// Author: Jimmy Madrigal <jimmymadrigal@gmail.com>
// Filename: main.js
if (typeof window.require_add == 'undefined')
    window.require_add = { paths: {}, shim: {}, urlArgs: "" };

(function (obj) {

    var scripts = document.getElementsByTagName("script");
    var path = scripts[scripts.length - 1].src;
    var relativePath = path.substring(0, path.lastIndexOf("/js") + 1);
    var version = path.substring(path.lastIndexOf("?v=") + 3);

    obj.urlArgs = "v=" + version;
    obj.paths.router = relativePath + 'js/router';
    obj.paths.templates = relativePath + 'templates';
    obj.paths.globalConfigOld = obj.paths.globalConfig;
    obj.paths.globalConfig = relativePath + 'js/globalConfig';
    obj.paths.customConfig = relativePath + 'templates/config/form.config.custom.html';

})(window.require_add);
