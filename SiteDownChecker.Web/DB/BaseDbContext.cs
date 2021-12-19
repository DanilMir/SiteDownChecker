using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SiteDownChecker.Web.DB;

public abstract class BaseDbContext : DbContext
{
    protected abstract string Catalog { get; }
    private string ConnectionString => $"Data Source=localhost;Initial Catalog={Catalog};Integrated Security=True";

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(config => config.AddConsole()));
        optionsBuilder.UseSqlServer(ConnectionString);
        base.OnConfiguring(optionsBuilder);
    }
}
