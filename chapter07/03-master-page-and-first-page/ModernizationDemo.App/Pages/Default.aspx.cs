using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.App
{
    public partial class Default : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IEnumerable<ProductModel> GetData(int? startRowIndex, int? maximumRows, out int totalRowCount)
        {
            var pageIndex = (startRowIndex / maximumRows) ?? 0;
            var products = Global.GetApiClient().GetProducts(pageIndex, maximumRows ?? 10);
            totalRowCount = products.TotalRecordCount;
            return products.Results;
        }

        protected void ProductsList_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            var productId = ((ProductModel)e.Item.DataItem).Id;
            var productPrice = Utils.GetProductPriceWithCaching(productId, SelectedCurrency);
            ((Literal)e.Item.FindControl("PriceLiteral")).Text = productPrice;
        }
    }
}