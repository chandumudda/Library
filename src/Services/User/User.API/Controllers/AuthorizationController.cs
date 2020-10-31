using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenIddict.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : Controller
    {
        private readonly IOptions<List<Models.User>> _authUsers;

        public AuthorizationController(IOptions<List<Models.User>> authUsers)
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