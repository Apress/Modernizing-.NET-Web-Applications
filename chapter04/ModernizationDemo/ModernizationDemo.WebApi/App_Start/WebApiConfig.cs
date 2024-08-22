using Newtonsoft.Json.Converters;
using System.Net.Http.Formatting;
using System.Web.Http;
using ModernizationDemo.WebApi.Security;

namespace ModernizationDemo.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // remove support for alternative formats (www-form-urlencoded) which break NSwag generation
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            // use string representation of enums
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            
            // add API key authentication filter
            config.Filters.Add(new ApiKeyAuthenticationFilter());

            // map API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
