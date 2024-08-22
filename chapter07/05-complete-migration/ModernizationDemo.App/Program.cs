using DotVVM.Framework.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ModernizationDemo.App;
using ModernizationDemo.App.Handlers;
using ModernizationDemo.BackendClient;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(_ => new ApiClient(builder.Configuration["Api:Url"], new HttpClient()));
builder.Services.AddSingleton<Utils>();

builder.Services.AddScoped<ProductsRssPresenter>();

builder.Services.AddDotVVM<DotvvmStartup>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToReturnUrl = c => DotvvmAuthenticationHelper.ApplyRedirectResponse(c.HttpContext, c.RedirectUri),
            OnRedirectToAccessDenied = c => DotvvmAuthenticationHelper.ApplyStatusCodeResponse(c.HttpContext, 403),
            OnRedirectToLogin = c => DotvvmAuthenticationHelper.ApplyRedirectResponse(c.HttpContext, c.RedirectUri),
            OnRedirectToLogout = c => DotvvmAuthenticationHelper.ApplyRedirectResponse(c.HttpContext, c.RedirectUri)
        };
        options.LoginPath = "/login";
    });

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.UseDotVVM<DotvvmStartup>();
app.UseStaticFiles();

app.Run();
