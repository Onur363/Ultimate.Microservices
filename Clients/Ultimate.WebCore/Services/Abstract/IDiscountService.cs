using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.WebCore.Models.Discount;

namespace Ultimate.WebCore.Services.Abstract
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscountAsync(string discountCode);
    }
}
