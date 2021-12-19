using System.Linq;
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
    public IActionResult Index(int userId)
    {
        var user = userId is default(int)
            ? HttpContext.Items["User"] as User
            : _dataContext.Users.FirstOrDefault(u => u.Id == userId);
        if (user is null)
            return BadRequest();
        return View(user);
    }
}
