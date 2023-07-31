using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Ultimate.SharedCommon.Dtos;
using Ultimate.WebCore.Models;
using Ultimate.WebCore.Services.Abstract;

namespace Ultimate.WebCore.Services.Concrete
{
    public class IdentityService : IIdentityService
    {

        private readonly HttpClient httpclient;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClientSettings clientSettings;
        private readonly ServiceApiSettings serviceApiSettings;

        public IdentityService(HttpClient httpclient, IHttpContextAccessor httpContextAccessor, IOptions<ServiceApiSettings> serviceApiSettings,
            IOptions<ClientSettings> clientSettings)
        {
            this.httpclient = httpclient;
            this.httpContextAccessor = httpContextAccessor;
            this.clientSettings = clientSettings.Value;
            this.serviceApiSettings = serviceApiSettings.Value;
        }

        public async Task<TokenResponse> GetTokenResponseByRefreshToken()
        {

            var discoveryEndPoint = await GetDiscoveryEndPoint();

            //Cookie ye OpenIDConnectParameterNames.Refresh token ile AuthenticationPropertylerde belirttiğimiz için buradada o değeri kullanıyoruz
            var refreshToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            RefreshTokenRequest refreshTokenRequest = new()
            {
                ClientId = clientSettings.WebClientForUser.ClientId,
                ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                RefreshToken = refreshToken,
                Address = discoveryEndPoint.TokenEndpoint
            };

            var token = await httpclient.RequestRefreshTokenAsync(refreshTokenRequest);
            if (token.IsError)
            {
                return null;
            }

            var authenticationToken = new List<AuthenticationToken>
           {
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(
                    token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture) }
           };

            var authenticationrResult = await httpContextAccessor.HttpContext.AuthenticateAsync();
            var properties = authenticationrResult.Properties;
            properties.StoreTokens(authenticationToken);

            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationrResult.Principal, properties);

            return token;
        }

        public async Task RevokeRefreshToken()
        {
            var discoveryEndPoint = await GetDiscoveryEndPoint();

            var refreshToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocation = new()
            {
                ClientId = clientSettings.WebClientForUser.ClientId,
                ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                Address = discoveryEndPoint.RevocationEndpoint, //Refresh token sonlandırmak için identtiyServer a gideceğimiz endPoint
                Token = refreshToken,
                TokenTypeHint = "refresh_token" //Refresh token ı revoke ettiğimizi belirtmek için IdentityServer kütüphanesinde faydalanıyoruz.

            };

            await httpclient.RevokeTokenAsync(tokenRevocation);
        }

        public async Task<Response<bool>> SignIn(SignInDto signInDto)
        {
            var discoveryEndPoint = await GetDiscoveryEndPoint();

            //BUSınıf ile ilgili BaseUrl ile ResourcePasswordOwner crenditial için token almamızı sağlar
            var token = await GetToken(signInDto, discoveryEndPoint);
            if (token.IsError)
            {
                var responseContext = await token.HttpResponse.Content.ReadAsStringAsync();
                var errorDto = JsonSerializer.Deserialize<ErrorDto>(responseContext, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Response<bool>.Fail(errorDto.Errors, 404);
            }

            ClaimsPrincipal claimsPrincipal = await GetClaimIdentity(discoveryEndPoint, token);
            AuthenticationProperties authenticationProperties = GetAuthenticationProperties(signInDto, token);

            await httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationProperties);

            return Response<bool>.Success(200);
        }

        //Bu metod ile cookie üzerinde token bilgilerinnin tutulasını sağlıyoruz. Kullanıcı bilgisinden beni hatırla kısmı aktif ise
        //IsPersıstent alanı ile bu değeri set ediyoruz
        private static AuthenticationProperties GetAuthenticationProperties(SignInDto signInDto, TokenResponse token)
        {
            var authenticationProperties = new AuthenticationProperties();
            //StoreTokens içerisinde Token,RefreshToken ve ExpiresIn değerlerini tutuyoruz
            authenticationProperties.StoreTokens(new List<AuthenticationToken>() {
                new AuthenticationToken
                {
                     Name=OpenIdConnectParameterNames.AccessToken,
                     Value=token.AccessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = token.RefreshToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)
                },
            });

            authenticationProperties.IsPersistent = signInDto.IsRemember;
            return authenticationProperties;
        }

        private async Task<ClaimsPrincipal> GetClaimIdentity(DiscoveryDocumentResponse discoveryEndPoint, TokenResponse token)
        {
            //Bu sınıf ile adres içeriinde OpenId ile userInfo EndPointitne istek yapacak ve ilgili token içeriisnde ki claim bilgileri gelecek
            var userInfoRequest = new UserInfoRequest()
            {
                Token = token.AccessToken,
                Address = discoveryEndPoint.UserInfoEndpoint
            };

            //IdenttiyModel sayesinde HttpClient içinde Extension oluşturulmuş metod ile İlgili kullanıcının claim bilgileri alıyoruz
            var userInfo = await httpclient.GetUserInfoAsync(userInfoRequest);
            if (userInfo.IsError)
            {
                throw userInfo.Exception;
            }

            // Burada ClaimIdentity nesnesi tanımlarkenoluşturmuş olduğumuz Name ve Role claimlerine hangi isimde erişileceğini belitiyoruz
            //Yani httpCOntext.User. Claimiiçinden ilgii alanlara eirşimi sağlamış olacağız
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;
        }

        private async Task<TokenResponse> GetToken(SignInDto signInDto, DiscoveryDocumentResponse discoveryEndPoint)
        {
            //IdeentityModel sayesinde ilgili istek atma sınıfları hazır olarak geliyor. HttpClient ile ilgili adres paramtereleri il istek gönderebiliyoruz.
            //Burada kullanıcı adı email i clientId ve ClientSecret ile istek yapılır.
            var passwordTokenRequest = new PasswordTokenRequest
            {
                ClientId = clientSettings.WebClientForUser.ClientId,
                ClientSecret = clientSettings.WebClientForUser.ClientSecret,
                UserName = signInDto.Email,
                Password = signInDto.Password,
                Address = discoveryEndPoint.TokenEndpoint

            };
            // Bu httpClient extension metodu ile GrantType password olan bie toke alma işlemi gerçekleştiriyoruz
            var token = await httpclient.RequestPasswordTokenAsync(passwordTokenRequest);
            return token;
        }

        private async Task<DiscoveryDocumentResponse> GetDiscoveryEndPoint()
        {
            //OpenID kullanan ypaıların DiscoveryEnd point ile ilgili EndPoint API ları almış olacak
            //IdentityModel paketinden gelen HttpClient extension yazılmış GetDiscoveryDocuımentAsync metodu
            var discoveryEndPoint = await httpclient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = serviceApiSettings.IdentityBaseUri,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discoveryEndPoint.IsError)
            {
                throw discoveryEndPoint.Exception;
            }

            return discoveryEndPoint;
        }
    }
}
