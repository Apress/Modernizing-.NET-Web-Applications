using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using System.Web.Security;
using System.Web;

namespace ModernizationDemo.App.ViewModels
{
    public class SiteViewModel : DotvvmViewModelBase
    {
        [Bind(Direction.ServerToClientFirstRequest)]
        public List<string> Currencies { get; set; } = ["USD", "EUR", "JPY", "GBP"];

        public string SelectedCurrency { get; set; } = "USD";

        public override Task Init()
        {
            if (HttpContext.Current.Session["SelectedCurrency"] is string currency)
            {
                SelectedCurrency = currency;
            }
            return base.Init();
        }

        public void OnCurrencyChanged()
        {
            HttpContext.Current.Session["SelectedCurrency"] = SelectedCurrency;
            Context.RedirectToLocalUrl(
                Context.HttpContext.Request.Url.PathAndQuery);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
            Context.RedirectToRouteHybrid("Products");
        }

    }
}

