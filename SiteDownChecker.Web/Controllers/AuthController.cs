using Microsoft.AspNetCore.Mvc;

namespace SiteDownChecker.Web.Controllers;

public class AuthController : Controller
{
    public IActionResult Registration() => View();
    public IActionResult Authentication() => View();
}
