using ModernizationDemo.App.Model;
using ModernizationDemo.BackendClient;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace ModernizationDemo.App.Controllers
{
    public class IndexController : ControllerBase
    {
        private string currency;
        private const int PageSize = 12;

        protected override void Initialize(RequestContext requestContext)
        {
            currency = requestContext.HttpContext.Session["Currency"] as string ?? "USD";

            base.Initialize(requestContext);
        }

        public ActionResult Index(int page = 0)
        {
            var products = Global.GetApiClient().GetProducts(page * PageSize, PageSize);
            var productPrices = products.Results.ToDictionary(p => p.Id, p => Utils.GetProductPriceWithCaching(p.Id, currency));

            return View(new ProductListModel()
            {
                Products = products,
                ProductPrices = productPrices,
                PagerModel = new PagerModel() { ActionName = "Index", PageIndex = page, PagesCount = (int)Math.Ceiling((double)products.TotalRecordCount / PageSize) }
            });
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // NOTE - in order to make the things simple, we are not using the tokens to communicate with the backend API
                    Global.GetApiClient().ValidateCredentials(model.UserName, model.Password);

                    FormsAuthentication.SetAuthCookie(model.UserName, false);

                    return Redirect("/admin/products");
                }
                catch (ApiException ex)
                {
                    ViewBag.FailureText = "Invalid username or password.";
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        [HttpGet]
        public ActionResult Product(Guid? id)
        {
            var product = Global.GetApiClient().GetProduct(id.Value);
            return View(new ProductDetailModel()
            {
                Product = product,
                ProductPrice = Utils.GetProductPriceWithCaching(product.Id, currency)
            });
        }
    }
}