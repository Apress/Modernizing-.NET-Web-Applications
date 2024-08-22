using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModernizationDemo.App.Migration
{
    public class ForwardedHeadersModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
        }

        private void OnBeginRequest(object sender, EventArgs e)
        {
            var request = ((HttpApplication)sender).Request;

            // handle the remote client IP address
            if (request.Headers["X-Forwarded-For"] is {} forwardedFor)
            {
                // the format is "client, proxy1, proxy2"
                var parts = forwardedFor.Split(new[] { ',' });
                request.ServerVariables["REMOTE_ADDR"] = parts[0].Trim();
                request.ServerVariables["REMOTE_HOST"] = parts[0].Trim();
            }

            // handle the scheme and host
            if (request.Headers["X-Forwarded-Proto"] == "https")
            {
                request.ServerVariables["HTTPS"] = "on";
                request.ServerVariables["SERVER_PORT_SECURE"] = "1";
                request.ServerVariables["SERVER_PORT"] = "433";
            }

            if (request.Headers["X-Forwarded-Host"] is {} forwardedHost)
            {
                var parts = forwardedHost.Split(new[] { ':' });
                request.ServerVariables["SERVER_NAME"] = parts[0];

                if (parts.Length > 1)
                {
                    request.ServerVariables["SERVER_PORT"] = parts[1];
                }
            }
        }

        public void Dispose()
        {
        }
    }
}