using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ultimate.PhotoStock.Dtos;
using Ultimate.SharedCommon.ControllerBaseSettings;
using Ultimate.SharedCommon.Dtos;

namespace Ultimate.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        // Asynckeron işlemleri hata fırlatarak osnlanırabiliriz. Photo yğkleme işlemi belli bir süreyi aştığınd işlemi kesmke için
        //cancelationToken otomatik olarak tetiklenecek ve işlemi iptal edecek
        [HttpPost("photosave")]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {
            if (photo != null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);
                //belirtilem yol üzerinde yeni bir file oluşturur. Varolan varsa üzerine yazılır
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await photo.CopyToAsync(stream, cancellationToken);

                    var returnPath = photo.FileName;

                    PhotoDto photoDto = new() { Url = returnPath };

                    return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
                }
            }
            return CreateActionResultInstance(Response<PhotoDto>.Fail("photo is empty", 400));
        }
        [HttpDelete("photodelete")]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);
            if (!System.IO.File.Exists(path))
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("photo not found", 404));
            }

            System.IO.File.Delete(path);

            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}
