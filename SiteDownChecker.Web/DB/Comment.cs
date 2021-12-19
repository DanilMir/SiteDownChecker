using System.ComponentModel.DataAnnotations;

namespace SiteDownChecker.Web.DB;

public class Comment
{
    [Required] public int Id { get; set; }
    [Required] public int WriterId { get; set; }
    [Required] public int SiteId { get; set; }
    [Required] public string Text { get; set; }
}
