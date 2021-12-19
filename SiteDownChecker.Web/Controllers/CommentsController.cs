using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SiteDownChecker.Web.Attributes;
using SiteDownChecker.Web.DB;

namespace SiteDownChecker.Web.Controllers;

public class CommentsController : Controller
{
    private readonly SiteDownContext _dataContext;
    public CommentsController(SiteDownContext dataContext) => 
        _dataContext = dataContext;

    [Authorize]
    public IActionResult Write(string text, int siteId)
    {
        if (text is null)
            return BadRequest();
        var user = HttpContext.Items["User"] as User;
        _dataContext.Comments.Add(new Comment
        {
            WriterId = user!.Id,
            SiteId = siteId,
            Text = text
        });
        _dataContext.SaveChanges();
        return Ok();
    }

    [Authorize]
    public IActionResult Delete(int commentId)
    {
        var comment = _dataContext.Comments.FirstOrDefault(c => c.Id == commentId);
        if (comment is null || comment.WriterId != (HttpContext.Items["User"] as User)!.Id)
        {
            Console.WriteLine(comment is null
                ? $"cant find comment with id = {commentId}"
                : $"{comment.WriterId} != {(HttpContext.Items["User"] as User)!.Id}");
            return BadRequest();
        }

        lock (_dataContext)
        {
            _dataContext.Comments.Remove(comment);
            _dataContext.SaveChanges();
        }
        return Ok();
    }
}
