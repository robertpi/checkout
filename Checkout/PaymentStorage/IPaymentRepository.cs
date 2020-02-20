using Checkout.PublicDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentStorage
{
    public interface IPaymentRepository
    {
        void SavePayment(CheckoutPaymentRecord paymentRecord);
        CheckoutPaymentRecord? GetPayment(Guid checkoutPaymentId);
    }
}
