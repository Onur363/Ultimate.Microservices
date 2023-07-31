using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ultimate.Basket.Dtos
{
    public class BasketDto
    {
        public string UserId { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }
        /// <summary>
        /// ShortCut => public decimal TotalPrice => BasketItems.Sum(x => x.Price * x.Quantity);
        /// </summary>
        public decimal TotalPrice
        {
            get { return BasketItems.Sum(x => x.Quantity * x.Price); }
        }
    }
}
