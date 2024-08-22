using System;
using System.Collections.Generic;
using System.Linq;
using ModernizationDemo.App.Model;
using Microsoft.AspNetCore.Mvc;

namespace ModernizationDemo.App.Controllers
{
    public class ControllerBase : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SwitchLanguage(SwitchLanguageModel body, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                System.Web.HttpContext.Current.Session["Currency"] = body.Currency;
            }

            if (!Url.IsLocalUrl(returnUrl))
            {
                return new StatusCodeResult((int) 400);
            }
            return Redirect(returnUrl);
        }
    }
}