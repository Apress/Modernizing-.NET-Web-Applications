# Sample project for Chapter 05

This solution contains examples of migration for the following technologies:

* `AdoNet` - ADO.NET migration to the new .NET
  * The `AdoNetTests` project shows the same code compiled for both framework versions, including different NuGet references and namespace imports.

* `LinqToSql` - LINQ to SQL migration to Entity Framework Core
  * `LinqToSqlTests` project uses LINQ to SQL with .NET Framework
  * `LinqToSqlToEfCoreTests` project shows a migrated model to EF Core with the newest .NET

* `EF` - Entity Framework 4 - 6 migration to Entity Framework Core
  * `EfLegacyTests` project shows the usage of Database First approach using the old EF 4 ObjectContext API on .NET Framework
  * `EfTests` project shows the usage of Database First approach using EF 6 DbContext API on .NET Framework
  * `EfCoreEdmxTests` project shows the migration of Database First approach with using EF 6 on the new .NET
  * `EfCodeFirstTests` project shows the usage of Code First approach using EF 6 DbContext API on .NET Framework
  * `EfCoreTests` project shows the migration to EF Core on the new .NET

All samples are implemented as unit tests. 

Before running the tests, **make sure the connection strings (either in the `*Tests.cs` or in `app.config`) point to a valid instance of SQL Server**.

**CAUTION: The tests drop and recreate the database every time they are run. Make sure the database name does not conflict with any existing database.**