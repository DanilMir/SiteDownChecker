using Microsoft.AspNetCore.Mvc;
using SiteDownChecker.Web.Attributes;
using SiteDownChecker.Web.DB;

namespace SiteDownChecker.Web.Controllers;

public class ProfileController : Controller
{
    private readonly SiteDownContext _dataContext;
    public ProfileController(SiteDownContext dataContext) => 
        _dataContext = dataContext;

    [Authorize]
    public IActionResult Index() => View(HttpContext.Items["User"]);
}
