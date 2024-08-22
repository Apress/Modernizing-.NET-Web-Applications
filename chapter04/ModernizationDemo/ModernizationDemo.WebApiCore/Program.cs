using ModernizationDemo.WebApiCore.Filters;
using ModernizationDemo.WebApiCore.Security;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Formatters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
    {
        options.OutputFormatters.RemoveType<StringOutputFormatter>();
        options.Filters.Add(typeof(FixNoContentResponseOnVoidActionsFilter));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}_{e.ActionDescriptor.RouteValues["action"]}");
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Name = "X-Api-Key",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKey"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    }
                },
                new string[] { }
            }
        }
    );
});

builder.Services.AddAuthentication("ApiKey")
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>("ApiKey", options =>
    {
        builder.Configuration.GetSection("Authentication").Bind(options);
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();

app.Run();