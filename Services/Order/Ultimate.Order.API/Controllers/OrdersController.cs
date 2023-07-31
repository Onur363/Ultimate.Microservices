using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.Services.Order.Application.Command;
using Ultimate.Services.Order.Application.Queries;
using Ultimate.SharedCommon.ControllerBaseSettings;
using Ultimate.SharedCommon.Services.Abstract;

namespace Ultimate.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : CustomBaseController
    {
        private readonly IMediator mediator;
        private readonly ISharedIdentityService sharedIdentityService;

        public OrdersController(IMediator mediator, ISharedIdentityService sharedIdentityService)
        {
            this.mediator = mediator;
            this.sharedIdentityService = sharedIdentityService;
        }

        [HttpGet("getorder")]
        public async Task<IActionResult> GetOrders()
        {
            var response = await mediator.Send(new GetOrdersByUserIdQuery { UserId = sharedIdentityService.GetUserId });
            return CreateActionResultInstance(response);
        }

        [HttpPost("saveorder")]

        public async Task<IActionResult> SaveOrder(CreateOrderCommand createOrderCommand)
        {
            createOrderCommand.BuyerId = sharedIdentityService.GetUserId;
            var response = await mediator.Send(createOrderCommand);
            return CreateActionResultInstance(response);
        }
    }
}
