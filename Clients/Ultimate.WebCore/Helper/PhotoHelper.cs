using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.WebCore.Models;

namespace Ultimate.WebCore.Helper
{
    public class PhotoHelper
    {
        private readonly ServiceApiSettings serviceApiSettings;

        public PhotoHelper(IOptions<ServiceApiSettings> serviceApiSettings)
        {
            this.serviceApiSettings = serviceApiSettings.Value;
        }

        public string GetPhotoStockUrl(string photoUrl)
        {
            return $"{serviceApiSettings.PhotoStockUri}/photos/{photoUrl}";
        }
    }
}
