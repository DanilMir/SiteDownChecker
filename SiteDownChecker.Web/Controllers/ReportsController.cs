using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SiteDownChecker.Web.DB;

namespace SiteDownChecker.Web.Controllers;

public class ReportsController : Controller
{
    private readonly SiteDownContext _dataContext;

    public ReportsController(SiteDownContext dataContext) =>
        _dataContext = dataContext;

    public IActionResult Report(int siteId)
    {
        var site = _dataContext.Sites.FirstOrDefault(s => s.Id == siteId);
        if (site is null) return BadRequest();
        ++site!.DownCount;
        _dataContext.Sites.Update(site);
        _dataContext.SaveChanges();
        return Ok();
    }
}
