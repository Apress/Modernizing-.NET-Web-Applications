using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
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
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapRoute(
                name: "Admin",
                url: "admin/{action}/{id}",
                defaults: new { controller = "Admin", id = UrlParameter.Optional }
            );
            RouteTable.Routes.MapRoute(
                name: "Index",
                url: "{action}/{id}",
                defaults: new { controller = "Index", action = "Index", id = UrlParameter.Optional }
            );

            GlobalFilters.Filters.Add(new HandleErrorAttribute());

            RouteTable.Routes.Add("ProductsRss", new Route("products/rss", new GenericRouteHandler<ProductsRssHandler>()));

            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ProductCreateEditModel), typeof(ProductCreateEditModel.Metadata)), typeof(ProductCreateEditModel));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ProductPriceModel), typeof(ProductPriceModel.Metadata)), typeof(ProductPriceModel));
        }

        public static ApiClient GetApiClient()
        {
            var httpClient = new HttpClient();
            return new ApiClient(ConfigurationManager.AppSettings["ApiBaseUrl"], httpClient);
        }
    }
}