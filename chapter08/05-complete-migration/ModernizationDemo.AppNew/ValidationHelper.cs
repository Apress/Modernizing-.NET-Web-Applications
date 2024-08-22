using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.AppNew
{
    public class ValidationHelper
    {

        public static void ConfigureMetadataTypes()
        {
            // Blazor does not support the MetadataType attribute, but the TypeDescriptor API can be used to achieve the same effect
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ProductCreateEditModel), typeof(ProductCreateEditModel.Metadata)), typeof(ProductCreateEditModel));
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(ProductPriceModel), typeof(ProductPriceModel.Metadata)), typeof(ProductPriceModel));
        }

    }
}
