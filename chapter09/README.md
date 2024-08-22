# Sample project for Chapter 09

This solution contains an example of **side-by-side modernization of an ASP.NET MVC and ASP.NET Web Pages applications**.

Every folder shows a different stage of the modernization:

* `01-initial-state` shows the original ASP.NET Web Forms application running on .NET Framework 4.7.2.
* `02-all-pages-and-handler` shows the project after adding the new ASP.NET Core application, configuring YARP, and rewriting pages and handlers to the new syntax.
* `03-complete-migration`shows the final application migrated into ASP.NET Core.


## Running the applications

Before running the web applications, make sure the `ModernizationDemo.BackendApi` project is running (I recommend to right-click on the solution in the _Solution Explorer_ window and configure multiple startup projects - set the mode for `ModernizationDemo.App`, `ModernizationDemo.AppNew` and `ModernizationDemo.BackendApi` projects to `Start`).

**Make sure the connection string in `ModernizationDemo.BackendApi/appsettings.json` file points to a valid SQL Server instance.** The database will be created automatically if it does not exist.

## Port collisions

Make sure you do not try to run two different stages at the same time - the projects are configured to use the same ports so there may be conflicts.