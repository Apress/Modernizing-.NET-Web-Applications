using DotVVM.Framework.Routing;
using Microsoft.Owin;
using Owin;
using System.Web.Hosting;

[assembly: OwinStartup(typeof(ModernizationDemo.App.Startup))]
namespace ModernizationDemo.App
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // use DotVVM
            var applicationPhysicalPath = HostingEnvironment.ApplicationPhysicalPath;
            var dotvvmConfiguration = app.UseDotVVM<DotvvmStartup>(applicationPhysicalPath);
            dotvvmConfiguration.AssertConfigurationIsValid();
        }
    }
}
