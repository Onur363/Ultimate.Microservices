using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.WebCore.Models.Orders;

namespace Ultimate.WebCore.Models.FakePayment
{
    public class PaymentInfoInput
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public decimal TotalPrice { get; set; }

        public OrderCreateInput Order { get; set; }
    }
}
