using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using ModernizationDemo.BusinessLogic.Models;

namespace ModernizationDemo.BusinessLogic.Services;

public class Data
{
    public static List<ProductModel> Products { get; }
    public static List<OrderModel> Orders { get; }

    static Data()
    {
        var productFaker = new Faker<ProductModel>()
            .UseSeed(123)
            .RuleFor(p => p.Id, r => r.IndexFaker)
            .RuleFor(p => p.Title, r => r.Commerce.Product())
            .RuleFor(p => p.Description, r => r.Commerce.ProductDescription())
            .RuleFor(p => p.ImageUrl, r => r.Image.PicsumUrl())
            .RuleFor(p => p.IsOnSale, r => r.Random.Bool(0.8f))
            .RuleFor(p => p.UnitPrice, r => Math.Round(r.Random.Decimal(1, 5000), 2))
            .RuleFor(p => p.Unit, r => r.PickRandom("pcs", "pkg", "kg"));
        Products = productFaker.Generate(100);

        var orderItemFaker = new Faker<OrderItemModel>()
            .UseSeed(123)
            .RuleFor(p => p.ProductId, r => r.PickRandom(Products).Id)
            .RuleFor(p => p.Quantity, r => r.Random.Int(1, 300))
            .RuleFor(p => p.UnitPrice, (r, p) => Products.Single(x => x.Id == p.ProductId).UnitPrice);
        var orderFaker = new Faker<OrderModel>()
            .UseSeed(123)
            .RuleFor(p => p.Id, r => r.IndexFaker)
            .RuleFor(p => p.CreatedAt, r => r.Date.Past(10, new DateTime(2024, 1, 1)).ToUniversalTime())
            .RuleFor(p => p.CompletedAt, (r, o) => r.Date.Soon(10, o.CreatedAt).OrNull(r, 0.3f))
            .RuleFor(p => p.CanceledAt, (r, o) => o.CompletedAt != null ? r.Date.Soon(10, o.CreatedAt).OrNull(r, 0.7f) : null)
            .RuleFor(p => p.OrderItems, r => orderItemFaker.Generate(r.Random.Int(1, 10)))
            .FinishWith((r, o) => o.CalculateProperties());
        Orders = orderFaker.Generate(1000);
    }
}