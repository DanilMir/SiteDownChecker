using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteDownChecker.Web.DB;

// ReSharper disable once ClassNeverInstantiated.Global
public class User
{
    public int Id { get; set; }
    [Required] public string Login { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Surname { get; set; }
    [Required] public long PhoneNumber { get; set; }
    [Required] public string AvatarURL { get; set; }
    
    [NotMapped] public string FullName => $"{Name} {Surname}";
    public override string ToString() => $"{Login} {Password} {Name} {Surname} {PhoneNumber}";
}
