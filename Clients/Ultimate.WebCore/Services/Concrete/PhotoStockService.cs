using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Ultimate.WebCore.Models.PhotoStock;
using Ultimate.WebCore.Services.Abstract;
using System.Net.Http.Json;
using Ultimate.SharedCommon.Dtos;

namespace Ultimate.WebCore.Services.Concrete
{
    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> DeletePhoto(string photoUrl)
        {
            var response = await httpClient.DeleteAsync($"photos/photodelete?photoUrl={photoUrl}");

            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoViewModel> UploadPhoto(IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return null;
            }

            //Path.GetExtension("fileName") gönderilne dosya isminin uzantısını oluşturur
            var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(formFile.FileName)}";

            /*using(var ms=new MemoryStream())
               { }
             */
            using var ms = new MemoryStream();

            await formFile.CopyToAsync(ms);
            var multiPartContent = new MultipartFormDataContent();

            //photo stock servisine ögndereceğimiz için "photo" key randomFileName values ikilis olmuş olacak
            multiPartContent.Add(new ByteArrayContent(ms.ToArray()), "photo", randomFileName);

            var response = await httpClient.PostAsync("photos/photosave", multiPartContent);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var photoViewModel = await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();
            return photoViewModel.Data;

        }
    }
}
