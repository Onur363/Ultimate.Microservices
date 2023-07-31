using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ultimate.SharedCommon.Services.Abstract;

namespace Ultimate.SharedCommon.Services.Concrete
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private IHttpContextAccessor httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId => httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
    }
}
