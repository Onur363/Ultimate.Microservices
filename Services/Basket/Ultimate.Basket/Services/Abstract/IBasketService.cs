using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.Basket.Dtos;
using Ultimate.SharedCommon.Dtos;

namespace Ultimate.Basket.Services.Abstract
{
    public interface IBasketService
    {
        Task<Response<BasketDto>> GetBasket(string userId);
        Task<Response<bool>> SaveOrUpdate(BasketDto basketDto);
        Task<Response<bool>> Delete(string userId);
    }
}
