using System.Data;

namespace ModernizationDemo.AdoNetTests;

public class DatabaseHelpers
{
    public static IReadOnlyList<ProductEntry> Products => new List<ProductEntry>
    {
        new() { ProductName = "Apple", Price = 1.2m },
        new() { ProductName = "Banana", Price = 1.2m },
        new() { ProductName = "Cherry", Price = 1.5m },
        new() { ProductName = "Date", Price = 1.8m },
        new() { ProductName = "Elderberry", Price = 2.1m },
        new() { ProductName = "Fig", Price = 2.4m },
        new() { ProductName = "Grape", Price = 2.7m },
        new() { ProductName = "Honeydew", Price = 3.0m },
        new() { ProductName = "Ice Cream", Price = 3.3m }
    };

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
             CREATE DATABASE {testDatabaseName}
             """, tempConnection);
        command.ExecuteNonQuery();

        tempConnection.Close();
    }

    private static string BuildMasterConnectionString(string connectionString, out string originalDatabaseName)
    {
        var builder = new SqlConnectionStringBuilder(connectionString);
        originalDatabaseName = builder.InitialCatalog;

        builder.InitialCatalog = "master";
        return builder.ConnectionString;
    }

    public static void SeedData(IDbConnection connection)
    {
        using var createTableCommand = connection.CreateCommand();
        createTableCommand.CommandText =
            $"""
             CREATE TABLE Products (
               Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
               ProductName NVARCHAR(40) NOT NULL,
               Price DECIMAL(18,2) NOT NULL
             )
             """;
        createTableCommand.ExecuteNonQuery();

        foreach (var product in Products)
        {
            var insertCommand = connection.CreateCommand();
            insertCommand.CommandText =
                """
                INSERT INTO Products (ProductName, Price) 
                  VALUES (@ProductName, @Price)
                """;

            var productNameParameter = insertCommand.CreateParameter();
            productNameParameter.ParameterName = "ProductName";
            productNameParameter.Value = product.ProductName;
            insertCommand.Parameters.Add(productNameParameter);

            var priceParameter = insertCommand.CreateParameter();
            priceParameter.ParameterName = "Price";
            priceParameter.Value = product.Price;
            insertCommand.Parameters.Add(priceParameter);

            insertCommand.ExecuteNonQuery();
        }
    }
}

public class ProductEntry
{
    public string ProductName { get; set; }

    public decimal Price { get; set; }
}