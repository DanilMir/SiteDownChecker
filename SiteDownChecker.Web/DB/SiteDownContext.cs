using Microsoft.EntityFrameworkCore;

namespace SiteDownChecker.Web.DB;

public class SiteDownContext : BaseDbContext
{
    protected override string Catalog => Config.SiteDownCatalog;
    
    public DbSet<User> Users { get; set; }
}
