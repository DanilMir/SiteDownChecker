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
        var comments = _dataContext.Comments
            .Where(c => c.SiteId == siteId)
            .Select(c => new BetterComment
            {
                Id = c.Id,
                Writer = _dataContext.Users.FirstOrDefault(u => u.Id == c.WriterId),
                Text = c.Text
            });
        return site switch
        {
            null => BadRequest(),
            _ => View(new SiteInfoModel
            {
                UserId = HttpContext.Items["User"] is User user ? user.Id : 0,
                Site = site,
                Comments = comments
            })
        };
    }

    public readonly struct BetterComment
    {
        public int Id { get; init; }
        public User Writer { get; init; }
        public string Text { get; init; }
    }

    public readonly struct SiteInfoModel
    {
        public Site Site { get; init; }
        public int UserId { get; init; }
        public IQueryable<BetterComment> Comments { get; init; }
    }
}
