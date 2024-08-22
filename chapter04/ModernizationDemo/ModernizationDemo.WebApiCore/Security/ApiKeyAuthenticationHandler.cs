using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ModernizationDemo.WebApiCore.Security
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
    {
        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // if we have the X-Api-Key header, validate it and set the current context identity
            if (Request.Headers.TryGetValue("X-Api-Key", out var apiKeys))
            {
                var user = ValidateApiKey(apiKeys.Single()!);
                if (user == null)
                {
                    // invalid API key
                    return AuthenticateResult.Fail("Invalid API key");
                }
                else
                {
                    // create the current context identity
                    return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(user), Scheme.Name));
                }
            }
            return AuthenticateResult.Fail("Missing header X-Api-Key");
        }

        private ClaimsIdentity? ValidateApiKey(string apiKey)
        {
            if (Options.AllowedKeys.FirstOrDefault(k => k.ApiKey == apiKey) is {} entry)
            {
                // create the API user identity
                return new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, entry.User)
                }, Scheme.Name);
            }
            return null;
        }

    }
}