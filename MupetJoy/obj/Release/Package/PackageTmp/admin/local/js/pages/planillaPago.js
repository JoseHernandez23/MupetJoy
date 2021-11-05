////define([
////    'jquery',
////    'underscore',
////    'backbone',
////    'globalConfig',
////    'pageViewClass',
////    'formViewClass',
////    'text!templates/planillaPago/form.html',
////],
////function ($, _, Backbone, globalConfig, PageViewClass, FormViewClass, formTemplate) {

////        //crear Page View
////        var pageView = new PageViewClass({}, {
////            id:"planilla_pago_page",
////            title: 'Planillas pago',
////            elements:
////            {
////                form_template: formTemplate
////            },
////        });

////        //crear Search View
////        var formView = new FormViewClass({ el: '#planilla_pago_page #form_template' });

////        /*** Enlazar vistas con eventos ***/

////        //Eventos de pagina: resize de grid cunado la pagina s emuestra
////        pageView.on('show', function () {
////            formView.$("input[name=_t]").val(globalConfig.getItem("token"));
////            //primer carga
////        });

////        formView.on('submit', function (data) {

////            var form = formView.$form;

////            $.ajax({
////                url: pageView.urlPrefix + "/PagoPlanillaBN.asmx/CargaArchivoPago",
////                data: new FormData(form[0]),
////                type: "post",
////                contentType: false,
////                processData: false,
////                cache: false,
////                dataType: "json",
////                //error: function (err) {
////                //    alert(err);
////                //},
////                //success: function (data) {
////                //    alert("Document uploaded successfully.");
////                //},
////            })
////            .done(function (response) { 
////                if (response.success) {

////                    formView.$("input[name=planilla]").val('');
////                    formView.$("label[for=resultado]").html(response.sapHook);
////                    alert("Carga correcta");
////                }
////                else {
////                    alert(response.error);
////                }
////                });
////        });
////        return pageView;
////});