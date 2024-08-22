using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Hosting;
using Microsoft.Owin;
using Newtonsoft.Json;

namespace ModernizationDemo.SignalR.Security;

public class ApiKeyAuthenticationMiddleware : OwinMiddleware
{
    private readonly IReadOnlyDictionary<string, string> allowedKeys;

    public ApiKeyAuthenticationMiddleware(OwinMiddleware next) : base(next)
    {
        // load allowed keys from file
        allowedKeys = LoadAllowedApiKeys();
    }

    public override Task Invoke(IOwinContext context)
    {
        var apiKey = GetApiKeyFromUrlOrHeader(context);
        if (apiKey != null)
        {
            // check the API key
            var user = ValidateApiKey(apiKey);
            if (user != null)
            {
                // set the current context identity
                context.Authentication.User = new ClaimsPrincipal(user);
            }
            else
            {
                // return 401 Unauthorized
                context.Response.StatusCode = 401;
                context.Response.ReasonPhrase = "Invalid API Key";
            }
        }

        return Next.Invoke(context);
    }

    private static string GetApiKeyFromUrlOrHeader(IOwinContext context)
    {
        var apiKey = context.Request.Query.Get("auth-token");
        if (!string.IsNullOrEmpty(apiKey))
        {
            return apiKey;
        }

        apiKey = context.Request.Headers.Get("X-Api-Key");
        if (!string.IsNullOrEmpty(apiKey))
        {
            return apiKey;
        }

        return null;
    }

    private ClaimsIdentity ValidateApiKey(string apiKey)
    {
        if (allowedKeys.TryGetValue(apiKey, out var user))
        {
            // create the API user identity
            return new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user)
            }, "X-Api-Key");
        }
        return null;
    }


    private IReadOnlyDictionary<string, string> LoadAllowedApiKeys()
    {
        var keysFile = File.ReadAllText(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data/apiUsers.json"));
        return JsonConvert.DeserializeObject<AllowedApiKeyEntry[]>(keysFile)
            .ToDictionary(u => u.ApiKey, u => u.User);
    }

    public class AllowedApiKeyEntry
    {
        public string User { get; set; }
        public string ApiKey { get; set; }
    }
}