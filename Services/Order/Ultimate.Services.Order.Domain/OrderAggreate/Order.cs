using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ultimate.Services.Order.Domain.Core;

namespace Ultimate.Services.Order.Domain.OrderAggreate
{
    //EF Core Features
    //--> OwnedTypes , Shadow Property, Backing Field

    //Önemli Not -> Bir aggregate Root bir entity i yi kullanıyorszsa başka Aggreagete Roote O entity yi kullanmamalı 
    //DDD Basic Compulsroy
    public class Order:Entity,IAggregateRoot
    {
        public DateTime CreatedDate { get; private set; }
        
        //Ef Core tarafında Bu adress entity verisini ayrı bir tabloda açıp Orde ile ilişkilendirebilir
        //veya Order a bağlayıp içindeki Proprtyleri Order tablasouna Column olarak açabilir
        //BU yapılara Owned Entity denilmektedir.
        public Address Address { get; private set; }

        public string BuyerId { get; private set; }

        //Backing Field Dışarıdan ekleme işlemini gidermek Business Kurallarını ilgili entity sınıfları içinde düzenlemek için
        //Encapsulation işlemi yapılıyor.Ef Core burda ilgili Alanı dolduracak
        private readonly List<OrderItem> _orderItems;

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order()
        {
            //default constructor lar lazy loading için önemli
        }
        public Order(string buyerId,Address address)
        {
            _orderItems = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            BuyerId = buyerId;
            Address = address;
        }

        public void AddOrderItem(string productId,string productName,decimal price,string pictureUrl)
        {
            var existProduct = _orderItems.Any(x => x.ProductId == productId);
            if (!existProduct)
            {
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);
                _orderItems.Add(newOrderItem);
            }
        }

        public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);
    }
}
