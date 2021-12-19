using System.Linq;

namespace SiteDownChecker.Web.DB;

public static class DbHelper
{
    public static User? GetUserWithId(string login, string password, SiteDownContext dataContext) =>
        dataContext.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
}
