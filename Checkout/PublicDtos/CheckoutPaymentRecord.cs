using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PublicDtos
{
    public class CheckoutPaymentRecord
    {
        public CheckoutPaymentRecord()
        {
            // avoids compiler nullable warnings
            PaymentParameters = new CheckoutPaymentParameters();
            BankPaymentId = "";
        }

        // a seperated id for record is allocated by us, because the bank
        // doesn't return an id in the case of a failure and we want to record failures
        // it will also be useful if other banks are integrated, since there identifiers
        // will probably have different formats.
        public Guid CheckoutPaymentId { get; set; }
        public CheckoutPaymentParameters PaymentParameters { get; set; }
        public string BankPaymentId { get; set; }
        public bool IsSuccessful { get; set; }
        public Instant PaymentDate { get; set; }
    }
}
