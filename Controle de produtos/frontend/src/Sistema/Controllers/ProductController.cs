using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class ProductsController : Controller
{
    public IActionResult Index()
    {
        var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Unknown";
        ViewData["UserName"] = userName;

        return View();
    }
}