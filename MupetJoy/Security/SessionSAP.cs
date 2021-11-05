using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MupetJoy.Security
{
    public static class SessionSAP
    {
        /// <summary>
        /// Sesion temporal de SAP
        /// </summary>
        public static string SessionId { get; set; }
        public static DateTime FechaSesion { get; set; }

        public static TimeSpan Renovacion = new TimeSpan(0, 20, 0);
    }
}