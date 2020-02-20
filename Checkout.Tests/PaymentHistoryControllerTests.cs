using Checkout.BankServices;
using Checkout.Controllers;
using Checkout.PaymentStorage;
using Checkout.PublicDtos;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NodaTime;
using NodaTime.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Tests
{
    public class PaymentHistoryControllerTests
    {
        private Mock<IPaymentStorage> paymentStorageMock;
        private PaymentHistoryController paymentHistoryController;
        private FakeClock testClock;

        [SetUp]
        public void Step()
        {
            paymentStorageMock = new Mock<IPaymentStorage>();
            paymentHistoryController = new PaymentHistoryController(paymentStorageMock.Object);
            testClock = new FakeClock(Instant.FromUtc(2020, 2, 19, 19, 3));
        }

        [Test]
        public void If_the_payment_id_is_available_result_should_be_ok()
        {
            // setup
            var paymentId = Guid.NewGuid();
            var paymentParameters = TestDataGenerator.CreateValidPaymentParameters();
            var paymentRecord = new CheckoutPaymentRecord()
            {
                CheckoutPaymentId = paymentId,
                BankPaymentId = Guid.NewGuid().ToString(),
                PaymentParameters = paymentParameters,
                PaymentDate = testClock.GetCurrentInstant(),
                IsSuccessful = true
            };

            paymentStorageMock.Setup(ps => ps.GetPayment(It.Is<Guid>(x => x == paymentId))).Returns(paymentRecord);

            // test
            var actionResult = paymentHistoryController.Index(paymentId);

            // verify 
            Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
            var httpResult = (OkObjectResult)actionResult.Result;
            Assert.IsNotNull(httpResult.Value);
            var result = (CheckoutPaymentRecord)httpResult.Value;
            Assert.AreEqual(paymentId, result.CheckoutPaymentId);
        }

        [Test]
        public void If_the_payment_id_is_not_available_result_should_be_not_found()
        {
            // setup
            var paymentId = Guid.NewGuid();

            // test
            var result = paymentHistoryController.Index(paymentId);

            // verify 
            Assert.IsAssignableFrom<NotFoundResult>(result.Result);
            Assert.IsNull(result.Value);
        }

    }
}
