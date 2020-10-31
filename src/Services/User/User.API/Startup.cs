using System.Collections.Generic;
using LibraryCore.Extensions;
using LibraryCore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Validation;

namespace User.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.Configure<DbConnectionDetails>(Configuration.GetSection("DbConnectionDetails"));
            services.Configure<List<Models.User>>(Configuration.GetSection("AuthenticationDetails"));
            services.AddConfigurationsCommon();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<DbContext>(options =>
            {
                options.UseInMemoryDatabase(nameof(DbContext));
                options.UseOpenIddict();
            });
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<DbContext>();
                })
                .AddServer(options =>
                {
                    options.UseMvc();
                    options.EnableTokenEndpoint("/connect/token");
                    options.AllowPasswordFlow();
                    options.AcceptAnonymousClients();
                    options.DisableHttpsRequirement();
                })
                .AddValidation();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OpenIddictValidationDefaults.AuthenticationScheme;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}