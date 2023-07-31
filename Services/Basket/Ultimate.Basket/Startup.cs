using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.Basket.Consumer;
using Ultimate.Basket.Services;
using Ultimate.Basket.Services.Abstract;
using Ultimate.Basket.Services.Concrete;
using Ultimate.Basket.Settings;
using Ultimate.SharedCommon.Messages;
using Ultimate.SharedCommon.Services.Abstract;
using Ultimate.SharedCommon.Services.Concrete;

namespace Ultimate.Basket
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

            services.AddMassTransit(opt =>
            {
                opt.AddConsumer<CourseNameChangeEventConsumer>();

                opt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(Configuration["RabbitMQURL"], "/", host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });

                    cfg.ReceiveEndpoint("course-name-change-event-basket-service", e =>
                    {
                        e.ConfigureConsumer<CourseNameChangeEventConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();

            var requiredAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //Jwt IdentityServer da eklenen Claim bilgilerini kendi isimleri ile maplemektedir.
            // Sub olarak gelen deðer NameIdentifier deðerine dönüþtürülmektedir. Bunu engekllemk için aþaðýdaki kodu ekledik.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
            services.AddHttpContextAccessor();
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            services.AddScoped<IBasketService, BasketService>();
            services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));

            services.AddSingleton<RedisService>(sp =>
            {
                var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;

                var redis = new RedisService(redisSettings.Host, redisSettings.Port);

                redis.Connect();

                return redis;
            });

            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter(requiredAuthorizePolicy));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ultimate.Basket", Version = "v1" });
            });


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityServerUrl"];
                options.Audience = "resource_basket";
                options.RequireHttpsMetadata = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ultimate.Basket v1"));
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
