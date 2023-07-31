using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Ultimate.SharedCommon.Dtos;
using Ultimate.WebCore.Models.Discount;
using Ultimate.WebCore.Services.Abstract;

namespace Ultimate.WebCore.Services.Concrete
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient httpClient;

        public DiscountService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<DiscountViewModel> GetDiscountAsync(string discountCode)
        {
            var response = await httpClient.GetAsync($"discounts/getbycode?code={discountCode}");

            if (!response.IsSuccessStatusCode) { return null; }

            var discount = await response.Content.ReadFromJsonAsync<Response<DiscountViewModel>>();
            return discount.Data;
        }
    }
}
