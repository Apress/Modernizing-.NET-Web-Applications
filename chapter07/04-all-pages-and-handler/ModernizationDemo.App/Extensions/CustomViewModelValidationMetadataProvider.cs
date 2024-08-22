using DotVVM.Framework.ViewModel.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ModernizationDemo.App.Extensions
{
    public class CustomViewModelValidationMetadataProvider : IViewModelValidationMetadataProvider
    {
        public IEnumerable<ValidationAttribute> GetAttributesForProperty(PropertyInfo property)
        {
            if (property.DeclaringType.Name != "Metadata"
                && property.DeclaringType.GetNestedType("Metadata", BindingFlags.NonPublic) is { } metadataType
                && metadataType.GetProperty(property.Name) is { } metadataProperty)
            {
                property = metadataProperty;
            }

            return property.GetCustomAttributes<ValidationAttribute>(true);
        }
    }
}