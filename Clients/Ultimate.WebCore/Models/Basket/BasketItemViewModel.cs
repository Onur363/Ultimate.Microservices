﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ultimate.WebCore.Models.Basket
{
    public class BasketItemViewModel
    {
        public int Quantity { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }

        private decimal? DiscountAppliedPrice;

        public decimal GetCurrentPrice => DiscountAppliedPrice.HasValue ? DiscountAppliedPrice.Value : Price;

        public void AppliedDiscount(decimal disCountPrice)
        {
            DiscountAppliedPrice = disCountPrice;
        }
    }
}
