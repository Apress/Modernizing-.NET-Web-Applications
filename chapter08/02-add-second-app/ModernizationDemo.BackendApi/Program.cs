using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModernizationDemo.BackendApi;
using ModernizationDemo.BackendApi.Data;
using ModernizationDemo.BackendApi.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ShopDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ShopDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// map operations
app.MapGet("/products", async (ShopDbContext dbContext, int skip = 0, int take = 20) =>
{
    return new PagedResponse<ProductModel>()
    {
        Results = await dbContext.Products
            .Select(p => new ProductModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                CreatedDate = p.CreatedDate
            })
            .OrderBy(p => p.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync(),
        TotalRecordCount = await dbContext.Products.CountAsync()
    };
}).WithName("GetProducts");

app.MapGet("/products/{id}", async (ShopDbContext dbContext, Guid id) =>
{
    return await dbContext.Products
        .Select(p => new ProductModel()
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            ImageUrl = p.ImageUrl,
            CreatedDate = p.CreatedDate
        })
        .SingleOrDefaultAsync(p => p.Id == id);
}).WithName("GetProduct");

app.MapPost("/products", async (ShopDbContext dbContext, ProductCreateEditModel model) =>
{
    var product = new Product()
    {
        Id = Guid.NewGuid(),
        Name = model.Name,
        Description = model.Description,
        ImageUrl = model.ImageUrl,
        CreatedDate = DateTime.UtcNow
    };
    dbContext.Products.Add(product);
    await dbContext.SaveChangesAsync();
    return product.Id;
}).WithName("AddProduct");

app.MapPut("/products/{id}", async (ShopDbContext dbContext, Guid id, ProductCreateEditModel model) =>
{
    var product = await dbContext.Products
        .Include(p => p.Prices)
        .SingleAsync(p => p.Id == id);

    product.Name = model.Name;
    product.Description = model.Description;
    product.ImageUrl = model.ImageUrl;

    await dbContext.SaveChangesAsync();
}).WithName("UpdateProduct");

app.MapDelete("/products/{id}", async (ShopDbContext dbContext, Guid id) =>
{
    var product = await dbContext.Products
        .SingleAsync(p => p.Id == id);
    dbContext.Products.Remove(product);
    await dbContext.SaveChangesAsync();
}).WithName("DeleteProduct");

app.MapGet("/products/{id}/prices", async (ShopDbContext dbContext, Guid id) => {
    var product = await dbContext.Products
        .Include(p => p.Prices)
        .SingleAsync(p => p.Id == id);

    return product.Prices
        .Select(p => new ProductPriceModel()
        {
            CurrencyCode = p.CurrencyCode,
            Price = p.Price
        })
        .ToList();
}).WithName("GetProductPrices");

app.MapGet("/products/{id}/price/{currency}", async (ShopDbContext dbContext, Guid id, string currency = "USD") =>
{
    var product = await dbContext.Products
        .Include(p => p.Prices)
        .SingleAsync(p => p.Id == id);

    var productPrice = product.Prices
        .FirstOrDefault(p => p.CurrencyCode == currency);

    return productPrice == null ? Results.NotFound() : Results.Ok(productPrice.Price);
})
.Produces(200, typeof(double)).Produces(404)
.WithName("GetProductPrice");

app.MapPut("/products/{id}/price/{currency}", async (ShopDbContext dbContext, Guid id, string currency, [param: FromBody] decimal newPrice) =>
{
    var product = await dbContext.Products
        .Include(p => p.Prices)
        .SingleAsync(p => p.Id == id);
    if (product.Prices.FirstOrDefault(p => p.CurrencyCode == currency) is not { } productPrice)
    {
        productPrice = new ProductPrice()
        {
            CurrencyCode = currency,
            Price = newPrice
        };
        product.Prices.Add(productPrice);
    }
    productPrice.Price = newPrice;
    await dbContext.SaveChangesAsync();
}).WithName("AddOrUpdateProductPrice");

app.MapDelete("/products/{id}/price/{currency}", async (ShopDbContext dbContext, Guid id, string currency) =>
{
    var product = await dbContext.Products
        .Include(p => p.Prices)
        .SingleAsync(p => p.Id == id);
    product.Prices.RemoveAll(p => p.CurrencyCode == currency);
    await dbContext.SaveChangesAsync();
}).WithName("DeleteProductPrice"); ;

app.MapPost("/login", async (SignInManager<IdentityUser> signInManager, string username, string password) =>
{
    var user = await signInManager.UserManager.FindByNameAsync(username);
    if (user == null)
    {
        return Results.Unauthorized();
    }

    var result = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
    if (!result.Succeeded)
    {
        return Results.Unauthorized();
    }

    return Results.Ok();
})
.WithName("ValidateCredentials")
.Produces(200)
.ProducesProblem(401);

// seed database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
    if (await dbContext.Database.EnsureCreatedAsync())
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await Seeder.SeedProducts(dbContext);
        await Seeder.SeedUsersAndRoles(userManager, roleManager);
    }
}

app.Run();
