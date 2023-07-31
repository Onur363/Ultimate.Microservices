using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ultimate.Gateway.DeleegateHandler
{
    public class TokenExchangeDelegateHandler : DelegatingHandler
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        private string _accessToken;

        public TokenExchangeDelegateHandler(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }

        private async Task<string> GetToken(string requestToken)
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                return _accessToken;
            }

            var disco = await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = configuration["IdentityServerUrl"],
                Policy = new DiscoveryPolicy() { RequireHttps = false }
            });

            if (disco.IsError)
            {
                throw disco.Exception;
            }

            TokenExchangeTokenRequest tokenExchangeTokenRequest = new TokenExchangeTokenRequest()
            {
                Address = disco.TokenEndpoint,
                ClientId = configuration["ClientId"],
                ClientSecret = configuration["ClientSecret"],
                GrantType = configuration["TokenGrantType"],
                SubjectToken = requestToken,
                SubjectTokenType = "urn:ietf:params:oauth:token-type:access_token",
                Scope = "openid discount_fullpermission payment_fullpermission"
            };

            var tokenResponse = await httpClient.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest);

            if (tokenResponse.IsError)
            {
                throw tokenResponse.Exception;
            }

            _accessToken = tokenResponse.AccessToken;
            return _accessToken;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestToken = request.Headers.Authorization.Parameter;

            var newToken = await GetToken(requestToken);

            request.SetBearerToken(newToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
