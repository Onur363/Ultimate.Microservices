using System;
using System.Collections.Generic;
using System.Text;

namespace Ultimate.SharedCommon.Services.Abstract
{
    public interface ISharedIdentityService
    {
        public string GetUserId { get; }
    }
}
