using Checkout.PublicDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentStorage
{
    public interface IPaymentStorage
    {
        // TODO needs to add more meta data, successful
        void SavePayment(CheckoutPaymentRecord paymentRecord);
        CheckoutPaymentRecord GetPayment(Guid checkoutPaymentId);
    }
}
