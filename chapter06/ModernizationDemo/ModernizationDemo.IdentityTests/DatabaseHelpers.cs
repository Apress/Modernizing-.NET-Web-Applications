using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModernizationDemo.IdentityTests;

public class DatabaseHelpers
{
    public static void RecreateDatabase(string connectionString)
    {
        // connect to the master database
        var masterConnectionString = BuildMasterConnectionString(connectionString, out var testDatabaseName);
        using var tempConnection = new SqlConnection(masterConnectionString);
        tempConnection.Open();

        // drop and recreate the database
        var command = new SqlCommand(
            $"""
             IF EXISTS (SELECT 1 FROM sys.databases WHERE [name] = N'{testDatabaseName}') BEGIN
               ALTER DATABASE {testDatabaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE
               DROP DATABASE {testDatabaseName}
             END
             """, tempConnection);
        command.ExecuteNonQuery();

        tempConnection.Close();

        using var dbContext = new AppDbContext();
        dbContext.Database.CreateIfNotExists();
    }

    private static string BuildMasterConnectionString(string connectionString, out string originalDatabaseName)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        originalDatabaseName = builder.InitialCatalog;

        builder.InitialCatalog = "master";
        return builder.ConnectionString;
    }

    public static async Task SeedData()
    {
        using var userManager = new AppUserManager();
        using var roleManager = new AppRoleManager();

        await roleManager.CreateAsync(new AppRole() { Id = Guid.NewGuid(), Name = "admin" });
        await roleManager.CreateAsync(new AppRole() { Id = Guid.NewGuid(), Name = "user" });

        IdentityResult result;

        var testUser1 = new AppUser()
        {
            Id = Guid.NewGuid(),
            UserName = "testuser",
            Email = "testuser@app.com",
            EmailConfirmed = true,
            LockoutEnabled = true
        };
        result = await userManager.CreateAsync(testUser1);
        Assert.IsTrue(result.Succeeded);
        result = await userManager.AddPasswordAsync(testUser1.Id, "TestPassword1234+");
        Assert.IsTrue(result.Succeeded);

        var testUser2 = new AppUser()
        {
            Id = Guid.NewGuid(),
            UserName = "testadmin",
            Email = "testadmin@app.com",
            EmailConfirmed = true,
            LockoutEnabled = true
        };
        result = await userManager.CreateAsync(testUser2);
        Assert.IsTrue(result.Succeeded);
        result = await userManager.AddPasswordAsync(testUser2.Id, "AdminPassword1234+");
        Assert.IsTrue(result.Succeeded);

        var testUser3 = new AppUser()
        {
            Id = Guid.NewGuid(),
            UserName = "testunapproved",
            Email = "testreader@app.com",
            EmailConfirmed = true,
            LockoutEnabled = true,
            LockoutEndDateUtc = new DateTime(9999, 12, 31)
        };
        result = await userManager.CreateAsync(testUser3);
        Assert.IsTrue(result.Succeeded);
        result = await userManager.AddPasswordAsync(testUser3.Id, "UnapprovedPassword1234+");
        Assert.IsTrue(result.Succeeded);

        result = await userManager.AddToRoleAsync(testUser2.Id, "admin");
        Assert.IsTrue(result.Succeeded);
        result = await userManager.AddToRoleAsync(testUser2.Id, "user");
        Assert.IsTrue(result.Succeeded);
        result = await userManager.AddToRoleAsync(testUser1.Id, "user");
        Assert.IsTrue(result.Succeeded);
    }
}
