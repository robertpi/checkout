using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.BankServices
{
    public class BankSimulator : IBank
    {
        private readonly Random rnd = new Random();

        public BankPaymentResult ProcessPayment(BankPaymentParameters parameters)
        {
            if (rnd.Next(100) == 42)
            {
                return BankPaymentResult.NewFailure();
            }

            return BankPaymentResult.NewSuccessfulPayment(Guid.NewGuid());
            
        }
    }
}
