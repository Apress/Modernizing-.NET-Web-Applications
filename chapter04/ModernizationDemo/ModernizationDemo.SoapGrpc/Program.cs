using ModernizationDemo.SoapGrpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<ProductsImplementation>();
app.MapGrpcService<OrdersImplementation>();

app.Run();
