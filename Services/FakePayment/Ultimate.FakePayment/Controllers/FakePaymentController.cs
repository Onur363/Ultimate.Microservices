using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.FakePayment.Models;
using Ultimate.SharedCommon.ControllerBaseSettings;
using Ultimate.SharedCommon.Dtos;
using Ultimate.SharedCommon.Messages;

namespace Ultimate.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomBaseController
    {
        private readonly ISendEndpointProvider sendEndpointProvider;

        public FakePaymentController(ISendEndpointProvider sendEndpointProvider)
        {
            this.sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost("receiverpayment")]
        public async Task<IActionResult> ReceiverPayment(PaymentDto paymentDto)
        {
            var sendEndPoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

            var createOrderMessageCommand = new CreateOrderMessageCommand()
            {
                BuyerId = paymentDto.Order.BuyerId,
                District = paymentDto.Order.Address.District,
                Line = paymentDto.Order.Address.Line,
                Province = paymentDto.Order.Address.Province,
                Street = paymentDto.Order.Address.Street,
                ZipCode = paymentDto.Order.Address.ZipCode,
            };

            paymentDto.Order.OrderItems.ForEach(x =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItem
                {
                    PictureUrl = x.PictureUrl,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName
                });
            });

            await sendEndPoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

            //Business codes
            return CreateActionResultInstance<NoContent>(SharedCommon.Dtos.Response<NoContent>.Success(200));
        }
    }
}
