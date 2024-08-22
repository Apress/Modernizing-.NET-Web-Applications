using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.App.Admin
{
    public partial class Products : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public IEnumerable<ProductModel> GetData(int? startRowIndex, int? maximumRows, out int totalRowCount)
        {
            var products = Global.GetApiClient().GetProducts(startRowIndex, maximumRows ?? 10);
            totalRowCount = products.TotalRecordCount;
            return products.Results;
        }

        protected void ProductsGrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var productId = ((ProductModel)e.Row.DataItem).Id;

                if (e.Row.DataItemIndex == ProductsGrid.EditIndex)
                {
                    double? price = null;
                    try
                    {
                        price = Global.GetApiClient().GetProductPrice(productId, SelectedCurrency);
                    }
                    catch (ApiException ex) when (ex.StatusCode == 404)
                    {
                    }
                    ((TextBox)e.Row.FindControl("PriceTextBox")).Text = price?.ToString("n2");
                }
                else
                {
                    var productPrice = Utils.GetProductPriceWithCaching(productId, SelectedCurrency);
                    ((Literal)e.Row.FindControl("PriceLiteral")).Text = productPrice;
                }
            }
        }

        public void DeleteProduct(Guid id)
        {
            Global.GetApiClient().DeleteProduct(id);
        }
    }
}