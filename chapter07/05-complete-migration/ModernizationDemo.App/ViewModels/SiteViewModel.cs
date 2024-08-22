using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ModernizationDemo.App.ViewModels
{
    public class SiteViewModel : DotvvmViewModelBase
    {
        [Bind(Direction.ServerToClientFirstRequest)]
        public List<string> Currencies { get; set; } = ["USD", "EUR", "JPY", "GBP"];

        public string SelectedCurrency { get; set; } = "USD";

        public override async Task Init()
        {
            var session = Context.GetAspNetCoreContext().Session;
            await session.LoadAsync();

            if (session.TryGetValue("SelectedCurrency", out var currency))
            {
                SelectedCurrency = Encoding.ASCII.GetString(currency);
            }
            await base.Init();
        }

        public async Task OnCurrencyChanged()
        {
            var session = Context.GetAspNetCoreContext().Session;
            await session.LoadAsync();

            session.Set("SelectedCurrency", Encoding.ASCII.GetBytes(SelectedCurrency));
            await session.CommitAsync();

            Context.RedirectToLocalUrl(
                Context.HttpContext.Request.Url.PathAndQuery);
        }

        public async Task SignOut()
        {
            await Context.GetAuthentication().SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Context.RedirectToRouteHybrid("Products");
        }

    }
}

