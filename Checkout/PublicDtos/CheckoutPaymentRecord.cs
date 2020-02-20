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

        public Guid CheckoutPaymentId { get; set; }
        public CheckoutPaymentParameters PaymentParameters { get; set; }
        public string BankPaymentId { get; set; }
        public bool IsSuccessful { get; set; }
        public Instant PaymentDate { get; set; }
    }
}
