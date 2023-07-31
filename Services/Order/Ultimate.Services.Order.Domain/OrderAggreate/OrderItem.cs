﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Services.Order.Domain.Core;

namespace Ultimate.Services.Order.Domain.OrderAggreate
{
    public class OrderItem:Entity
    {
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string PictureUrl { get; private set; }
        public Decimal Price { get; private set; }

        //public int OrderId { get; set; } Bu property ShadowProperty olarak geçeçcek Model üzerinde tutulmuyor ama EfCore
        //Migration sırasında bu alanı açacak
        public OrderItem()
        {
            //Migration işlemleri için default constructor gerekli
        }
        public OrderItem(string productId, string productName, string pictureUrl, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
        }

        public void UpdateOrderItem(string productName,string pictureUrl,decimal price)
        {
            ProductName = productName;
            Price = price;
            PictureUrl = pictureUrl;
        }
    }
}
