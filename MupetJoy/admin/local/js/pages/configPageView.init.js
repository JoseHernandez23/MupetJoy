define([
  'text!templates/config/tr.company.html'
], function (companyRowTemplate) {
    return {
        initialize: function (configPageView) {
            configPageView.editView.params.rowTemplate["ImplantesServiceCompanies"] = companyRowTemplate;
        }
    }
});
