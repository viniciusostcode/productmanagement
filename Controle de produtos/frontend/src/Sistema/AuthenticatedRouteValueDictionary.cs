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

        // Verifica se o usuário está autenticado
        if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            // Se não estiver autenticado, redireciona para a página de login
            Add("controller", "Account");
            Add("action", "Login");
        }
        else
        {
            // Se estiver autenticado, redireciona para a página de produtos
            Add("controller", "Home");
            Add("action", "Produtos");
        }
    }
}