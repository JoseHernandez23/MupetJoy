using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MupetJoy.Models;
using MupetJoy.Security;
using Newtonsoft.Json;
using RestSharp;

namespace MupetJoy.BLL
{
    public class Payment_BLL
    {
        public IRestResponse CreatePayment(string oDataPay)
        {
            ClienteRestBLSAP clienteRest = new ClienteRestBLSAP();
            string URL = "/IncomingPayments";
            string link = ConfigurationManager.AppSettings["URLServiceLayer"] + URL;
            IRestResponse response = clienteRest.EjecutarPost(link, oDataPay);
            return response;
        }
    }
}