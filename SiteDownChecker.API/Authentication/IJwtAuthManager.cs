namespace SiteDownChecker.API.Authentication
{
    public interface IJwtAuthManager
    {
        string GetToken(string login);
    }
}
