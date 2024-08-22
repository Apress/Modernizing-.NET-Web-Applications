using System.Security;
using CoreWCF.IdentityModel.Selectors;

namespace ModernizationDemo.WcfCore.Security;

public class CustomUserNamePasswordValidator : UserNamePasswordValidator
{
    public override async ValueTask ValidateAsync(string userName, string password)
    {
        if (userName == "test" && password == "test-password")
        {
            return;
        }
        else
        {
            throw new SecurityException();
        }
    }
}