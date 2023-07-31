// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace Ultimate.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_catalog"){Scopes={"catalog_fullpermission" } },
            new ApiResource("resource_photo_stock"){Scopes={ "photo_stock_fullpermission" } },
            new ApiResource("resource_basket"){Scopes={ "basket_fullpermission" } },
            new ApiResource("resource_discount"){Scopes={"discount_fullpermission"}},
            new ApiResource("resource_order"){Scopes={"order_fullpermission"}},
            new ApiResource("resource_payment"){Scopes={"payment_fullpermission"}},
            new ApiResource("resource_gateway"){Scopes={"gateway_fullpermission"}},
             new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),//JWT deki payload içeriisndeki Kullanıcı Id bilgisi mutlaka eklenmelidir.
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                //new IdentityResource(){Name="roles",DisplayName="Roles",Description="Kullanıcı rolleri",UserClaims={"roles"} }
                   };

        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
           new ApiScope("catalog_fullpermission","Catalog API için full erişim"),
           new ApiScope("photo_stock_fullpermission","Photo Stock API için full erişim"),
            new ApiScope("basket_fullpermission","basket API için full erişim"),
            new ApiScope("discount_fullpermission","discount için API full erişim"),
             new ApiScope("order_fullpermission","order için API full erişim"),
              new ApiScope("payment_fullpermission","payment için API full erişim"),
               new ApiScope("gateway_fullpermission","gateway için API full erişim"),
           new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientName="Asp.Net Core MVC",
                    ClientId="WebMvcClient",
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={ "gateway_fullpermission", "catalog_fullpermission", "photo_stock_fullpermission",IdentityServerConstants.LocalApi.ScopeName}
                },
                    new Client
                {
                    ClientName="Asp.Net Core MVC",
                    ClientId="WebMvcClientForUser",
                    AllowOfflineAccess=true, //kullanıcı offline olsa dahi token alma işlemi gerçekleştirecek
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    AllowedScopes=
                        {
                            "gateway_fullpermission","order_fullpermission","basket_fullpermission",IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,IdentityServerConstants.LocalApi.ScopeName
                        },
                    AccessTokenLifetime=1*60*60, //1 saat token ömrü var
                    RefreshTokenExpiration=TokenExpiration.Absolute,// refresh toke süresi dolduğunda bitir demek
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds, //refresh token 60 gün saniye cinsinden süresi var
                    RefreshTokenUsage=TokenUsage.ReUse//refresh token tekrar kullanılabilir olsun
                },

                new Client
                {
                    ClientName="Token Exchange Client",
                    ClientId="tokenExchangeClient",
                    ClientSecrets={new Secret("secret".Sha256())},
                    AllowedGrantTypes=new []{"urn:ietf:params:oauth:grant-type:token-exchange"},
                    AllowedScopes={ "discount_fullpermission", "payment_fullpermission",IdentityServerConstants.StandardScopes.OpenId}
                }

            };
    }
}