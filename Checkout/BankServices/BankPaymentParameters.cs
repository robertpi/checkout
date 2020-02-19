using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.BankServices
{
    public class BankPaymentParameters
    {
        public BankPaymentParameters(string cardNumber, int expiryMonth, int expiryYear, string cvv, decimal amount, string currency)
        {
            CardNumber = cardNumber;
            ExpiryMonth = expiryMonth;
            ExpiryYear = expiryYear;
            Cvv = cvv;
            Amount = amount;
            Currency = currency;
        }

        public string CardNumber { get; }
        public string Cvv { get; }
        public int ExpiryMonth { get; }
        public int ExpiryYear { get; }
        public decimal Amount { get; }
        public string Currency { get; }
    }
}
