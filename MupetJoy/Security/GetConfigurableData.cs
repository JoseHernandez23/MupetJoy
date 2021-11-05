using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MupetJoy.Security
{
    public class GetConfigurableData
    {
        public string GetDataConfigurationManager(string Value)
        {
            string respuesta = string.Empty;

            switch (Value)
            {
                case "DiscountPercent":
                    respuesta = ConfigurationManager.AppSettings["DiscountPercent"];
                    break;

                case "CNX":
                    respuesta = ConfigurationManager.ConnectionStrings["Hana"].ConnectionString;
                    break;

                case "BDCountry":
                    respuesta = ConfigurationManager.AppSettings["ItalyBD"];
                    break;
             
                default:
                    respuesta = string.Empty;
                    break;
            }
            return respuesta;
        }
    }
}