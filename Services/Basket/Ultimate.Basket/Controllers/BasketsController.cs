using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.Basket.Dtos;
using Ultimate.Basket.Services.Abstract;
using Ultimate.SharedCommon.ControllerBaseSettings;
using Ultimate.SharedCommon.Services.Abstract;

namespace Ultimate.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : CustomBaseController
    {
        private readonly IBasketService basketService;
        private readonly ISharedIdentityService sharedIdentityService;

        public BasketsController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            this.basketService = basketService;
            this.sharedIdentityService = sharedIdentityService;
        }

        [HttpGet("getbasket")]
        public async Task<IActionResult> GetBaske()
        {
            var basket = await basketService.GetBasket(sharedIdentityService.GetUserId);
            return CreateActionResultInstance(basket);
        }

        [HttpPost("saveorupdate")]
        public async Task<IActionResult> SaveOrUpdate(BasketDto basketDto)
        {
            basketDto.UserId = sharedIdentityService.GetUserId;
            var response = await basketService.SaveOrUpdate(basketDto);
            return CreateActionResultInstance(response);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBasket()
        {
            var basket = await basketService.Delete(sharedIdentityService.GetUserId);
            return CreateActionResultInstance(basket);
        }
    }
}
