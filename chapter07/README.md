# Sample project for Chapter 07

This solution contains an example of **in-place modernization of an ASP.NET Web Forms application**.

Every folder shows a different stage of the modernization:

* `01-initial-state` shows the original ASP.NET Web Forms application running on .NET Framework 4.7.2.
* `02-add-dotvvm` shows the project after adding DotVVM.
* `03-master-page-and-first-page` shows the project after migrating the master page and one of the application pages.
* `04-all-pages-and-handler` shows the project after all DotVVM pages and handlers have been migrated.
* `05-complete-migration` shows the project switched to the new .NET.

## Running the application

Before running the web application, make sure the `ModernizationDemo.BackendApi` project is running (I recommend to run it by right-clicking on the project in the _Solution Explorer_ window and choosing **View > In Browser**). 

**Make sure the connection string in `ModernizationDemo.BackendApi/appsettings.json` file points to a valid SQL Server instance.** The database will be created automatically if it does not exist.

## Port collisions

Make sure you do not try to run two different stages at the same time - the projects are configured to use the same ports so there may be conflicts.