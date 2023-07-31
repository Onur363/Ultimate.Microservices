using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.Discount.Services.Abstract;
using Ultimate.SharedCommon.ControllerBaseSettings;
using Ultimate.SharedCommon.Services.Abstract;

namespace Ultimate.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomBaseController
    {
        private readonly IDiscountService discountService;
        private readonly ISharedIdentityService sharedIdentityService;

        public DiscountsController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            this.discountService = discountService;
            this.sharedIdentityService = sharedIdentityService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var discounts = await discountService.GetAll();
            return CreateActionResultInstance(discounts);
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById(int id)
        {
            var discount = await discountService.GetById(id);
            return CreateActionResultInstance(discount);
        }

        [HttpGet("getbycode")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var userId = sharedIdentityService.GetUserId;
            var discount = await discountService.GetByCodeAndUserId(userId, code);
            return CreateActionResultInstance(discount);
        }

        [HttpPost("savediscount")]
        public async Task<IActionResult> Save(Model.Discount discount)
        {
            var result = await discountService.Save(discount);
            return CreateActionResultInstance(result);
        }

        [HttpPut("updatediscount")]
        public async Task<IActionResult> Update(Model.Discount discount)
        {
            var result = await discountService.Update(discount);
            return CreateActionResultInstance(result);
        }

        [HttpDelete("deletediscount")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await discountService.Delete(id);
            return CreateActionResultInstance(result);
        }
    }
}
