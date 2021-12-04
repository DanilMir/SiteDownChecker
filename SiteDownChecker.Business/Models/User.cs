using System;

namespace SiteDownChecker.Business.Models;

public class User : IConvertibleFromBusinessModel<User, User>, IConvertibleToBusinessModel<User>
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}
