﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.SharedCommon.Dtos;

namespace Ultimate.Discount.Services.Abstract
{
    public interface IDiscountService
    {
        Task<Response<List<Model.Discount>>> GetAll();

        Task<Response<Model.Discount>> GetById(int id);

        Task<Response<NoContent>> Save(Model.Discount discount);

        Task<Response<NoContent>> Update(Model.Discount discount);

        Task<Response<NoContent>> Delete(int id);

        Task<Response<Model.Discount>> GetByCodeAndUserId(string userId,string code);
    }
}
