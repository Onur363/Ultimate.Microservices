using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Ultimate.SharedCommon.Dtos;
using Ultimate.WebCore.Models.Basket;
using Ultimate.WebCore.Services.Abstract;

namespace Ultimate.WebCore.Services.Concrete
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient httpClient;
        private readonly IDiscountService discountService;
        public BasketService(HttpClient httpClient, IDiscountService discountService)
        {
            this.httpClient = httpClient;
            this.discountService = discountService;
        }

        public async Task AddBasketItemAsync(BasketItemViewModel basketItemViewModel)
        {
            var basket = await GetAsync();
            if (basket != null)
            {
                if (!basket.BasketItems.Any(x => x.CourseId == basketItemViewModel.CourseId))
                {
                    basket.BasketItems.Add(basketItemViewModel);
                }
            }
            else
            {
                basket = new BasketViewModel();
                basket.BasketItems.Add(basketItemViewModel);
            }

            await SaveOrUpdateAsync(basket);
        }

        public async Task<bool> ApplyDiscountAsync(string discountCode)
        {
            await CancelApplyDiscountAsync();
            var basket = await GetAsync();
            if (basket == null)
            {
                return false;
            }

            var hasDiscount = await discountService.GetDiscountAsync(discountCode);
            if (hasDiscount == null)
            {
                return false;
            }

            basket.ApplyDiscount(hasDiscount.Code,hasDiscount.Rate);

            await SaveOrUpdateAsync(basket);
            return true;
        }

        public async Task<bool> CancelApplyDiscountAsync()
        {
            var basket = await GetAsync();
            if (basket == null || basket.DiscountCode == null)
            {
                return false;
            }

            basket.CancelApplyDiscount();
            await SaveOrUpdateAsync(basket);
            return true;
        }

        public async Task<bool> DeleteAsync()
        {
            var result = await httpClient.DeleteAsync("baskets/delete");
            return result.IsSuccessStatusCode;
        }

        public async Task<BasketViewModel> GetAsync()
        {
            var response = await httpClient.GetAsync("baskets/getbasket");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var basketViewModel = await response.Content.ReadFromJsonAsync<Response<BasketViewModel>>();
            return basketViewModel.Data;
        }

        public async Task<bool> RemoveBasketItemAsync(string courseId)
        {
            var basket = await GetAsync();
            if (basket == null)
            {
                return false;
            }

            var deleteResult = basket.BasketItems.Remove(basket.BasketItems.FirstOrDefault(x => x.CourseId == courseId));
            if (!deleteResult)
            {
                return false;
            }

            //basket ıtemlar boş ise
            if (!basket.BasketItems.Any())
            {
                basket.DiscountCode = null;
            }

            await SaveOrUpdateAsync(basket);
            return deleteResult;
        }

        public async Task<bool> SaveOrUpdateAsync(BasketViewModel basketViewModel)
        {
            var response = await httpClient.PostAsJsonAsync<BasketViewModel>("baskets/saveorupdate", basketViewModel);
            return response.IsSuccessStatusCode;
        }

        
    }
}
