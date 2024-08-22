# Sample project for Chapter 06

This solution contains examples of migration for the following technologies:

* `Membership` - ASP.NET Membership Providers to ASP.NET Core Identity
* `UniversalProviders` - ASP.NET Universal Providers to ASP.NET Core Identity
* `AspNetIdentity` - ASP.NET Identity to ASP.NET Core Identity

All samples are implemented as unit tests. 

Before running the tests, **make sure the connection strings (either in the `*Tests.cs` or in `app.config`) point to a valid instance of SQL Server**.

**Make sure you run the .NET Framework project in each category first to create the database with the legacy formats. The new .NET project will migrate the data into a new structure on startup.**

**CAUTION: The tests drop and recreate the database every time they are run. Make sure the database name does not conflict with any existing database.**