using System.Web.SessionState;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.SystemWebAdapters;
using ModernizationDemo.AppNew;
using ModernizationDemo.AppNew.Handlers;
using ModernizationDemo.BackendClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSystemWebAdapters()
    .AddJsonSessionSerializer(options =>
    {
        options.RegisterKey<string>("Currency");
    })
    .AddWrappedAspNetCoreSession();

builder.Services.AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options =>
        {
            options.LoginPath = "/login";
        });

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddScoped<Utils>();
builder.Services.AddMemoryCache();

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
app.UseSession();
app.UseSystemWebAdapters();

app.MapGet("/products/rss", (ProductsRssHandler handler, HttpContext context) => handler.BuildRssFeed(context));
app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/");
});

app.MapRazorPages()
    .RequireSystemWebAdapterSession(new SessionAttribute() { SessionBehavior = SessionStateBehavior.Required });

app.Run();
