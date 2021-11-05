using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MupetJoy.Models
{
    public class ResponseProcessOrder
    {
        public string Error { get; set; }
        public string BusinessPartner { get; set; }
        public string Invoice { get; set; }
        public string Payment { get; set; }
    }
}