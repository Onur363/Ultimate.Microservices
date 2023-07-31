using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Services.Order.Infrastructure;
using Ultimate.SharedCommon.Messages;

namespace Ultimate.Services.Order.Application.Consumer
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext orderDbContext;

        public CreateOrderMessageCommandConsumer(OrderDbContext orderDbContext)
        {
            this.orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var newAddress = new Domain.OrderAggreate.Address(context.Message.Province, context.Message.District
                , context.Message.Street, context.Message.Line, context.Message.ZipCode);

            Domain.OrderAggreate.Order order = new Domain.OrderAggreate.Order(context.Message.BuyerId,
                newAddress);

            context.Message.OrderItems.ForEach(x =>
            {
                order.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl);
            });

            await orderDbContext.Orders.AddAsync(order);

            await orderDbContext.SaveChangesAsync();
        }
    }
}
