using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Services.Order.Infrastructure;
using Ultimate.SharedCommon.Messages;

namespace Ultimate.Services.Order.Application.Consumer
{
    public class CourseNameChangeEventConsumer : IConsumer<CourseNameChangeEvent>
    {
        private readonly OrderDbContext orderDbContext;

        public CourseNameChangeEventConsumer(OrderDbContext orderDbContext)
        {
            this.orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CourseNameChangeEvent> context)
        {
            var orderItems = await orderDbContext.OrderItems.Where(x => x.ProductId == context.Message.CourseId)
                .ToListAsync();

            orderItems.ForEach(x =>
            {
                x.UpdateOrderItem(context.Message.UpdatedName, x.PictureUrl, x.Price);
            });

            await orderDbContext.SaveChangesAsync();
        }
    }
}
