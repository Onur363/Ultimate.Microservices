using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Ultimate.SharedCommon.Dtos;
using Ultimate.WebCore.Models.FakePayment;
using Ultimate.WebCore.Models.Orders;
using Ultimate.WebCore.Services.Abstract;

namespace Ultimate.WebCore.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService paymentService;
        private readonly HttpClient httpClient;
        private readonly IBasketService basketService;
        private readonly ICatalogService catalogService;

        public OrderService(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService, ICatalogService catalogService)
        {
            this.paymentService = paymentService;
            this.httpClient = httpClient;
            this.basketService = basketService;
            this.catalogService = catalogService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckOutInfoInput checkOutInfoInput)
        {
            var basket = await basketService.GetAsync();
            PaymentInfoInput payment = SetPaymentInfoInput(checkOutInfoInput, basket);
            var responsePayment = await paymentService.RecievePaymentAsync(payment);

            if (!responsePayment)
            {
                return null;
            }

            var orderCreaeteInput = new OrderCreateInput
            {
                Address = new AddressCreateInput
                {
                    District = checkOutInfoInput.District,
                    Line = checkOutInfoInput.Line,
                    Province = checkOutInfoInput.Province,
                    Street = checkOutInfoInput.Street,
                    ZipCode = checkOutInfoInput.ZipCode
                },
            };

            basket.BasketItems.ForEach(x =>
            {
                var picture = catalogService.GetByCourseIdAsync(x.CourseId).Result.Picture;
                var orderItem = new OrderItemCreateInput()
                {
                    PictureUrl = picture,
                    Price = x.GetCurrentPrice,
                    ProductId = x.CourseId,
                    ProductName = x.CourseName
                };
                orderCreaeteInput.OrderItems.Add(orderItem);
            });

            var response = await httpClient.PostAsJsonAsync<OrderCreateInput>("orders/saveorder", orderCreaeteInput);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var orderCreated = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();
            await basketService.DeleteAsync();
            return orderCreated.Data;
        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders/getorder");
            return response.Data;
        }

        public async Task<OrderSuspendViewModel> SuspendOrder(CheckOutInfoInput checkOutInfoInput)
        {
            var basket = await basketService.GetAsync();
            var orderCreaeteInput = new OrderCreateInput
            {
                BuyerId=basket.UserId,
                Address = new AddressCreateInput
                {
                    District = checkOutInfoInput.District,
                    Line = checkOutInfoInput.Line,
                    Province = checkOutInfoInput.Province,
                    Street = checkOutInfoInput.Street,
                    ZipCode = checkOutInfoInput.ZipCode
                },
            };

            basket.BasketItems.ForEach(x =>
            {
                var picture = catalogService.GetByCourseIdAsync(x.CourseId).Result.Picture;
                var orderItem = new OrderItemCreateInput()
                {
                    PictureUrl = picture,
                    Price = x.GetCurrentPrice,
                    ProductId = x.CourseId,
                    ProductName = x.CourseName
                };
                orderCreaeteInput.OrderItems.Add(orderItem);
            });
            PaymentInfoInput payment = SetPaymentInfoInputForMessage(checkOutInfoInput, basket, orderCreaeteInput);
            var responsePayment = await paymentService.RecievePaymentAsync(payment);

            if (!responsePayment)
            {
                return new OrderSuspendViewModel() { Error = "Ödeme işlemi gerçekleşmedi", IsSuccessful = false };
            }

            await basketService.DeleteAsync();

            return new OrderSuspendViewModel() { IsSuccessful = true };

        }

        private static PaymentInfoInput SetPaymentInfoInput(CheckOutInfoInput checkOutInfoInput, Models.Basket.BasketViewModel basket)
        {
            return new PaymentInfoInput
            {
                CardName = checkOutInfoInput.CardName,
                CVV = checkOutInfoInput.CVV,
                CardNumber = checkOutInfoInput.CardNumber,
                TotalPrice = basket.TotalPrice,
                Expiration = checkOutInfoInput.Expiration,
            };
        }

        private static PaymentInfoInput SetPaymentInfoInputForMessage(CheckOutInfoInput checkOutInfoInput, Models.Basket.BasketViewModel basket, OrderCreateInput orderCreateInput)
        {
            return new PaymentInfoInput
            {
                CardName = checkOutInfoInput.CardName,
                CVV = checkOutInfoInput.CVV,
                CardNumber = checkOutInfoInput.CardNumber,
                TotalPrice = basket.TotalPrice,
                Expiration = checkOutInfoInput.Expiration,
                Order = orderCreateInput
            };
        }
    }
}
