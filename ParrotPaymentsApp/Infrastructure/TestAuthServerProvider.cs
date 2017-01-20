using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Owin.Security.OAuth;
using Microsoft.AspNet.Identity.EntityFramework;
using ParrotPaymentsApp.Infrastructure;

namespace ParrotPaymentsApp.Infrastructure
{
    public class TestAuthServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (AuthManager _mng = new AuthManager())
            {
                IdentityUser user = await _mng.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_result", "Имя пользователя или пароль введены неправильно.");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);
        }
    }
}