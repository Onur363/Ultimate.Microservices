using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Ultimate.WebCore.Models.FakePayment;
using Ultimate.WebCore.Services.Abstract;

namespace Ultimate.WebCore.Services.Concrete
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient httpClient;

        public PaymentService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> RecievePaymentAsync(PaymentInfoInput paymentInfoInput)
        {
            var response = await httpClient.PostAsJsonAsync<PaymentInfoInput>("fakepayment/receiverpayment", paymentInfoInput);
            return response.IsSuccessStatusCode;
        }
    }
}
