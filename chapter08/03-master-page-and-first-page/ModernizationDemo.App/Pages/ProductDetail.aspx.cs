using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.App
{
    public partial class ProductDetail : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public ProductModel GetData()
        {
            return Global.GetApiClient().GetProduct(new Guid((string)RouteData.Values["id"]));
        }

        protected void ProductForm_OnDataBound(object sender, EventArgs e)
        {
            var productId = ((ProductModel)ProductForm.DataItem).Id;
            var productPrice = Utils.GetProductPriceWithCaching(productId, SelectedCurrency);
            ((Literal)ProductForm.FindControl("PriceLiteral")).Text = productPrice;
        }
    }
}