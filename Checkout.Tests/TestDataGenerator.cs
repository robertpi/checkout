using Checkout.PublicDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Tests
{
    public static class TestDataGenerator
    {
        public const string CarNumber           = "5500000000000004";
        public const string MaskedCardNumber    = "************0004";

        public static CheckoutPaymentParameters CreateValidPaymentParameters() 
        {
            var targetPaymentParameters = new CheckoutPaymentParameters()
            {
                // number taken from: https://www.easy400.net/js2/regexp/ccnums.html
                CardNumber = CarNumber,
                Cvv = "124",
                ExpiryMonth = 11,
                ExpiryYear = 2020,
                Amount = 11M,
                Currency = "EUR"
            };

            return targetPaymentParameters;
        }
    }
}
