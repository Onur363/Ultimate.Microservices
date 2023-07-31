using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ultimate.Services.Order.Application.Command;
using Ultimate.Services.Order.Application.Dtos;
using Ultimate.Services.Order.Domain.OrderAggreate;
using Ultimate.Services.Order.Infrastructure;
using Ultimate.SharedCommon.Dtos;

namespace Ultimate.Services.Order.Application.Handles
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly OrderDbContext context;

        public CreateOrderCommandHandler(OrderDbContext context)
        {
            this.context = context;
        }

        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newAddress = new Address(request.Address.Province, request.Address.District, request.Address.Street, request.Address.ZipCode, request.Address.Line);
            Domain.OrderAggreate.Order order = new Domain.OrderAggreate.Order(request.BuyerId, newAddress);

            request.OrderItems.ForEach(x =>
            {
                order.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl);
            });

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            return Response<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId=order.Id },200);

        }
    }
}
