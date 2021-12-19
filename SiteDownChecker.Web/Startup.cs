using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SiteDownChecker.Web.DB;
using SiteDownChecker.Web.Services;

namespace SiteDownChecker.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => 
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<SiteDownContext>();
            
            services.AddRazorPages();
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) =>
            (env.IsDevelopment()
                ? app.UseDeveloperExceptionPage()
                : app.UseExceptionHandler("/Home/Error").UseHsts())
            .UseStatusCodePagesWithReExecute("/{0}")
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseRouting()
            .UseAuthorization()
            .UseMiddleware<JwtMiddleware>()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
    }
}
