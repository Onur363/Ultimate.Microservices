using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.WebCore.Models.Catalog;

namespace Ultimate.WebCore.Services.Abstract
{
    public interface ICatalogService
    {
        Task<List<CourseViewModel>> GetAllAsync();
        Task<List<CatagoryViewModel>> GetAllCategoryAsync();
        Task<List<CourseViewModel>> GetAllCourseByUserIdAsync(string userId);
        Task<CourseViewModel> GetByCourseIdAsync(string courseId);
        Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput);
        Task<bool> UpdateCourseAsync(CourseUpdateInput courseCreateInput);

        Task<bool> DeleteCourseAsync(string courseId);
    }
}
