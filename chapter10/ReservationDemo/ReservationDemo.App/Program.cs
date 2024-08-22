using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReservationDemo.App;
using ReservationDemo.App.Services;
using ReservationDemo.Domain.Repositories;
using ReservationDemo.Domain.Services;
using ReservationDemo.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDotVVM<DotvvmStartup>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});

builder.Services.AddScoped<ReservationPolicyService>();
builder.Services.AddScoped<ReservationService>();

builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<ReservationDayRepository>();

builder.Services.AddScoped<ICurrentCustomerProvider, CurrentCustomerProvider>();

var app = builder.Build();

app.UseDotVVM<DotvvmStartup>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run();