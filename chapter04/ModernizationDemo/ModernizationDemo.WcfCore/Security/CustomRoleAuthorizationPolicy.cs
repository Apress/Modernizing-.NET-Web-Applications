using System.Security.Principal;
using CoreWCF.IdentityModel.Claims;
using CoreWCF.IdentityModel.Policy;

namespace ModernizationDemo.WcfCore.Security;

public class CustomRoleAuthorizationPolicy : IAuthorizationPolicy
{
    public string Id => "CustomAuthorizationPolicy";

    public ClaimSet Issuer => ClaimSet.System;

    public bool Evaluate(EvaluationContext evaluationContext, ref object state)
    {
        if (!evaluationContext.Properties.TryGetValue("Identities", out var identities)
            || identities is not List<IIdentity> typedIdentities 
            || typedIdentities is not [ GenericIdentity identity ])
        {
            return false;
        }

        var userName = identity.FindFirst(ClaimTypes.Name)!.Value;
        if (userName == "test")
        {
            evaluationContext.Properties["Principal"] = new GenericPrincipal(identity, new[] { "admin" });
        }
        else
        {
            evaluationContext.Properties["Principal"] = new GenericPrincipal(identity, new[] { "" });
        }

        return true;
    }
}