using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.SharedCommon.Dtos;
using Ultimate.WebCore.Models;

namespace Ultimate.WebCore.Services.Abstract
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SignInDto signInDto);
        Task<TokenResponse> GetTokenResponseByRefreshToken();
        Task RevokeRefreshToken();
    }
}
