using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MupetJoy.Security
{
    public class RequestLoginSAP
    {
        public string CompanyDB { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}