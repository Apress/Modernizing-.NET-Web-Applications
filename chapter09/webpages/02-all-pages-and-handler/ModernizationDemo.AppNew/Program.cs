using System.Web.SessionState;
using Microsoft.AspNetCore.SystemWebAdapters;
using ModernizationDemo.AppNew.Handlers;
using ModernizationDemo.BackendClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSystemWebAdapters()
    .AddJsonSessionSerializer(options =>
    {
        options.RegisterKey<string>("Currency");
    })
    .AddRemoteAppClient(options =>
    {
        options.RemoteAppUrl = new(builder.Configuration["ProxyTo"]);
        options.ApiKey = builder.Configuration["RemoteAppApiKey"];
    })
    .AddSessionClient()
    .AddAuthenticationClient(isDefaultScheme: true);
builder.Services.AddHttpForwarder();

builder.Services.AddRazorPages();

builder.Services.AddSingleton(_ => new ApiClient(
    builder.Configuration["Api:Url"], new HttpClient()));
builder.Services.AddScoped<ProductsRssHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSystemWebAdapters();

app.MapGet("/products/rss", (ProductsRssHandler handler, HttpContext context) => handler.BuildRssFeed(context));

app.MapRazorPages()
    .RequireSystemWebAdapterSession(new SessionAttribute() { SessionBehavior = SessionStateBehavior.Required });
app.MapForwarder("/{**catch-all}", app.Configuration["ProxyTo"]).Add(static builder => ((RouteEndpointBuilder)builder).Order = int.MaxValue);

app.Run();
