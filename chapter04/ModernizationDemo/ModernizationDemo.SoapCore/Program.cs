using ModernizationDemo.SoapCore;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSoapCore();
builder.Services.AddScoped<Products>();
builder.Services.AddScoped<Orders>();

var app = builder.Build();

app.UseRouting();

var endpoints = app as IEndpointRouteBuilder;
endpoints.UseSoapEndpoint<Products>("/Products.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
endpoints.UseSoapEndpoint<Orders>("/Orders.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);

app.Run();
