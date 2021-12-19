using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SiteDownChecker.Web.DB;

namespace SiteDownChecker.Web.Controllers;

public class HomeController : Controller
{
    private readonly SiteDownContext _dataContext;

    public HomeController(SiteDownContext dataContext) =>
        _dataContext = dataContext;

    public IActionResult Index() => View(_dataContext.Sites);
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
