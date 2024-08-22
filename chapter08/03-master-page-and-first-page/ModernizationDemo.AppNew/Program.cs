using System.Net;
using ModernizationDemo.AppNew.Components;
using ModernizationDemo.AppNew.Migration;
using ModernizationDemo.AppNew.Services;
using ModernizationDemo.BackendClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSystemWebAdapters()
    .AddJsonSessionSerializer(options =>
    {
        options.RegisterKey<string>("SelectedCurrency");
    })
    .AddRemoteAppClient(options =>
    {
        options.RemoteAppUrl = new(builder.Configuration["ProxyTo"]);
        options.ApiKey = builder.Configuration["RemoteAppApiKey"];
    })
    .AddSessionClient()
    .AddAuthenticationClient(isDefaultScheme: true);
builder.Services.AddHttpForwarder();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<SelectedCurrencyService>();

builder.Services.AddSingleton(_ => new ApiClient(
    builder.Configuration["Api:Url"], new HttpClient()));

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

app.UseSystemWebAdapters();
app.LoadSessionForBlazorServer();

app.MapHub<CacheInvalidationHub>("_migration/cache-invalidate")
    .ShortCircuit();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapForwarder("/{**catch-all}", app.Configuration["ProxyTo"]).Add(static builder => ((RouteEndpointBuilder)builder).Order = int.MaxValue);

app.Run();