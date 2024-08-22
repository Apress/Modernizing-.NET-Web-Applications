using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ModernizationDemo.App.Model;
using ModernizationDemo.BackendClient;
using Microsoft.AspNetCore.Mvc;
using ModernizationDemo.AppNew;

namespace ModernizationDemo.App.Controllers
{
    [Route("")]
    public class IndexController(ApiClient apiClient, Utils utils) : ControllerBase
    {
        private const int PageSize = 12;

        public string Currency => System.Web.HttpContext.Current.Session["Currency"] as string ?? "USD";

        [HttpGet]
        public async Task<ActionResult> Index(int page = 0)
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
                PagerModel = new PagerModel() { ActionName = "Index", PageIndex = page, PagesCount = (int)Math.Ceiling((double)products.TotalRecordCount / PageSize) }
            });
        }

        [HttpGet("login")]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // NOTE - in order to make the things simple, we are not using the tokens to communicate with the backend API
                    await apiClient.ValidateCredentialsAsync(model.UserName, model.Password);

                    var identity = new ClaimsIdentity([ new Claim(ClaimTypes.Name, model.UserName) ], CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    return Redirect("/admin/products");
                }
                catch (ApiException ex)
                {
                    ViewBag.FailureText = "Invalid username or password.";
                }
            }

            return View(model);
        }

        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        [HttpGet("product/{id?}")]
        public async Task<ActionResult> Product(Guid? id)
        {
            var product = await apiClient.GetProductAsync(id.Value);
            return View(new ProductDetailModel()
            {
                Product = product,
                ProductPrice = await utils.GetProductPriceWithCaching(product.Id, Currency)
            });
        }
    }
}