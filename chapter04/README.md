# Sample project for Chapter 04

This solution contains examples of migration for the following technologies:

* `Soap` - ASP.NET XML Web Services to SoapCore and gRPC
* `Wcf` - WCF to CoreWCF
* `WebApi` - ASP.NET Web API to ASP.NET Core MVC
* `SignalR` - ASP.NET SignalR to ASP.NET Core SignalR

Each group of projects contains the old .NET Framework project, the new ASP.NET Core-based implementation (using the `Core` suffix), and various support projects, such as `Tests` or `Client` applications.

**All test projects require the web applications to be running.** The easiest way is to run them by right-click on the project and selecting **View > In Browser**. This will run the project in the background without the debugger, which allows you to run the tests from Visual Studio.

The ports should be set in `launchSettings.json` or in project properties which are committed in the repository, so no specific configuration is necessary. If the applications do not respond, look in the test projects which URLs they use and make sure they run on the correct ports.

The projects do not use any database - all test data are generated in memory.