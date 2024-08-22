using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.AppNew.Handlers
{
    public class ProductsRssHandler(ApiClient apiClient)
    {

        public async Task BuildRssFeed(HttpContext context)
        {
            context.Response.ContentType = "application/atom+xml";
            context.Response.Headers.CacheControl = $"private,max-age={TimeSpan.FromMinutes(30).TotalSeconds}";

            var baseUri = GetApplicationBaseUri(context);
            var feed = new SyndicationFeed
            {
                Id = baseUri.ToString(),
                Title = new TextSyndicationContent("Contoso Shop"),
                Description = new TextSyndicationContent("All products"),
                Items = await GetFeedItems(baseUri)
            };
            feed.Links.Add(new SyndicationLink(baseUri));

            using var ms = new MemoryStream();
            await using var xw = new System.Xml.XmlTextWriter(ms, Encoding.UTF8);
            xw.Formatting = System.Xml.Formatting.Indented;
            var ff = new Atom10FeedFormatter(feed);
            ff.WriteTo(xw);
            xw.Flush();

            ms.Position = 0;
            await ms.CopyToAsync(context.Response.Body);
        }

        private async Task<IEnumerable<SyndicationItem>> GetFeedItems(Uri baseUri)
        {
            var results = new List<SyndicationItem>();

            var products = await apiClient.GetProductsAsync(0, 1000);
            foreach (var product in products.Results)
            {
                var item = new SyndicationItem
                {
                    Id = product.Id.ToString(),
                    Title = new TextSyndicationContent(product.Name),
                    Content = new TextSyndicationContent(product.Description),
                    PublishDate = product.CreatedDate.ToUniversalTime()
                };
                item.Links.Add(new SyndicationLink(new Uri(baseUri, $"product/{product.Id}")));
                results.Add(item);
            }

            return results;
        }

        public static Uri GetApplicationBaseUri(HttpContext context)
        {
            var uriBuilder = new UriBuilder(context.Request.GetEncodedUrl());
            uriBuilder.Path = context.Request.PathBase.Value ?? string.Empty;
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