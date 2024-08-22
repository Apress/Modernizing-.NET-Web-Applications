using System.Diagnostics;
using Bogus;
using Microsoft.AspNetCore.Identity;
using ModernizationDemo.BackendApi.Data;

namespace ModernizationDemo.BackendApi;

public class Seeder
{
    public static async Task SeedProducts(ShopDbContext context)
    {
        var otherCurrencies = new[] { "EUR", "JPY", "GBP" };
        var imageIds = Enumerable.Range(1, 200).Except([97, 105]).ToArray();

        var productsFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.ImageUrl, f => f.Image.PicsumUrl(imageId: f.PickRandom(imageIds)))
            .RuleFor(p => p.CreatedDate, f => f.Date.Between(new DateTime(2020, 1, 1), new DateTime(2024, 1, 1)))
            .RuleFor(p => p.Prices, f => 
                new[]
                {
                    new ProductPrice() { CurrencyCode = "USD", Price = f.Random.Decimal(0.1m, 10000m) }
                }
                .Concat(
                    f.PickRandom(otherCurrencies, f.Random.Int(1, otherCurrencies.Length))
                    .Select(c => new ProductPrice()
                    {
                        CurrencyCode = c,
                        Price = f.Random.Decimal(0.1m, 10000m)
                    }))
            .ToList());

        context.Products.AddRange(productsFaker.Generate(100));
        await context.SaveChangesAsync();
    }

    public static async Task SeedUsersAndRoles(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        var result = await roleManager.CreateAsync(new IdentityRole("Admin"));
        Debug.Assert(result.Succeeded);

        var adminUser = new IdentityUser()
        {
            UserName = "admin",
            Email = "admin@test.com",
            EmailConfirmed = true
        };
        result = await userManager.CreateAsync(adminUser);
        Debug.Assert(result.Succeeded);

        result = await userManager.AddPasswordAsync(adminUser, "Admin1234+");
        Debug.Assert(result.Succeeded);
    }
}