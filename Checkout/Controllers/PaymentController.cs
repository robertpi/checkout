﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.BankServices;
using Checkout.PaymentStorage;
using Checkout.PublicDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace Checkout.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : Controller
    {
        private readonly IBank bank;
        private readonly IPaymentStorage paymentStorage;
        private readonly IClock clock;

        public PaymentController(IBank bank, IPaymentStorage paymentStorage, IClock clock)
        {
            this.bank = bank;
            this.paymentStorage = paymentStorage;
            this.clock = clock;
        }

        [HttpPost]
        public CheckoutPaymentResult Post([FromBody]CheckoutPaymentParameters pp)
        {
            var bankPaymentParameters =
                new BankPaymentParameters(pp.CardNumber, pp.ExpiryMonth, pp.ExpiryYear, pp.Cvv, pp.Amount, pp.Currency);

            var result = bank.ProcessPayment(bankPaymentParameters);

            var paymentRecord = CreatPaymentObject(pp, result);
            paymentStorage.SavePayment(paymentRecord);

            return new CheckoutPaymentResult
            {
                CheckoutPaymentId = paymentRecord.CheckoutPaymentId,
                IsPaymentSuccessful = result.IsPaymentSuccessful
            };
        }

        private CheckoutPaymentRecord CreatPaymentObject(CheckoutPaymentParameters pp, BankPaymentResult result)
        {
            var checkoutPaymentId = Guid.NewGuid();

            // mask card number before storage to help mitigate issue with storing the card number
            MaskCardNumber(pp);

            return new CheckoutPaymentRecord
            {
                CheckoutPaymentId = checkoutPaymentId,
                BankPaymentId = result.PaymentId?.ToString(),
                PaymentParameters = pp,
                PaymentDate = clock.GetCurrentInstant(),
                IsSuccessful = result.IsPaymentSuccessful
            };
        }

        private static void MaskCardNumber(CheckoutPaymentParameters pp)
        {
            const int CharctersToKeep = 4;
            if (pp.CardNumber.Length < CharctersToKeep)
            {
                throw new ArgumentException($"Card number should contain at least {CharctersToKeep} characters");
            }
            var maskedLength = pp.CardNumber.Length - 4;
            var lastDigits = pp.CardNumber.Substring(maskedLength);
            var maskedPart = new string('*', maskedLength);
            pp.CardNumber = maskedPart + lastDigits;
        }
    }
}