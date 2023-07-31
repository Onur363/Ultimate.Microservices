using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.SharedCommon.Services.Abstract;
using Ultimate.WebCore.Models.Catalog;
using Ultimate.WebCore.Services.Abstract;

namespace Ultimate.WebCore.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatalogService catalogService;
        private readonly ISharedIdentityService sharedIdentityService;

        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            this.catalogService = catalogService;
            this.sharedIdentityService = sharedIdentityService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await catalogService.GetAllCourseByUserIdAsync(sharedIdentityService.GetUserId));
        }

        public async Task<IActionResult> Create()
        {
            var categories = await catalogService.GetAllCategoryAsync();
            ViewBag.categoryList = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
        {
            var categories = await catalogService.GetAllCategoryAsync();
            if (!ModelState.IsValid)
            {
                ViewBag.categoryList = new SelectList(categories, "Id", "Name");
                return View();
            }
            courseCreateInput.UserId = sharedIdentityService.GetUserId;

            await catalogService.CreateCourseAsync(courseCreateInput);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            var course = await catalogService.GetByCourseIdAsync(id);
            var categories = await catalogService.GetAllCategoryAsync();

            if (course != null)
            {
                ViewBag.categoryList = new SelectList(categories, "Id", "Name", course.CategoryId);
                CourseUpdateInput courseUpdateInput = new()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Feature = course.Feature,
                    Price = course.Price,
                    CategoryId = course.CategoryId,
                    Description = course.Description,
                    UserId = course.UserId,
                    Picture = course.Picture
                };
                return View(courseUpdateInput);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
        {
            var categories = await catalogService.GetAllCategoryAsync();
            if (!ModelState.IsValid)
            {
                ViewBag.categoryList = new SelectList(categories, "Id", "Name", courseUpdateInput.CategoryId);
                return View();
            }

            await catalogService.UpdateCourseAsync(courseUpdateInput);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await catalogService.DeleteCourseAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
