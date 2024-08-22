using DotVVM.Framework.Configuration;
using DotVVM.Framework.ViewModel.Validation;
using Microsoft.Extensions.DependencyInjection;
using ModernizationDemo.App.Extensions;
using ModernizationDemo.App.Handlers;

namespace ModernizationDemo.App
{
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
    {
        // For more information about this class, visit https://dotvvm.com/docs/tutorials/basics-project-structure
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            ConfigureRoutes(config, applicationPath);
            ConfigureControls(config, applicationPath);
            ConfigureResources(config, applicationPath);

            config.AddWebFormsAdapters();
        }

        private void ConfigureRoutes(DotvvmConfiguration config, string applicationPath)
        {
            // register routes   
            config.RouteTable.Add("Login", "login", "Views/Login.dothtml");

            config.RouteTable.Add("Products", "", "Views/Default.dothtml");
            config.RouteTable.Add("ProductDetail", "product/{id}", "Views/ProductDetail.dothtml");

            config.RouteTable.Add("AdminProducts", "admin/products", "Views/Admin/Products.dothtml");
            config.RouteTable.Add("AdminProductCreate", "admin/product", "Views/Admin/ProductDetail.dothtml");
            config.RouteTable.Add("AdminProductDetail", "admin/product/{id}", "Views/Admin/ProductDetail.dothtml");

            config.RouteTable.Add("ProductsRss", "products/rss", typeof(ProductsRssPresenter));
        }

        private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
        {
            config.Markup.AddMarkupControl("cc", "CustomDataPager", "Controls/CustomDataPager.dotcontrol");
            // register code-only controls and markup controls
        }

        private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
        {
            // register custom resources and adjust paths to the built-in resources
        }

        public void ConfigureServices(IDotvvmServiceCollection options)
        {
            options.AddDefaultTempStorages("temp");

            options.Services.AddSingleton<IViewModelValidationMetadataProvider, CustomViewModelValidationMetadataProvider>();
        }
    }
}
