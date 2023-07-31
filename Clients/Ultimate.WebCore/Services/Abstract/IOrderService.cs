using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.WebCore.Models.Orders;

namespace Ultimate.WebCore.Services.Abstract
{
    public interface IOrderService
    {
        /// <summary>
        /// Sycnhrounus communication. İt will be send request to Order Microservice
        /// </summary>
        /// <param name="checkOutInfoInput"></param>
        /// <returns></returns>
        Task<OrderCreatedViewModel> CreateOrder(CheckOutInfoInput checkOutInfoInput);

        /// <summary>
        /// Asynchronous communication
        /// </summary>
        /// <param name="checkOutInfoInput"></param>
        /// <returns></returns>
        Task<OrderSuspendViewModel> SuspendOrder(CheckOutInfoInput checkOutInfoInput);

        Task<List<OrderViewModel>> GetOrder();
    }
}
