using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.WebCore.Models.FakePayment;

namespace Ultimate.WebCore.Services.Abstract
{
    public interface IPaymentService
    {
        Task<bool> RecievePaymentAsync(PaymentInfoInput paymentInfoInput);
    }
}
