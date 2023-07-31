using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.WebCore.Models.PhotoStock;

namespace Ultimate.WebCore.Services.Abstract
{
    public interface IPhotoStockService
    {
        Task<PhotoViewModel> UploadPhoto(IFormFile formFile);
        Task<bool> DeletePhoto(string photoUrl);
    }
}
