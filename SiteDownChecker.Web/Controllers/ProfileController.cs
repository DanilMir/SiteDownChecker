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

    [HttpGet]
    public IActionResult Index(int userId)
    {
        var user = userId is default(int)
            ? HttpContext.Items["User"] as User
            : _dataContext.Users.FirstOrDefault(u => u.Id == userId);
        if (user is null)
            return BadRequest();
        return View(user);
    }

    [Authorize, HttpGet]
    public IActionResult Edit() => View(HttpContext.Items["User"]);

    [Authorize, HttpPost]
    public IActionResult Edit([FromForm] User user)
    {
        var currentUser = (User) HttpContext.Items["User"]!;
        if (user.Login != string.Empty && user.Login is not null && _dataContext.Users.All(u => u.Login != user.Login))
            currentUser.Login = user.Login;
        if (user.Name != string.Empty && user.Name is not null)
            currentUser.Name = user.Name;
        if (user.AvatarURL != string.Empty && user.AvatarURL is not null) currentUser.AvatarURL = user.AvatarURL;
        _dataContext.Users.Update(currentUser);
        _dataContext.SaveChanges();
        return Ok();
    }
}
