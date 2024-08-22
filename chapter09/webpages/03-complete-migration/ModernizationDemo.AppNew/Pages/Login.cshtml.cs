using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModernizationDemo.AppNew.Model;
using ModernizationDemo.BackendClient;

namespace ModernizationDemo.AppNew.Pages
{
    public class LoginModel(ApiClient apiClient) : BasePageModel
    {
        public string FailureText { get; set; }
        
        [BindProperty]
        [Required(ErrorMessage = "User name is required.")]
        public string UserName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // NOTE - in order to make the things simple, we are not using the tokens to communicate with the backend API
                    await apiClient.ValidateCredentialsAsync(Request.Form["UserName"], Request.Form["Password"]);

                    var identity = new ClaimsIdentity([ new Claim(ClaimTypes.Name, UserName) ], CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    return Redirect("/admin/products");
                }
                catch (ApiException ex)
                {
                    FailureText = "Invalid username or password.";
                }
            }

            return Page();
        }
    }
}
