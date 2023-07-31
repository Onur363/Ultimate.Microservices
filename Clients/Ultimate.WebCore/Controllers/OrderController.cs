using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.WebCore.Models.Orders;
using Ultimate.WebCore.Services.Abstract;

namespace Ultimate.WebCore.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService basketService;
        private readonly IOrderService orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            this.basketService = basketService;
            this.orderService = orderService;
        }

        public async Task<IActionResult> CheckOut()
        {
            var basket = await basketService.GetAsync();
            ViewBag.basket = basket;
            return View(new CheckOutInfoInput());
        }
        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckOutInfoInput checkOutInfoInput)
        {
            /*
             * 1. yol synchron communication
             * var orderStatus = await orderService.CreateOrder(checkOutInfoInput);
             */
            var orderStatus = await orderService.SuspendOrder(checkOutInfoInput);
            if (orderStatus==null)
            {
                ViewBag.error = "sipariş işlemi gerçekleştirilirken bir hata meydana geldi lütfen tekrar deneyiniz";
                return View();
            }

            //1. yol 
            //return RedirectToAction(nameof(SuccessfullCheckOut), new { orderId = orderStatus.OrderId });
            return RedirectToAction(nameof(SuccessfullCheckOut), new { orderId =new Random().Next(1,300) });
        }

        public IActionResult SuccessfullCheckOut(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }

        public async Task<IActionResult> CheckOutHistory()
        {
            return View(await orderService.GetOrder());
        }
    }
}
