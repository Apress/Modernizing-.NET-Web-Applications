using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ModernizationDemo.App.Handlers;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.App
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.Add("ProductsRss", new Route("products/rss", new GenericRouteHandler<ProductsRssHandler>()));

            SystemWebAdapterConfiguration.AddSystemWebAdapters(this)
                .AddProxySupport(options => options.UseForwardedHeaders = true)
                .AddJsonSessionSerializer(options =>
                {
                    options.RegisterKey<string>("Currency");
                })
                .AddRemoteAppServer(options => options.ApiKey = ConfigurationManager.AppSettings["RemoteAppApiKey"])
                .AddSessionServer()
                .AddAuthenticationServer();
        }

        public static ApiClient GetApiClient()
        {
            var httpClient = new HttpClient();
            return new ApiClient(ConfigurationManager.AppSettings["ApiBaseUrl"], httpClient);
        }
    }
}