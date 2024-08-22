using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.App.Pages.Admin
{
    public partial class ProductDetail : PageBase
    {

        public Guid? ProductId => RouteData.Values.TryGetValue("Id", out var id) ? new Guid(id.ToString()) : (Guid?)null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ProductId == null)
                {
                    ProductForm.ChangeMode(FormViewMode.Insert);
                }
                else
                {
                    ProductForm.ChangeMode(FormViewMode.Edit);
                }
            }
        }

        public ProductCreateEditModel GetData()
        {
            if (ProductId == null)
            {
                return new ProductCreateEditModel();
            }
            else
            {
                var product = Global.GetApiClient().GetProduct(ProductId.Value);
                return new ProductCreateEditModel
                {
                    Name = product.Name,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl
                };
            }
        }

        public void InsertData()
        {
            var product = new ProductCreateEditModel();
            if (!TryUpdateModel(product))
            {
                return;
            }

            var productId = Global.GetApiClient().AddProduct(product);
            Response.RedirectToRoute("AdminProductDetail", new { Id = productId });
        }

        public ICollection<ProductPriceModel> GetPrices()
        {
            return Global.GetApiClient().GetProductPrices(ProductId.Value);
        }

        public ProductPriceModel GetPrice()
        {
            return new ProductPriceModel() { CurrencyCode = "USD" };
        }

        public void InsertPrice()
        {
            var price = new ProductPriceModel();
            if (!TryUpdateModel(price))
            {
                return;
            }

            Global.GetApiClient().AddOrUpdateProductPrice(ProductId.Value, price.CurrencyCode, price.Price);
            Utils.ResetProductPriceWithCache(ProductId.Value, price.CurrencyCode);

            ((GridView)ProductForm.FindControl("PricesGrid")).DataBind();
        }

        protected void InsertCurrencyDropDown_ServerValidate(object source, ServerValidateEventArgs args)
        {
            var existingPrices = Global.GetApiClient().GetProductPrices(ProductId.Value);
            args.IsValid = !existingPrices.Any(p => p.CurrencyCode == args.Value);
        }

        public void UpdatePrice(string currencyCode)
        {
            var price = new ProductPriceModel();
            if (!TryUpdateModel(price))
            {
                return;
            }

            Global.GetApiClient().AddOrUpdateProductPrice(ProductId.Value, currencyCode, price.Price);
            Utils.ResetProductPriceWithCache(ProductId.Value, currencyCode);
        }

        public void DeletePrice(string currencyCode)
        {
            Global.GetApiClient().DeleteProductPrice(ProductId.Value, currencyCode);
            Utils.ResetProductPriceWithCache(ProductId.Value, currencyCode);
        }

        public void UpdateData()
        {
            var product = new ProductCreateEditModel();
            if (!TryUpdateModel(product))
            {
                return;
            }

            Global.GetApiClient().UpdateProduct(ProductId.Value, product);
            Response.RedirectToRoute("AdminProducts");
        }
    }
}