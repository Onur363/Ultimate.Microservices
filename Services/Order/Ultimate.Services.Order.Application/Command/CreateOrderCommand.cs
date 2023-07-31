using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Services.Order.Application.Dtos;
using Ultimate.Services.Order.Domain.OrderAggreate;
using Ultimate.SharedCommon.Dtos;

namespace Ultimate.Services.Order.Application.Command
{
    public class CreateOrderCommand:IRequest<Response<CreatedOrderDto>>
    {
        public string BuyerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public AddressDto Address { get; set; }

    }
}
