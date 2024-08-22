using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ModernizationDemo.BackendClient
{
    public partial class ProductCreateEditModel
    {
        public class Metadata
        {
            [Required(ErrorMessage = "Product name is required!")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Product description is required!")]
            public string Description { get; set; }

            [Required(ErrorMessage = "Product image is required!")]
            public string ImageUrl { get; set; }
        }
    }
}
