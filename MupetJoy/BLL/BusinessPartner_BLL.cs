using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MupetJoy.Models;
using MupetJoy.Security;
using Newtonsoft.Json;
using RestSharp;

namespace MupetJoy.BLL
{
    public class BusinessPartner_BLL
    {
        public IRestResponse CreateBusinessPartner(string oDataBP)
        {
            ClienteRestBLSAP clienteRest = new ClienteRestBLSAP();
            string URL = "/BusinessPartners";
            string link = ConfigurationManager.AppSettings["URLServiceLayer"] + URL;
            IRestResponse response = clienteRest.EjecutarPost(link, oDataBP);
            //var result = await Task.Run(() => {
            //    return response;
            //});

            //return result;
            return response;
        }

        public IRestResponse VerifyBusinessPartner(string CardCode)
        {
            ClienteRestBLSAP clienteRest = new ClienteRestBLSAP();
            string URL = "/BusinessPartners(Number)";
            string URL2 = URL.Replace("Number", "'" + CardCode + "'");
            string link = ConfigurationManager.AppSettings["URLServiceLayer"] + URL2;
            IRestResponse response = clienteRest.EjecutarGet(link);
            return response;
        }

        public IRestResponse UpdateBusinessPartner(string oDataBP)
        {
            ClienteRestBLSAP clienteRest = new ClienteRestBLSAP();
            string URL = "/BusinessPartners(Number)";
            string URL2 = URL.Replace("Number", "'" + oDataBP + "'");
            string link = ConfigurationManager.AppSettings["URLServiceLayer"] + URL2;
            IRestResponse response = clienteRest.EjecutarPatch(link, oDataBP);
            return response;
        }
    }
}