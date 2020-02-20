using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.BankServices;
using Checkout.PaymentStorage;
using Checkout.PublicDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NodaTime;

namespace Checkout.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : Controller
    {
        private readonly IBank bank;
        private readonly IPaymentRepository paymentRepository;
        private readonly IClock clock;
        private readonly ILogger logger;

        public PaymentController(IBank bank, IPaymentRepository paymentRepository, IClock clock, ILogger<PaymentController> logger)
        {
            this.bank = bank;
            this.paymentRepository = paymentRepository;
            this.clock = clock;
            this.logger = logger;
        }

        [HttpPost]
        public CheckoutPaymentResult Post([FromBody]CheckoutPaymentParameters paymentParameters)
        {
            BankPaymentParameters bankPaymentParameters = CreateBankPaymentParameters(paymentParameters);

            var result = bank.ProcessPayment(bankPaymentParameters);

            var paymentRecord = CreatPaymentObject(paymentParameters, result);
            paymentRepository.SavePayment(paymentRecord);

            return new CheckoutPaymentResult
            {
                CheckoutPaymentId = paymentRecord.CheckoutPaymentId,
                IsPaymentSuccessful = result.IsPaymentSuccessful
            };
        }

        private static BankPaymentParameters CreateBankPaymentParameters(CheckoutPaymentParameters paymentParameters)
        {
            BankPaymentParameters bankPaymentParameters = 
                new BankPaymentParameters(paymentParameters.CardNumber, 
                                          paymentParameters.ExpiryMonth, 
                                          paymentParameters.ExpiryYear, 
                                          paymentParameters.Cvv, 
                                          paymentParameters.Amount, 
                                          paymentParameters.Currency);

            return bankPaymentParameters;
        }

        private CheckoutPaymentRecord CreatPaymentObject(CheckoutPaymentParameters paymentParameters, BankPaymentResult result)
        {
            var checkoutPaymentId = Guid.NewGuid();

            // mask card number before storage to help mitigate issue with storing the card number
            MaskCardNumber(paymentParameters);

            return new CheckoutPaymentRecord
            {
                CheckoutPaymentId = checkoutPaymentId,
                BankPaymentId = result.PaymentId?.ToString() ?? "",
                PaymentParameters = paymentParameters,
                PaymentDate = clock.GetCurrentInstant(),
                IsSuccessful = result.IsPaymentSuccessful
            };
        }

        private void MaskCardNumber(CheckoutPaymentParameters pp)
        {
            const int CharctersToKeep = 4;
            if (string.IsNullOrWhiteSpace(pp.CardNumber) || pp.CardNumber.Length < CharctersToKeep)
            {
                // would be nice to add more details to this log, but could lead to senative data being logged
                logger.LogError("Received malformed card number");
                throw new ArgumentException($"Card number should contain at least {CharctersToKeep} characters");
            }
            var maskedLength = pp.CardNumber.Length - 4;
            var lastDigits = pp.CardNumber.Substring(maskedLength);
            var maskedPart = new string('*', maskedLength);
            pp.CardNumber = maskedPart + lastDigits;
        }
    }
}