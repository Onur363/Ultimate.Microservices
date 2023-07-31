using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ultimate.FakePayment.Models
{
    public class PaymentDto
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public decimal TotalPrice { get; set; }

        //RabbitMq için ödeme alındıntan sonra message kuyruk sistemine gönderilecek ve ödeme alt yapısı oluşturulacak
        public OrderDto Order { get; set; }
    }
}
