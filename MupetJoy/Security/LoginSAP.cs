using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MupetJoy.Security
{
    public class LoginSAP
    {
        #region Métodos públicos
        public ResponseLoginSAP Login()
        {
            ResponseLoginSAP responseLogin = null;
            try
            {
                string link = ConfigurationManager.AppSettings.Get("URLServiceLayer") + "/Login";

                RequestLoginSAP solicitud = new RequestLoginSAP();
                solicitud.CompanyDB = ConfigurationManager.AppSettings.Get("CompanyDB");
                solicitud.Password = ConfigurationManager.AppSettings.Get("PasswordSAP");
                solicitud.UserName = ConfigurationManager.AppSettings.Get("UserNameSAP");

                string parametro = JsonConvert.SerializeObject(solicitud);

                ClienteRestBLSAP clienteRest = new ClienteRestBLSAP();

                IRestResponse response = clienteRest.EjecutarPostLogin(link, parametro);

                responseLogin = JsonConvert.DeserializeObject<ResponseLoginSAP>(response.Content);
                SessionSAP.FechaSesion = DateTime.Now;
                SessionSAP.SessionId = responseLogin.SessionId;               

            }
            catch (Exception ex)
            {
                //this.manejadorError.ManejarError(ex);
            }

            return responseLogin;
        }
        #endregion
    }
}