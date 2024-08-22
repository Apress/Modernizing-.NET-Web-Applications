using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModernizationDemo.App.Model;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.App.Controllers
{
    [Authorize]
    public class AdminController : ControllerBase
    {
        private const int PageSize = 10;

        public ActionResult Products(int page = 0)
        {
            var products = Global.GetApiClient().GetProducts(page * PageSize, PageSize);
            var productPrices = products.Results.ToDictionary(p => p.Id, p => Utils.GetProductPriceWithCaching(p.Id, Session["Currency"] as string ?? "USD"));

            return View(new ProductListModel()
            {
                Products = products,
                ProductPrices = productPrices,
                PagerModel = new PagerModel() { ActionName = "Products", PageIndex = page, PagesCount = (int)Math.Ceiling((double)products.TotalRecordCount / PageSize) }
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProduct(Guid id)
        {
            Global.GetApiClient().DeleteProduct(id);
            return RedirectToAction("Products");
        }

        [HttpGet]
        public ActionResult Product(Guid? id)
        {
            if (id == null)
            {
                return View(new ProductCreateEditModel());
            }
            else
            {
                var product = Global.GetApiClient().GetProduct(id.Value);
                return View(new ProductCreateEditModel()
                {
                    Name = product.Name,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Product(ProductCreateEditModel body, Guid? id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    Global.GetApiClient().AddProduct(body);
                }
                else
                {
                    Global.GetApiClient().UpdateProduct(id.Value, body);
                }

                return RedirectToAction("Products");
            }

            return View(body);
        }
    }
}