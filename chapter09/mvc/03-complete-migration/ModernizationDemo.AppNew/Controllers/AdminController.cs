using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModernizationDemo.App.Model;
using ModernizationDemo.BackendClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ModernizationDemo.AppNew;

namespace ModernizationDemo.App.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AdminController(ApiClient apiClient, Utils utils) : ControllerBase
    {
        private const int PageSize = 10;

        public string Currency => System.Web.HttpContext.Current.Session["Currency"] as string ?? "USD";

        [HttpGet("products")]
        public async Task<ActionResult> Products(int page = 0)
        {
            var products = await apiClient.GetProductsAsync(page * PageSize, PageSize);
            var productPrices = new Dictionary<Guid, string>();
            foreach (var product in products.Results)
            {
                var price = await utils.GetProductPriceWithCaching(product.Id, Currency);
                productPrices[product.Id] = price;
            }

            return View(new ProductListModel()
            {
                Products = products,
                ProductPrices = productPrices,
                PagerModel = new PagerModel() { ActionName = "Products", PageIndex = page, PagesCount = (int)Math.Ceiling((double)products.TotalRecordCount / PageSize) }
            });
        }

        [HttpPost("products/delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            await apiClient.DeleteProductAsync(id);
            return RedirectToAction("Products");
        }

        [HttpGet("product/{id?}")]
        public async Task<ActionResult> Product(Guid? id)
        {
            if (id == null)
            {
                return View(new AdminProductDetailModel());
            }
            else
            {
                var product = await apiClient.GetProductAsync(id.Value);
                return View(new AdminProductDetailModel()
                {
                    Name = product.Name,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl
                });
            }
        }

        [HttpPost("product/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Product(AdminProductDetailModel body, Guid? id)
        {
            if (ModelState.IsValid)
            {
                var data = new ProductCreateEditModel()
                {
                    Name = body.Name,
                    Description = body.Description,
                    ImageUrl = body.ImageUrl
                };

                if (id == null)
                {
                    await apiClient.AddProductAsync(data);
                }
                else
                {
                    await apiClient.UpdateProductAsync(id.Value, data);
                }

                return RedirectToAction("Products");
            }

            return View(body);
        }
    }
}