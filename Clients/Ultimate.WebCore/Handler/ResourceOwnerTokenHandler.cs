using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ultimate.WebCore.Exceptions;
using Ultimate.WebCore.Services.Abstract;

namespace Ultimate.WebCore.Handler
{
    public class ResourceOwnerTokenHandler:DelegatingHandler
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IIdentityService identityService;
        private readonly ILogger<ResourceOwnerTokenHandler> logger;

        public ResourceOwnerTokenHandler(IHttpContextAccessor httpContextAccessor, IIdentityService identityService, ILogger<ResourceOwnerTokenHandler> logger)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.identityService = identityService;
            this.logger = logger;
        }

        protected async override  Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var tokenResponse = await identityService.GetTokenResponseByRefreshToken();
                if (tokenResponse != null)
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnAuthorizeException();
            }

            return response;
        }
    }
}
