﻿using Microsoft.AspNetCore.Authentication;

namespace ModernizationDemo.WebApiCore.Security;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{

    public IReadOnlyList<AllowedApiKeyEntry> AllowedKeys { get; set; }

    public class AllowedApiKeyEntry
    {
        public string User { get; set; }
        public string ApiKey { get; set; }
    }

}