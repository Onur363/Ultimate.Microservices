using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.SharedCommon.Services.Abstract;
using Ultimate.SharedCommon.Services.Concrete;
using Ultimate.WebCore.Handler;
using Ultimate.WebCore.Models;
using Ultimate.WebCore.Services.Abstract;
using Ultimate.WebCore.Services.Concrete;

namespace Ultimate.WebCore.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISharedIdentityService, SharedIdentityService>();
            services.AddHttpContextAccessor();
            services.AddHttpClient<IIdentityService, IdentityService>();
            services.AddAccessTokenManagement();//IClientAccessTokenCache DI çözümlemesi yapması için bu kodu servise ekliyoruz

            //Bu OptionsPAttern Configurasyonu ile herhangi bir class ta Constructor da "IOptions<T>" interface ni DI işlemni gerçekleştiriyor olacağız.
            services.Configure<ServiceApiSettings>(configuration.GetSection("ServiceApiSettings"));
            services.Configure<ClientSettings>(configuration.GetSection("ClientSettings"));

            var serviceApiSettings = configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

            //İlgili Client belirtilen Servicelerde kullanılmaya başladığında baseAddRees ini ve RequestHeardr ını tanımlamış olduğumuz sınıftan veriyoruz.
            services.AddHttpClient<IUserService, UserService>(opt =>
            {
                opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
            }).AddHttpMessageHandler<ResourceOwnerTokenHandler>();

            services.AddHttpClient<ICatalogService, CatalogService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Catalog.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IPhotoStockService, PhotoStockService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.PhotoStock.Path}");
            }).AddHttpMessageHandler<ClientCredentialTokenHandler>();

            services.AddHttpClient<IBasketService, BasketService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Basket.Path}");
            }).AddHttpMessageHandler<ResourceOwnerTokenHandler>();

            services.AddHttpClient<IDiscountService, DiscountService>(opt =>
             {
                 opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Discount.Path}");
             }).AddHttpMessageHandler<ResourceOwnerTokenHandler>();

            services.AddHttpClient<IPaymentService, PaymentService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Payment.Path}");
            }).AddHttpMessageHandler<ResourceOwnerTokenHandler>();

            services.AddHttpClient<IOrderService, OrderService>(opt =>
            {
                opt.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}/{serviceApiSettings.Order.Path}");
            }).AddHttpMessageHandler<ResourceOwnerTokenHandler>();

            services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();

            return services;
        }
    }
}
