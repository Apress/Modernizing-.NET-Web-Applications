using DotVVM.Framework.Hosting;
using ModernizationDemo.BackendClient;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ModernizationDemo.App.Handlers
{
    public class ProductsRssPresenter(ApiClient apiClient) : IDotvvmPresenter
    {
        public async Task ProcessRequest(IDotvvmRequestContext context)
        {
            context.HttpContext.Response.ContentType = "application/atom+xml";
            context.HttpContext.Response.Headers["Cache-Control"] = "private";
            context.HttpContext.Response.Headers["Expires"] = DateTime.UtcNow.AddMonths(1).ToString("R");

            var baseUri = GetApplicationBaseUri(context.HttpContext);
            var feed = new SyndicationFeed
            {
                Id = baseUri.ToString(),
                Title = new TextSyndicationContent("Contoso Shop"),
                Description = new TextSyndicationContent("All products"),
                Items = GetFeedItems(baseUri)
            };
            feed.Links.Add(new SyndicationLink(baseUri));
            using (var xw = new System.Xml.XmlTextWriter(context.HttpContext.Response.Body, Encoding.UTF8))
            {
                xw.Formatting = System.Xml.Formatting.Indented;
                var ff = new Atom10FeedFormatter(feed);
                ff.WriteTo(xw);
            }
        }

        private IEnumerable<SyndicationItem> GetFeedItems(Uri baseUri)
        {
            var products = apiClient.GetProducts(0, 1000).Results;
            foreach (var product in products)
            {
                var item = new SyndicationItem
                {
                    Id = product.Id.ToString(),
                    Title = new TextSyndicationContent(product.Name),
                    Content = new TextSyndicationContent(product.Description),
                    PublishDate = product.CreatedDate.ToUniversalTime()
                };
                item.Links.Add(new SyndicationLink(new Uri(baseUri, $"product/{product.Id}")));
                yield return item;
            }
        }

        public static Uri GetApplicationBaseUri(IHttpContext context)
        {
            var uriBuilder = new UriBuilder(context.Request.Url);
            uriBuilder.Path = context.Request.PathBase.Value;
            uriBuilder.Query = string.Empty;
            uriBuilder.Fragment = string.Empty;
            if (!uriBuilder.Path.EndsWith("/"))
            {
                uriBuilder.Path += "/";
            }
            return uriBuilder.Uri;
        }

    }
}