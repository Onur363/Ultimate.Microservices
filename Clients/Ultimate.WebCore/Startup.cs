using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.WebCore.Extensions;
using Ultimate.WebCore.Handler;
using Ultimate.WebCore.Helper;
using Ultimate.WebCore.Models;
using Ultimate.WebCore.Services.Abstract;
using Ultimate.WebCore.Services.Concrete;
using Ultimate.WebCore.Validation;

namespace Ultimate.WebCore
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
            //HttpClient isteklerinden önce Header a token eklemeyi saðlayan Hnadler sýnýfýmýzýda service olarak ekliyoruz
            services.AddScoped<ResourceOwnerTokenHandler>();
            services.AddScoped<ClientCredentialTokenHandler>();
            services.AddSingleton<PhotoHelper>();
            services.AddCustomServices(Configuration);

            //bu kod ile FleuntValidation ý ilgili kullandýðýmýz yerdeki classýný verip tüm Validatorlarý dahil etmeye yarýyor.
            //RegisterValidatorsFromAssemblyContaining
            services.AddControllersWithViews().AddFluentValidation(fv=>fv.RegisterValidatorsFromAssemblyContaining<CourseCreateInputValidator>());

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
                AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
                {
                    opts.LoginPath = "/Auth/SignIn";
                    opts.ExpireTimeSpan = TimeSpan.FromDays(60);
                    opts.SlidingExpiration = true;
                    opts.Cookie.Name = "ULTIMATEWEB";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
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
