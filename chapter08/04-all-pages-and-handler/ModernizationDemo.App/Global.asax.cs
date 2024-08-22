﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Hosting;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using ModernizationDemo.App.Handlers;
using ModernizationDemo.App.Migration;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.App
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapPageRoute("Login", "login", "~/Pages/Login.aspx");
            RouteTable.Routes.MapPageRoute("Logout", "logout", "~/Pages/Logout.aspx");

            RouteTable.Routes.MapPageRoute("Products", "", "~/Pages/Default.aspx");
            RouteTable.Routes.MapPageRoute("ProductDetail", "product/{id}", "~/Pages/ProductDetail.aspx");

            RouteTable.Routes.MapPageRoute("AdminProducts", "admin/products", "~/Pages/Admin/Products.aspx");
            RouteTable.Routes.MapPageRoute("AdminProductCreate", "admin/product", "~/Pages/Admin/ProductDetail.aspx");
            RouteTable.Routes.MapPageRoute("AdminProductDetail", "admin/product/{id}", "~/Pages/Admin/ProductDetail.aspx");

            RouteTable.Routes.Add("ProductsRss", new Route("products/rss", new GenericRouteHandler<ProductsRssHandler>()));

            SystemWebAdapterConfiguration.AddSystemWebAdapters(this)
                .AddProxySupport(options => options.UseForwardedHeaders = true)
                .AddJsonSessionSerializer(options =>
                {
                    options.RegisterKey<string>("SelectedCurrency");
                })
                .AddRemoteAppServer(options => options.ApiKey = ConfigurationManager.AppSettings["RemoteAppApiKey"])
                .AddSessionServer()
                .AddAuthenticationServer();

            CacheInvalidationClient.StartWorker();
        }

        protected void Application_End(object sender, EventArgs e)
        {
            CacheInvalidationClient.StopWorker();
        }

        public static ApiClient GetApiClient()
        {
            var httpClient = new HttpClient();
            return new ApiClient(ConfigurationManager.AppSettings["ApiBaseUrl"], httpClient);
        }
    }
}