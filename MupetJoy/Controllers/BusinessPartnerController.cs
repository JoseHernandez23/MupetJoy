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
    /// Business Partner Controller
    /// </summary>
    [Authorize]
    [RoutePrefix("api/bp")]
    public class BusinessPartnerController : ApiController
    {
        [HttpPost]
        [Route("PostBusinessPartner")]
        public string PostBusinessPartner(string oDataBP)
        {
            var respuesta = string.Empty;
            try
            {
                int? logEntryIdRet = null;
                BusinessPartner_BLL Action = new BusinessPartner_BLL();
                BusinessPartnerModel oBusinessPartner = null;             
                int? logEntryId = null;

                var result = new LogEntry()
                    .OverrideAction("Save Business Partner")
                    .OverrideInput(new { oDataBP })
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

                        IRestResponse response = Action.CreateBusinessPartner(oDataBP);

                        NVTResult res = null;
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.Created:
                                oBusinessPartner = JsonConvert.DeserializeObject<BusinessPartnerModel>(response.Content);
                                res = new NVTResult(new { response.Content }, oBusinessPartner.CardCode);
                                break;

                            case HttpStatusCode.BadRequest:
                                res = new NVTResult("Ocurrio un error al crear un Business partner");
                                break;

                            case HttpStatusCode.Unauthorized:
                                res = new NVTResult("No hay conexion o autorizacion para acceder a SAP");
                                break;

                            default:
                                res = new NVTResult("Ocurrio un error al crear un Business partner");
                                break;
                        }

                        return res;

                    })
                    .OnBeforeFinish((scenario, nvtResult) =>
                    {
                        if (!nvtResult.Success)
                        {
                            if (oBusinessPartner != null)
                            {
                                return new NVTResult("Ocurrio un error al crear el Business Partner. " + nvtResult.Error);
                            }
                            return new NVTResult("Ocurrio un error al crear un Business partner");
                        }
                        return nvtResult;
                    })
                    .Finish();

                logEntryIdRet = logEntryId;

                if (result.Success)
                {
                    respuesta = oBusinessPartner.CardCode;
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
                respuesta = "Ocurrio un error al crear un Business partner " + ex.Message;
                return respuesta;
            }


        }

        /*[HttpPost]
        [Route("PostBusinessPartner")]
        public string PostBusinessPartner(string oDataBP)
        {
            var respuesta = string.Empty;
            try
            {
                BusinessPartner_BLL Action = new BusinessPartner_BLL();
                BusinessPartnerModel oBusinessPartner = null;
              
                // Se verifica si la aplicación está logueada con SAP
                if (String.IsNullOrEmpty(SessionSAP.SessionId) == true ||
                    DateTime.Now.Subtract(SessionSAP.FechaSesion) >= SessionSAP.Renovacion)
                {
                    LoginSAP loginBL = new LoginSAP();
                    loginBL.Login();
                }

                IRestResponse response = Action.CreateBusinessPartner(oDataBP);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        oBusinessPartner = JsonConvert.DeserializeObject<BusinessPartnerModel>(response.Content);
                        respuesta = oBusinessPartner.CardCode;
                        break;

                    case HttpStatusCode.BadRequest:
                        respuesta = "-1";
                        break;

                    case HttpStatusCode.Unauthorized:
                        respuesta = "-1";
                        break;

                    default:
                        respuesta = "-1";
                        break;
                }

                return respuesta;

            }
            catch (Exception ex)
            {
                respuesta = "Ocurrio un error al crear un Business partner " + ex.Message;
                return respuesta;
            }


        }*/

        [HttpGet]
        [Route("GetBusinessPartner")]
        public object GetBusinessPartner(string CardCode)
        {
            var respuesta = string.Empty;
            try
            {
                BusinessPartner_BLL Action = new BusinessPartner_BLL();
                int? logEntryIdRet = null;

                BusinessPartnerModel oBusinessPartner = null;
                int? logEntryId = null;
                var result = new LogEntry()
                    .OverrideAction("Get Business Partner")
                    .OverrideInput(new { CardCode })
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

                        IRestResponse response = Action.VerifyBusinessPartner(CardCode);

                        NVTResult res = null;
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                oBusinessPartner = JsonConvert.DeserializeObject<BusinessPartnerModel>(response.Content);
                                res = new NVTResult(new { response.Content }, oBusinessPartner.CardCode);
                                break;

                            case HttpStatusCode.BadRequest:
                                res = new NVTResult("Ocurrio un error al consultar un Business partner");
                                break;

                            case HttpStatusCode.Unauthorized:
                                res = new NVTResult("No hay conexion o autorizacion para acceder a SAP");
                                break;

                            default:
                                res = new NVTResult("Ocurrio un error al consultar un Business partner");
                                break;
                        }
                        return res;

                    })
                    .OnBeforeFinish((scenario, nvtResult) =>
                    {
                        if (!nvtResult.Success)
                        {
                            if (oBusinessPartner != null)
                            {
                                return new NVTResult("Ocurrio un error al verificar un Business Partner. " + nvtResult.Error);
                            }
                            return new NVTResult("Ocurrio un error al consultar un Business partner");
                        }
                        return nvtResult;

                    })
                    .Finish();

                logEntryIdRet = logEntryId;

                if (result.Success)
                {
                    var res = oBusinessPartner;
                    return res;
                }
                else
                {
                    respuesta = "-1";
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta = "Ocurrio un error al consultar un Business partner " + ex.Message;
                return respuesta;
            }


        }

       
        //En caso de que soliciten actualizar un Business Partner
        /*[HttpPatch]
        [Route("PatchBusinessPartner")]
        public object PatchBusinessPartner(string oDataBP)
        {
            var respuesta = string.Empty;
            try
            {
                BusinessPartner_BLL Action = new BusinessPartner_BLL();
                int? logEntryIdRet = null;
                SaveBP BP = null;
                
                SaveBP oBusinessPartner = null;
                int? logEntryId = null;
                var result = new LogEntry()
                    .OverrideAction("Update Business Partner")
                    .OverrideInput(new { oDataBP })
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

                        IRestResponse response = Action.UpdateBusinessPartner(oDataBP);
                        oBusinessPartner = JsonConvert.DeserializeObject<SaveBP>(response.Content);

                    //return new NVTResult(oBusinessPartner, CardCode);
                    return new NVTResult(new { oBusinessPartner }, oBusinessPartner.CardCode);

                    })
                    .OnBeforeFinish((scenario, nvtResult) =>
                    {

                        if (!nvtResult.Success)
                        {
                            if (oBusinessPartner != null)
                            {
                                return new NVTResult("Ocurrio un error al actualizar el Business Partner. " + nvtResult.Error);
                            }
                            return new NVTResult("Ocurrio un error al actualizar un Business partner");
                        }
                        return nvtResult;

                    })
                    .Finish();

                logEntryIdRet = logEntryId;

                if (result.Success)
                {
                    BP = oBusinessPartner;
                    return BP;
                }
                else
                {
                    respuesta = "-1";
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta = "Ocurrio un error al actualizar un Business partner " + ex.Message;
                return respuesta;
            }
            

        }*/


    }
}