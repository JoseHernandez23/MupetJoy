using System;
using System.Web.Http;
using MupetJoy.App_Start;

namespace MupetJoy
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
