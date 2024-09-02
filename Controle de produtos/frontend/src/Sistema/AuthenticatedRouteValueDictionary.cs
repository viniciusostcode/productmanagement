using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

public class AuthenticatedRouteValueDictionary : RouteValueDictionary
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticatedRouteValueDictionary()
    {
    }

    public AuthenticatedRouteValueDictionary(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            Add("controller", "Account");
            Add("action", "Login");
        }
        else
        {
            Add("controller", "Home");
            Add("action", "Products");
        }
    }
}