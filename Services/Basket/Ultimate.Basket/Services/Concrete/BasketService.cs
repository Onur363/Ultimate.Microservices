using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Ultimate.Basket.Dtos;
using Ultimate.Basket.Services.Abstract;
using Ultimate.SharedCommon.Dtos;

namespace Ultimate.Basket.Services.Concrete
{
    public class BasketService : IBasketService
    {
        private readonly RedisService redisService;

        public BasketService(RedisService redisService)
        {
            this.redisService = redisService;
        }

        public async Task<Response<bool>> Delete(string userId)
        {
            var status = await redisService.GetDb().KeyDeleteAsync(userId);
            return status ? Response<bool>.Success(204) : Response<bool>.Fail("basket not found", 404);
        }

        public async Task<Response<BasketDto>> GetBasket(string userId)
        {
            var existBasket = await redisService.GetDb().StringGetAsync(userId);
            if (String.IsNullOrEmpty(existBasket))
            {
                return Response<BasketDto>.Fail("basket not found", 404);
            }

            var basketDto = JsonSerializer.Deserialize<BasketDto>(existBasket);
            return Response<BasketDto>.Success(basketDto, 200);
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketDto basketDto)
        {
            var redisValue = JsonSerializer.Serialize(basketDto);
            var status = await redisService.GetDb().StringSetAsync(basketDto.UserId, redisValue);

            return status ? Response<bool>.Success(204) : Response<bool>.Fail("basket could not update or save", 500);
        }
    }
}
