using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.App
{
    public partial class Login : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Login1_OnAuthenticate(object sender, AuthenticateEventArgs e)
        {
            try
            {
                // NOTE - in order to make the things simple, we are not using the tokens to communicate with the backend API
                Global.GetApiClient().ValidateCredentials(Login1.UserName, Login1.Password);

                e.Authenticated = true;
                FormsAuthentication.SetAuthCookie(Login1.UserName, false);

                Response.RedirectToRoute("AdminProducts");
            }
            catch (ApiException ex) when (ex.StatusCode == 401)
            {
                e.Authenticated = false;
                return;
            }
        }
    }
}