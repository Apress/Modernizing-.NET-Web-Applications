using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using Newtonsoft.Json;

namespace ModernizationDemo.WebApi.Security
{
    public class ApiKeyAuthenticationFilter : IAuthenticationFilter
    {
        private readonly IReadOnlyDictionary<string, string> allowedKeys;

        public bool AllowMultiple => true;

        public ApiKeyAuthenticationFilter()
        {
            // load allowed keys from file
            allowedKeys = LoadAllowedApiKeys();
        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // if we have the X-Api-Key header, validate it and set the current context identity
            if (context.Request.Headers.TryGetValues("X-Api-Key", out var apiKeys))
            {
                var user = ValidateApiKey(apiKeys.Single());
                if (user == null)
                {
                    // invalid API key
                    context.ErrorResult = new StatusCodeResult(HttpStatusCode.Unauthorized, context.Request);
                }
                else
                {
                    // set the current context identity
                    context.Principal = new ClaimsPrincipal(user);
                }
            }
            return Task.CompletedTask;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
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

        private IReadOnlyDictionary<string,string> LoadAllowedApiKeys()
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
}