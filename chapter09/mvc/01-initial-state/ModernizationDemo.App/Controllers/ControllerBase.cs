using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ModernizationDemo.App.Model;

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
                Session["Currency"] = body.Currency;
            }

            if (!Url.IsLocalUrl(returnUrl))
            {
                return new HttpStatusCodeResult(400);
            }
            return Redirect(returnUrl);
        }
    }
}