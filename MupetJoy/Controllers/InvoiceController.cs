using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using MupetJoy.Models;
using Novitec.Mupet.Common;
using Novitec.Mupet.Scenarios;
using System.Data;
using System.Data.Services.Client;
using Newtonsoft.Json;
using Novitec.Mupet.ServiceHelper;
using MupetPrintLibrary;
using Novitec.Mupet.Hana;
using Novitec.Mupet.Scenarios.Data;
using MupetJoy.Security;
using RestSharp;
using System.Net;
using MupetJoy.BLL;
using System.Threading.Tasks;

namespace MupetJoy.Controllers
{
    /// <summary>
    /// Invoice Controller
    /// </summary>
    [Authorize]
    [RoutePrefix("api/inv")]
    public class InvoiceController : ApiController
    {
        [HttpPost]
        [Route("PostCreateInvoice")]
        public string PostCreateInvoice(string oDataInv)
        {
            var respuesta = string.Empty;
            try
            {
                int? logEntryIdRet = null;
                Invoice_BLL Action = new Invoice_BLL();
                InvoiceModel oInvoice = null;
                int? logEntryId = null;

                var result = new LogEntry()
                    .OverrideAction("Create Invoice")
                    .OverrideInput(new { oDataInv })
                    .Start<SimpleProcess>()
                    .OnProcessingBody((scenario) =>
                    {
                        logEntryId = scenario.LogEntryModelId;
                        // Se verifica si la aplicación está logueada con SAP
                        if (String.IsNullOrEmpty(SessionSAP.SessionId) == true ||
                            DateTime.Now.Subtract(SessionSAP.FechaSesion) >= SessionSAP.Renovacion)
                        {
                            LoginSAP loginBL = new LoginSAP();
                            loginBL.Login();
                        }

                        IRestResponse response = Action.CreateInvoice(oDataInv);

                        NVTResult res = null;
                        int contador = 0;
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.Created:
                                oInvoice = JsonConvert.DeserializeObject<InvoiceModel>(response.Content);
                                res = new NVTResult(new { response.Content }, oInvoice.DocNum.ToString());
                                break;

                            case HttpStatusCode.BadRequest:
                                res = new NVTResult("Ocurrio un error al crear una Factura");
                                break;

                            case HttpStatusCode.Unauthorized:
                                res = new NVTResult("No hay conexion o autorizacion para acceder a SAP");
                                break;

                            default:
                                res = new NVTResult("Ocurrio un error al crear una Factura");
                                break;
                        }
                       
                        return res;

                    })
                    .OnBeforeFinish((scenario, nvtResult) =>
                    {
                        if (!nvtResult.Success)
                        {
                            if (oInvoice != null)
                            {
                                return new NVTResult("Ocurrio un error al crear una factura. " + nvtResult.Error);
                            }
                            return new NVTResult("Ocurrio un error al crear una factura");
                        }
                        return nvtResult;
                    })
                    .Finish();

                logEntryIdRet = logEntryId;

                if (result.Success)
                {
                    respuesta = oInvoice.DocNum.ToString();
                    return respuesta;
                }
                else
                {
                    respuesta = "-1";
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta = "Ocurrio un error al crear una factura " + ex.Message;
                return respuesta;
            }
        }

        /*[HttpGet]
        [Route("GetInvoice")]
        public object GetInvoice(string DocNum)
        {
            var respuesta = string.Empty;
            try
            {
                Invoice_BLL Action = new Invoice_BLL();
                int? logEntryIdRet = null;
                InvoiceModel oInvoiceModel = null;
                int? logEntryId = null;
                var result = new LogEntry()
                    .OverrideAction("Get Invoice")
                    .OverrideInput(new { DocNum })
                    .Start<SimpleProcess>()
                    .OnProcessingBody((scenario) =>
                    {
                        logEntryId = scenario.LogEntryModelId;
                        // Se verifica si la aplicación está logueada con SAP
                        if (String.IsNullOrEmpty(SessionSAP.SessionId) == true ||
                            DateTime.Now.Subtract(SessionSAP.FechaSesion) >= SessionSAP.Renovacion)
                        {
                            LoginSAP loginBL = new LoginSAP();
                            loginBL.Login();
                        }

                        IRestResponse response = Action.GetDataInvoice(DocNum);

                        NVTResult res = null;
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                oInvoiceModel = JsonConvert.DeserializeObject<InvoiceModel>(response.Content);
                                res = new NVTResult(new { response.Content }, oInvoiceModel.CardCode);
                                break;

                            case HttpStatusCode.BadRequest:
                                res = new NVTResult("Ocurrio un error al consultar una factura");
                                break;

                            case HttpStatusCode.Unauthorized:
                                res = new NVTResult("No hay conexion o autorizacion para acceder a SAP");
                                break;

                            default:
                                res = new NVTResult("Ocurrio un error al consultar una factura");
                                break;
                        }
                        return res;

                    })
                    .OnBeforeFinish((scenario, nvtResult) =>
                    {
                        if (!nvtResult.Success)
                        {
                            if (oInvoiceModel != null)
                            {
                                return new NVTResult("Ocurrio un error al verificar una factura. " + nvtResult.Error);
                            }
                            return new NVTResult("Ocurrio un error al consultar una factura");
                        }
                        return nvtResult;

                    })
                    .Finish();

                logEntryIdRet = logEntryId;

                if (result.Success)
                {
                    var res = oInvoiceModel;
                    return res;
                }
                else
                {
                    respuesta = "-1 error al consultar una factura";
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta = "Ocurrio un error al consultar una factura " + ex.Message;
                return respuesta;
            }
        }*/
    }
}