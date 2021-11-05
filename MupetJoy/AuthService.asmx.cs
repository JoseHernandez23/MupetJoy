using Novitec.Mupet.Common;
using Novitec.Mupet.ServiceHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace MupetJoy
{
    /// <summary>
    /// Servicios llamados por el sition administrador
    /// </summary>
    [WebService(Namespace = "")]
    [System.ComponentModel.ToolboxItem(false)]
    public class AuthService : WebServiceAuthBase
    {
        public const string ROLE_PORTAL = "Portal";

        [WebMethod]
        public new void Roles()
        {
            var list = AuthServiceHelper.MupetUser.Roles();
            list.Add(ROLE_PORTAL);
            returnJson(list);
        }
    }
}
