using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web.Profile;
using System.Web.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ModernizationDemo.UniversalTests;

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

        // hack - we need to initialize the database ourselves - the provider caches it has been done
        // they do not expect the database to be dropped and recreated within the same process
        var dbContextType = Type.GetType("System.Web.Providers.Entities.MembershipContext, System.Web.Providers", throwOnError: true);
        var dbContext = (DbContext)Activator.CreateInstance(dbContextType, "DB");
        dbContext.Database.CreateIfNotExists();
    }

    private static string BuildMasterConnectionString(string connectionString, out string originalDatabaseName)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        originalDatabaseName = builder.InitialCatalog;

        builder.InitialCatalog = "master";
        return builder.ConnectionString;
    }

    public static void SeedData()
    {
        Roles.CreateRole("admin");
        Roles.CreateRole("user");

        MembershipCreateStatus status;
        
        Membership.CreateUser("test-user", "TestPassword1234+", "testuser@app.com", "q", "a", true, out status);
        Assert.AreEqual(MembershipCreateStatus.Success, status);
        
        Membership.CreateUser("test-admin", "AdminPassword1234+", "testadmin@app.com", "q", "a", true, out status);
        Assert.AreEqual(MembershipCreateStatus.Success, status);
        
        Membership.CreateUser("test-unapproved", "UnapprovedPassword1234+", "testreader@app.com", "q", "a", false, out status);
        Assert.AreEqual(MembershipCreateStatus.Success, status);

        Roles.AddUserToRole("test-admin", "admin");
        Roles.AddUserToRole("test-admin", "user");
        Roles.AddUserToRole("test-user", "user");

        var testUserProfile = ProfileBase.Create("test-user");
        testUserProfile["FirstName"] = "First Name";
        testUserProfile["LastName"] = "Last Name";
        testUserProfile["FavoriteNumber"] = 42;
        testUserProfile["BirthDate"] = new DateTime(2000, 1, 1);
        testUserProfile["ImageUrls"] = new StringCollection()
        {
            "https://picsum.photos/200/300",
            "https://picsum.photos/id/237/200/300"
        };
        testUserProfile["ShoppingCart"] = new ShoppingCart()
        {
            Created = new DateTime(2010, 1, 3),
            LastUpdated = new DateTime(2010, 1, 4, 15, 0, 0),
            Items = new Dictionary<string, CartItem>()
            {
                { "burger", new CartItem() { Price = 15.99, Quantity = 15 } },
                { "steak", new CartItem() { Price = 42.99, Quantity = 6 } }
            }
        };
        testUserProfile.Save();
    }
}
