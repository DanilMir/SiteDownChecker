namespace SiteDownChecker.Web.Authentication
{
    public interface IJwtAuthManager
    {
        string GetToken(string id);
    }
}
