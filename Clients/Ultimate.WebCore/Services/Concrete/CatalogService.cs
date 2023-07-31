using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Ultimate.SharedCommon.Dtos;
using Ultimate.WebCore.Helper;
using Ultimate.WebCore.Models;
using Ultimate.WebCore.Models.Catalog;
using Ultimate.WebCore.Services.Abstract;

namespace Ultimate.WebCore.Services.Concrete
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient httpClient;
        private readonly IPhotoStockService photoStockService;
        private readonly PhotoHelper photoHelper;

        public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photoHelper)
        {
            this.httpClient = httpClient;
            this.photoStockService = photoStockService;
            this.photoHelper = photoHelper;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            var resultPhotoService = await photoStockService.UploadPhoto(courseCreateInput.PhotoFormFile);
            if (resultPhotoService != null)
            {
                courseCreateInput.Picture = resultPhotoService.Url;
            }

            var response = await httpClient.PostAsJsonAsync<CourseCreateInput>($"courses", courseCreateInput);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var courses = await GetByCourseIdAsync(courseId);
            if (courses != null)
            {
                await photoStockService.DeletePhoto(courses.Picture);
            }

            var response = await httpClient.DeleteAsync($"courses/{courseId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<CourseViewModel>> GetAllAsync()
        {
            //Startup ta HttpClient DI ıBaseAdress set ettiğimiz için  (localhost:5000/services/catalog/courses)
            var response = await httpClient.GetAsync("courses");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();

            responseSuccess.Data.ForEach(x => x.ShortPictureUrl = photoHelper.GetPhotoStockUrl(x.Picture));
            return responseSuccess.Data;
        }

        public async Task<List<CatagoryViewModel>> GetAllCategoryAsync()
        {
            var response = await httpClient.GetAsync("categories");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CatagoryViewModel>>>();
            return responseSuccess.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId)
        {
            var response = await httpClient.GetAsync($"courses/getAllByUserId/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            responseSuccess.Data.ForEach(x => x.ShortPictureUrl = photoHelper.GetPhotoStockUrl(x.Picture));
            return responseSuccess.Data;
        }

        public async Task<CourseViewModel> GetByCourseIdAsync(string courseId)
        {
            var response = await httpClient.GetAsync($"courses/{courseId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
            responseSuccess.Data.ShortPictureUrl = photoHelper.GetPhotoStockUrl(responseSuccess.Data.Picture);
            return responseSuccess.Data;

        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var resultPhotoService = await photoStockService.UploadPhoto(courseUpdateInput.PhotoFormFile);

            if (resultPhotoService != null)
            {
                await photoStockService.DeletePhoto(courseUpdateInput.Picture);
                courseUpdateInput.Picture = resultPhotoService.Url;
            }

            var response = await httpClient.PutAsJsonAsync<CourseUpdateInput>($"courses", courseUpdateInput);
            return response.IsSuccessStatusCode;
        }
    }
}
