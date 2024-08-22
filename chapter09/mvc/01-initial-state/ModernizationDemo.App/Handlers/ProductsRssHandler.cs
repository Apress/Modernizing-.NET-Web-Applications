using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Web;

namespace ModernizationDemo.App.Handlers
{
    /// <summary>
    /// Summary description for ProductsRssHandler
    /// </summary>
    public class ProductsRssHandler : IHttpHandler
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/atom+xml";
            context.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
            context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(30));

            var baseUri = GetApplicationBaseUri(context);
            var feed = new SyndicationFeed
            {
                Id = baseUri.ToString(),
                Title = new TextSyndicationContent("Contoso Shop"),
                Description = new TextSyndicationContent("All products"),
                Items = GetFeedItems(baseUri)
            };
            feed.Links.Add(new SyndicationLink(baseUri));
            using (var xw = new System.Xml.XmlTextWriter(context.Response.Output))
            {
                xw.Formatting = System.Xml.Formatting.Indented;
                var ff = new Atom10FeedFormatter(feed);
                ff.WriteTo(xw);
            }
        }

        private IEnumerable<SyndicationItem> GetFeedItems(Uri baseUri)
        {
            var apiClient = Global.GetApiClient();
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

        public static Uri GetApplicationBaseUri(HttpContext context)
        {
            var uriBuilder = new UriBuilder(context.Request.Url);
            uriBuilder.Path = context.Request.ApplicationPath ?? string.Empty;
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