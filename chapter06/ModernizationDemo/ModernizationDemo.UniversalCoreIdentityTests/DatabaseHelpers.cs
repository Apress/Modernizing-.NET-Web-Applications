using Microsoft.Data.SqlClient;

namespace ModernizationDemo.UniversalCoreIdentityTests;

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
}