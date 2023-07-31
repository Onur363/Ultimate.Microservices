using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.WebCore.Models.Basket;
using Ultimate.WebCore.Models.Discount;
using Ultimate.WebCore.Services.Abstract;

namespace Ultimate.WebCore.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly ICatalogService catalogService;
        private readonly IBasketService basketService;

        public BasketController(ICatalogService catalogService, IBasketService basketService)
        {
            this.catalogService = catalogService;
            this.basketService = basketService;
        }

        public async Task<IActionResult> Index()
        {
            var basket = await basketService.GetAsync();

            return View(basket);
        }

        public async Task<IActionResult> AddBasketItem(string courseId)
        {
            var course = await catalogService.GetByCourseIdAsync(courseId);

            var basketItem = new BasketItemViewModel { CourseId = course.Id, CourseName = course.Name, Quantity = 1, Price = course.Price };

            await basketService.AddBasketItemAsync(basketItem);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveBasketItem(string courseId)
        {
            await basketService.RemoveBasketItemAsync(courseId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ApplyDiscount(DiscountApplyInput discountApplyInput)
        {
            if (!ModelState.IsValid)
            {
                TempData["discountError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).First();
                return RedirectToAction(nameof(Index));
            }

            var discountStatus = await basketService.ApplyDiscountAsync(discountApplyInput.Code);
            TempData["discountStatus"] = discountStatus;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CancelApplyDiscount()
        {
            await basketService.CancelApplyDiscountAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
