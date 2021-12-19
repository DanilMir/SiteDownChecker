using System.ComponentModel.DataAnnotations;

namespace SiteDownChecker.Web.DB;

// ReSharper disable once ClassNeverInstantiated.Global
public class User
{
    public int Id { get; set; }
    [Required] public string Login { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string AvatarURL { get; set; }
    
    public override string ToString() => $"{Login} {Password} {Name} {AvatarURL}";
}
