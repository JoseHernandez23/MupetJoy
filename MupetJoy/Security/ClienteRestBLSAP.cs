using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace MupetJoy.Security
{
    public class ClienteRestBLSAP
    {
        #region Métodos Públicos

        public IRestResponse EjecutarPostLogin(string link, string parametro)
        {
            RestClient client = new RestClient(link);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");

            if (string.IsNullOrEmpty(SessionSAP.SessionId) == false)
            {
                request.AddHeader("B1SESSION", SessionSAP.SessionId);
            }
            request.AddParameter("application/json", parametro, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            this.LeerCreate(response);

            return response;
        }

        public IRestResponse EjecutarPost(string link, string parametro)
        {
            RestClient client = new RestClient(link);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");

            if (string.IsNullOrEmpty(SessionSAP.SessionId) == false)
            {
                request.AddCookie("B1SESSION", SessionSAP.SessionId);
            }
            request.AddParameter("application/json", parametro, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            this.LeerCreate(response);

            return response;
        }

        public IRestResponse EjecutarPost(string link)
        {
            RestClient client = new RestClient(link);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            if (string.IsNullOrEmpty(SessionSAP.SessionId) == false)
            {
                request.AddCookie("B1SESSION", SessionSAP.SessionId);
            }

            IRestResponse response = client.Execute(request);

            this.LeerCreate(response);

            return response;
        }

        public IRestResponse EjecutarPatch(string link, string parametro)
        {
            RestClient client = new RestClient(link);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            RestRequest request = new RestRequest(Method.PATCH);
            request.AddHeader("Content-Type", "application/json");
            if (string.IsNullOrEmpty(SessionSAP.SessionId) == false)
            {
                request.AddCookie("B1SESSION", SessionSAP.SessionId);
            }
            request.AddParameter("application/json", parametro, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            this.LeerUpdate(response);

            return response;
        }
      
        public IRestResponse EjecutarGet(string link)
        {
            RestClient client = new RestClient(link);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            if (string.IsNullOrEmpty(SessionSAP.SessionId) == false)
            {
                request.AddCookie("B1SESSION", SessionSAP.SessionId);
            }
            
            IRestResponse response = client.Execute(request);

            this.LeerGet(response);

            return response;
        }
        #endregion

        #region Métodos Privados
        private RestClient CrearCliente(string link)
        {
            RestClient client = new RestClient(link);

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            /*if (this.appSettingsAPI.ValidarSSL == false)
            {
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }*/

            return client;
        }

        private void LeerGet(IRestResponse response)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return;
            }

            this.LeerStatus(response);
        }

        private void LeerCreate(IRestResponse response)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return;
            }

            this.LeerStatus(response);
        }

        private void LeerUpdate(IRestResponse response)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return;
            }

            this.LeerStatus(response);
        }

        private void LeerStatus(IRestResponse response)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            //if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            //{
            //    throw new Exception("La sesion ha expirado");
            //}

            //if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            //{
            //    throw new Exception(RecursoMensaje.ErrorSAPBadRequest);
            //}

            //if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            //{
            //    throw new Exception("No encontrado");
            //}

            //RootErrorSAP error = JsonConvert.DeserializeObject<RootErrorSAP>(response.Content);

            /*if (error != null && error.error != null && error.error.message != null)
            {
                throw new Exception(error.error.message.value);
            }*/
        }
        #endregion
    }
}