using ModernizationDemo.App.Model;
using ModernizationDemo.BackendClient;
using Microsoft.AspNetCore.Mvc;

namespace ModernizationDemo.App.Controllers
{
    [Route("")]
    public class IndexController(ApiClient apiClient) : ControllerBase
    {
        private const int PageSize = 12;

        public string Currency => System.Web.HttpContext.Current.Session["Currency"] as string ?? "USD";

        [HttpGet]
        public async Task<ActionResult> Index(int page = 0)
        {
            var products = await apiClient.GetProductsAsync(page * PageSize, PageSize);
            var productPrices = products.Results.ToDictionary(p => p.Id, p => Utils.GetProductPriceWithCaching(p.Id, Currency));

            return View(new ProductListModel()
            {
                Products = products,
                ProductPrices = productPrices,
                PagerModel = new PagerModel() { ActionName = "Index", PageIndex = page, PagesCount = (int)Math.Ceiling((double)products.TotalRecordCount / PageSize) }
            });
        }

        [HttpGet("login/new")]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost("login/new")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // NOTE - in order to make the things simple, we are not using the tokens to communicate with the backend API
                    await apiClient.ValidateCredentialsAsync(model.UserName, model.Password);

                    // TODO ASP.NET membership should be replaced with ASP.NET Core identity. For more details see https://docs.microsoft.com/aspnet/core/migration/proper-to-2x/membership-to-core-identity.
                    //FormsAuthentication.SetAuthCookie(model.UserName, false);

                    return Redirect("/admin/products");
                }
                catch (ApiException ex)
                {
                    ViewBag.FailureText = "Invalid username or password.";
                }
            }

            return View(model);
        }

        [HttpGet("logout/new")]
        public ActionResult Logout()
        {
            // TODO ASP.NET membership should be replaced with ASP.NET Core identity. For more details see https://docs.microsoft.com/aspnet/core/migration/proper-to-2x/membership-to-core-identity.
            //FormsAuthentication.SignOut();
            return Redirect("/");
        }

        [HttpGet("product/{id?}")]
        public async Task<ActionResult> Product(Guid? id)
        {
            var product = await apiClient.GetProductAsync(id.Value);
            return View(new ProductDetailModel()
            {
                Product = product,
                ProductPrice = Utils.GetProductPriceWithCaching(product.Id, Currency)
            });
        }
    }
}