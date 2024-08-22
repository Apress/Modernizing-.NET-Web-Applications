using ModernizationDemo.SignalRCore.Hubs;
using ModernizationDemo.SignalRCore.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddAuthentication("ApiKey")
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKey", options =>
    {
        builder.Configuration.GetSection("Authentication").Bind(options);
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/hubs/ChatHub");
app.MapHub<OnlineUsersCounter>("/hubs/Users");

app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
