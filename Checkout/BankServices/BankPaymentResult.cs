using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.BankServices
{
    public class BankPaymentResult
    {
        private BankPaymentResult(Guid? paymentId, bool isPaymentSuccessful)
        {
            PaymentId = paymentId;
            IsPaymentSuccessful = isPaymentSuccessful;
        }

        public Guid? PaymentId { get; }
        public bool IsPaymentSuccessful { get;  }

        public static BankPaymentResult NewSuccessfulPayment(Guid guid) 
        {
            return new BankPaymentResult(guid, true);
        }

        public static BankPaymentResult NewFailure()
        {
            return new BankPaymentResult(null, false);
        }
    }
}
