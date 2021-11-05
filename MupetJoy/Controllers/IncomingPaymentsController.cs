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

namespace MupetJoy.Controllers
{
    /// <summary>
    /// Incoming Payments Controller
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Payments")]
    public class IncomingPaymentsController : ApiController
    {
        [HttpPost]
        [Route("PostIncomingPayment")]
        public string PostIncomingPayment(string oDataPay)
        {
            var respuesta = string.Empty;
            try
            {
                int? logEntryIdRet = null;
                Payment_BLL Action = new Payment_BLL();
                PaymentModel oPay = null;              
                int? logEntryId = null;

                var result = new LogEntry()
                    .OverrideAction("Create Incoming Payment")
                    .OverrideInput(new { oDataPay })
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

                        IRestResponse response = Action.CreatePayment(oDataPay);

                        NVTResult res = null;
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.Created:
                                oPay = JsonConvert.DeserializeObject<PaymentModel>(response.Content);
                                res = new NVTResult(new { response.Content }, oPay.DocNum.ToString());
                                break;

                            case HttpStatusCode.BadRequest:
                                res = new NVTResult("Ocurrio un error al crear un pago");
                                break;

                            case HttpStatusCode.Unauthorized:
                                res = new NVTResult("No hay conexion o autorizacion para acceder a SAP");
                                break;

                            default:
                                res = new NVTResult("Ocurrio un error al crear un pago");
                                break;
                        }

                        return res;

                    })
                    .OnBeforeFinish((scenario, nvtResult) =>
                    {
                        if (!nvtResult.Success)
                        {
                            if (oPay != null)
                            {
                                return new NVTResult("Ocurrio un error al crear un pago. " + nvtResult.Error);
                            }
                            return new NVTResult("Ocurrio un error al crear un pago");
                        }
                        return nvtResult;
                    })
                    .Finish();

                logEntryIdRet = logEntryId;

                if (result.Success)
                {
                    respuesta = oPay.DocNum.ToString();
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
                respuesta = "Ocurrio un error al crear un pago " + ex.Message;
                return respuesta;
            }
        }
    }
}