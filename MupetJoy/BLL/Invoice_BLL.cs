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
    public class Invoice_BLL
    {
        public IRestResponse CreateInvoice(string oDataInv)
        {
            ClienteRestBLSAP clienteRest = new ClienteRestBLSAP();
            string URL = "/Invoices";
            string link = ConfigurationManager.AppSettings["URLServiceLayer"] + URL;
            IRestResponse response = clienteRest.EjecutarPost(link, oDataInv);           
            return response;
        }

        public IRestResponse GetDataInvoice(string DocNum)
        {
            ClienteRestBLSAP clienteRest = new ClienteRestBLSAP();
            string URL = "/Invoices?$select=*&$filter=DocNum eq " + DocNum;
            string link = ConfigurationManager.AppSettings["URLServiceLayer"] + URL;
            IRestResponse response = clienteRest.EjecutarGet(link);
            return response;
        }
    }
}