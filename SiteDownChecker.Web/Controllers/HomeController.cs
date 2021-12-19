using Microsoft.AspNetCore.Mvc;

namespace SiteDownChecker.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Privacy() => View();
}
