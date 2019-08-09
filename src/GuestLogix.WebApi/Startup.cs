using GuestLogix.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace GuestLogix.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMemoryCache();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Guest Logix Interview - Routes API", Version = "v1" });
            });

            services.AddSingleton<ICacheManager, CacheManager>();
            services.AddSingleton<IRouteService, RouteService>();
            services.AddSingleton<ICsvMapper, CsvMapper>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ICacheManager cacheManager)
        {
            app.UseMvc();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Guest Logix Interview - Routes API");
            });

            //load routes/airport/airline data to cache
            cacheManager.Load();
        }
    }
}
