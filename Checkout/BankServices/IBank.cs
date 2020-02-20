using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkout.BankServices
{
    public interface IBank
    {
        public BankPaymentResult ProcessPayment(BankPaymentParameters parameters);
    }
}
