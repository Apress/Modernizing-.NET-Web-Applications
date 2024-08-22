using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using System.ComponentModel.DataAnnotations;
using ModernizationDemo.BackendClient;
using System.Web.Security;

namespace ModernizationDemo.App.ViewModels
{
    public class LoginViewModel(ApiClient apiClient) : SiteViewModel
    {
        [Required(ErrorMessage = "User name is required!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        public string FailureText { get; set; }

        public async Task Login()
        {
            try
            {
                // NOTE - in order to make the things simple, we are not using the tokens to communicate with the backend API
                await apiClient.ValidateCredentialsAsync(UserName, Password);

                FormsAuthentication.SetAuthCookie(UserName, false);

                Context.RedirectToRouteHybrid("AdminProducts");
            }
            catch (ApiException ex) when (ex.StatusCode == 401)
            {
                FailureText = "Invalid username or password.";
            }
        }
    }
}

