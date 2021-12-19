using System.ComponentModel.DataAnnotations;

namespace SiteDownChecker.Web.DB;

public class Site
{
    [Required] public int Id { get; set; }
    [Required] public string Url { get; set; }
    [Required] public string LogoUrl { get; set; }
    [Required] public int DownCount { get; set; }
}
