using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ModernizationDemo.AppNew;
using ModernizationDemo.AppNew.Components;
using ModernizationDemo.AppNew.Handlers;
using ModernizationDemo.AppNew.Services;
using ModernizationDemo.BackendClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/login";
    });

ValidationHelper.ConfigureMetadataTypes();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<SelectedCurrencyService>();

builder.Services.AddSingleton(_ => new ApiClient(
    builder.Configuration["Api:Url"], new HttpClient()));

builder.Services.AddScoped<ProductsRssHandler>();
builder.Services.AddScoped<Utils>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapGet("/products/rss", (ProductsRssHandler handler, HttpContext context) => handler.BuildRssFeed(context));
app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/");
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();