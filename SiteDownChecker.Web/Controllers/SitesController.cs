using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiteDownChecker.Web.DB;

namespace SiteDownChecker.Web.Controllers;

public class SitesController : Controller
{
    private readonly SiteDownContext _dataContext;
    public SitesController(SiteDownContext dataContext) => 
        _dataContext = dataContext;

    public class SiteCreationModel
    {
        public string Url { get; set; }
        public string LogoUrl { get; set; }
    }

    [HttpGet]
    public IActionResult Create() => View();
    
    [HttpPost]
    public IActionResult Create(SiteCreationModel site)
    {
        Console.WriteLine($"new site : {site.Url}");
        if (site?.Url is null || site.LogoUrl is null) return BadRequest();
        _dataContext.Sites.Add(new Site
        {
            Url = site.Url,
            LogoUrl = site.LogoUrl,
            DownCount = 0
        });
        _dataContext.SaveChanges();
        return Ok();
    }
}
