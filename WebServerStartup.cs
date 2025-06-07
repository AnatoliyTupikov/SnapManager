using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapManager
{
    internal  class WebServerStartup
    {
        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddControllersWithViews();
            services.AddControllers();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) 
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
                      
            app.UseEndpoints(endpoints =>
            {
                // Простой маршрут
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
