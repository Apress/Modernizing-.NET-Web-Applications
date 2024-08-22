using System.Web;
using System.Web.Routing;

namespace ModernizationDemo.App
{
    public class GenericRouteHandler<T> : IRouteHandler
        where T : IHttpHandler, new()
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new T();
        }
    }
}