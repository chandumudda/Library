using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Admin.UI.Models;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Server;

namespace Admin.UI.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IOptions<List<User>> _authUsers;

        public AuthorizationController(IOptions<List<User>> authUsers)
        {
            _authUsers = authUsers;
        }

        [HttpPost("connect/token"), Produces("application/json")]
        public IActionResult Exchange(OpenIdConnectRequest openIdConnectRequest)
        {
            if (!openIdConnectRequest.IsPasswordGrantType())
                throw new InvalidOperationException("The specified grant type is not supported.");

            var user = _authUsers.Value.FirstOrDefault(x => x.UserName == openIdConnectRequest.Username);

            if (user == null)
                throw new InvalidOperationException("Invalid user.");

            if (openIdConnectRequest.Username != user.UserName ||
                openIdConnectRequest.Password != user.Password)
            {
                return Forbid(OpenIddictServerDefaults.AuthenticationScheme);
            }

            var identity = new ClaimsIdentity(
                OpenIddictServerDefaults.AuthenticationScheme,
                OpenIdConnectConstants.Claims.Name,
                OpenIdConnectConstants.Claims.Role);

            identity.AddClaim(OpenIdConnectConstants.Claims.Subject,
                user.ClaimSubject,
                OpenIdConnectConstants.Destinations.AccessToken);
            identity.AddClaim(OpenIdConnectConstants.Claims.Name, user.ClaimName,
                OpenIdConnectConstants.Destinations.AccessToken);
            identity.AddClaim(OpenIdConnectConstants.Claims.Role, user.ClaimRole,
                OpenIdConnectConstants.Destinations.AccessToken);

            var principal = new ClaimsPrincipal(identity);

            return SignIn(principal, OpenIddictServerDefaults.AuthenticationScheme);
        }
    }
}
