using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SiteDownChecker.Web.DB;

namespace SiteDownChecker.Web.Controllers;

public class HomeController : Controller
{
    private readonly SiteDownContext _dataContext;

    public HomeController(SiteDownContext dataContext) =>
        _dataContext = dataContext;

    public IActionResult Index()
    {
        _dataContext.Sites.Add(new Site
        {
            Url = "vk.com",
            LogoUrl = "https://play.google.com/store/apps/details?id=com.innersloth.spacemafia&hl=ru&gl=US",
            DownCount = 13458
        });
        _dataContext.SaveChanges();
        return View(_dataContext.Sites);
    }

    public IActionResult Privacy() => View();

    public IActionResult Site(int siteId)
    {
        var site = _dataContext.Sites.FirstOrDefault(s => s.Id == siteId);
        return site switch
        {
            null => BadRequest(),
            _ => View(site)
        };
    }
}
