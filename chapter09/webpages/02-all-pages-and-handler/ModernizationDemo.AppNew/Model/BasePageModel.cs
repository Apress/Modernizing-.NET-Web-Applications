using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ModernizationDemo.AppNew.Model;

public class BasePageModel : PageModel
{
    [BindProperty]
    public string? Currency { get; set; }

    public virtual Task OnGetAsync()
    {
        Currency = System.Web.HttpContext.Current!.Session["Currency"] as string ?? "USD";
        return Task.CompletedTask;
    }

    public IActionResult OnPostSwitchCurrency()
    {
        System.Web.HttpContext.Current!.Session["Currency"] = Currency;
        return Redirect(Request.Path);
    }

}