using Checkout.PublicDtos;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PaymentStorage
{
    public class InMemoryPaymentStorage : IPaymentStorage
    {
        private readonly ConcurrentDictionary<Guid, CheckoutPaymentRecord> payments 
            = new ConcurrentDictionary<Guid, CheckoutPaymentRecord>();

        public void SavePayment(CheckoutPaymentRecord paymentRecord)
        {
            // always overwrite any existing value at given key
            payments.AddOrUpdate(paymentRecord.CheckoutPaymentId, paymentRecord, (key, oldValue) => paymentRecord);
        }

        // returns null if key not found
        CheckoutPaymentRecord IPaymentStorage.GetPayment(Guid checkoutPaymentId)
        {
            payments.TryGetValue(checkoutPaymentId, out var paymentRecord);
            return paymentRecord;
        }
    }
}
