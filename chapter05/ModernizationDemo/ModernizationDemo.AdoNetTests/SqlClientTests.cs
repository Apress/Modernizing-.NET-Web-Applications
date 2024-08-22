using System.Data;

namespace ModernizationDemo.AdoNetTests;

public class SqlClientTests : IDisposable
{
    private readonly SqlConnection connection;

    public SqlClientTests()
    {
        var connectionString = "Data Source=.\\SQLEXPRESS; Initial Catalog=test; Integrated Security=true; Encrypt=false";
        
        DatabaseHelpers.RecreateDatabase(connectionString);
        
        connection = new SqlConnection(connectionString);
        connection.Open();
        DatabaseHelpers.SeedData(connection);
    }

    [Fact]
    public void ScalarQuery()
    {
        using var command = new SqlCommand("SELECT COUNT(*) FROM Products", connection);
        Assert.Equal(DatabaseHelpers.Products.Count, command.ExecuteScalar());
    }

    [Fact]
    public void DataReaderQuery()
    {
        using var command = new SqlCommand("SELECT * FROM Products ORDER BY Id", connection);
        using var reader = command.ExecuteReader();

        for (var i = 0; i < DatabaseHelpers.Products.Count; i++)
        {
            var product = DatabaseHelpers.Products[i];

            Assert.True(reader.Read());
            Assert.Equal(i + 1, reader["Id"]);
            Assert.Equal(product.ProductName, reader["ProductName"]);
            Assert.Equal(product.Price, reader["Price"]);
        }

        Assert.False(reader.Read());
    }

    [Fact]
    public void DataTableTest()
    {
        // load data into a DataTable
        using var adapter = new SqlDataAdapter()
        {
            SelectCommand = new SqlCommand("SELECT Id, ProductName, Price FROM Products ORDER BY Id", connection),
            InsertCommand = new SqlCommand("INSERT INTO Products (ProductName, Price) VALUES (@ProductName, @Price); SELECT SCOPE_IDENTITY()", connection),
            UpdateCommand = new SqlCommand("UPDATE Products SET ProductName = @ProductName, Price = @Price WHERE Id = @Id", connection),
            DeleteCommand = new SqlCommand("DELETE FROM Products WHERE Id = @Id", connection)
        };
        adapter.InsertCommand.Parameters.Add(new SqlParameter("@ProductName", SqlDbType.NVarChar) { SourceColumn = "ProductName" });
        adapter.InsertCommand.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { SourceColumn = "Price" });
        adapter.UpdateCommand.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { SourceColumn = "Id" });
        adapter.UpdateCommand.Parameters.Add(new SqlParameter("@ProductName", SqlDbType.NVarChar) { SourceColumn = "ProductName" });
        adapter.UpdateCommand.Parameters.Add(new SqlParameter("@Price", SqlDbType.Decimal) { SourceColumn = "Price" });
        adapter.DeleteCommand.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { SourceColumn = "Id" });

        var table = new DataTable();
        adapter.Fill(table);

        // verify loaded data
        Assert.Equal(DatabaseHelpers.Products.Count, table.Rows.Count);
        for (var i = 0; i < DatabaseHelpers.Products.Count; i++)
        {
            var product = DatabaseHelpers.Products[i];
            var row = table.Rows[i];

            Assert.Equal(i + 1, row["Id"]);
            Assert.Equal(product.ProductName, row["ProductName"]);
            Assert.Equal(product.Price, row["Price"]);
        }

        // modify the DataTable (insert, update, delete)
        table.Rows.Add(0, "New Product", 123.45m);
        table.Rows.Add(0, "New Product 2", 66.6m);

        table.Rows[0]["ProductName"] = "Modified Product";
        table.Rows[0]["Price"] = 99.99m;

        table.Rows[2].Delete();

        // save changes back to the database
        Assert.Equal(4, adapter.Update(table));
        
        // reload table
        table.Reset();
        adapter.Fill(table);

        Assert.Equal(10, table.Rows.Count);
        Assert.Equal("Modified Product", table.Rows[0]["ProductName"]);
        Assert.Equal(99.99m, table.Rows[0]["Price"]);
        Assert.Equal("Date", table.Rows[2]["ProductName"]);
        Assert.Equal("New Product", table.Rows[8]["ProductName"]);
        Assert.Equal(123.45m, table.Rows[8]["Price"]);
        Assert.Equal("New Product 2", table.Rows[9]["ProductName"]);
        Assert.Equal(66.6m, table.Rows[9]["Price"]);
    }

    public void Dispose()
    {
        connection?.Dispose();
    }
}