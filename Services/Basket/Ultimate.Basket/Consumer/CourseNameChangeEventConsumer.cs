using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.Basket.Services.Abstract;
using Ultimate.SharedCommon.Messages;
using Ultimate.SharedCommon.Services.Abstract;

namespace Ultimate.Basket.Consumer
{
    public class CourseNameChangeEventConsumer : IConsumer<CourseNameChangeEvent>
    {
        private readonly IBasketService basketService;

        public CourseNameChangeEventConsumer(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            this.basketService = basketService;
        }

        public async Task Consume(ConsumeContext<CourseNameChangeEvent> context)
        {
            var basket = await basketService.GetBasket(context.Message.UserId);

            basket.Data.BasketItems.Where(x => x.CourseId == context.Message.CourseId).
                ToList().ForEach(x => x.CourseName = context.Message.UpdatedName);

            await basketService.SaveOrUpdate(basket.Data);
        }
    }
}
