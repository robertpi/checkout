using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.PublicDtos
{
    public class CheckoutPaymentResult
    {
        public Guid CheckoutPaymentId { get; set; }
        public bool IsPaymentSuccessful { get; set; }
    }
}
