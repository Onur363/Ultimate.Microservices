using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Services.Order.Application.Dtos;
using Ultimate.SharedCommon.Dtos;

namespace Ultimate.Services.Order.Application.Queries
{
    public class GetOrdersByUserIdQuery:IRequest<Response<List<OrderDto>>>
    {
        public string UserId { get; set; }
    }
}
