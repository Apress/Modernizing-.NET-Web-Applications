using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ModernizationDemo.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(config =>
            {
                WebApiConfig.Register(config);
                SwaggerConfig.Register(config);
            });
        }
    }
}
