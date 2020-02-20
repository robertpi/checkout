using Checkout.BankServices;
using Checkout.Controllers;
using Checkout.PaymentStorage;
using Checkout.PublicDtos;
using Moq;
using NodaTime;
using NodaTime.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkout.Tests
{
    public class PaymentControllerTests
    {
        private Mock<IBank> bankMock;
        private Mock<IPaymentStorage> paymentStorageMock;
        private PaymentController paymentController;

        [SetUp]
        public void Step() 
        {
            bankMock = new Mock<IBank>();
            paymentStorageMock = new Mock<IPaymentStorage>();
            var testClock = new FakeClock(Instant.FromUtc(2020, 2, 19, 19, 3));
            paymentController = new PaymentController(bankMock.Object, paymentStorageMock.Object, testClock);
        }

        [Test]
        public void If_the_bank_successfully_proccess_the_payment_the_api_should_indicate_this()
        {
            // setup
            var successResult = BankPaymentResult.NewSuccessfulPayment(Guid.NewGuid());
            bankMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentParameters>())).Returns(successResult);
            var paymentParameters = TestDataGenerator.CreateValidPaymentParameters();

            // test
            var result = paymentController.Post(paymentParameters);

            // verify 
            // - payment is saved with masked number
            // - status is correct
            paymentStorageMock.Verify(ps => 
                ps.SavePayment(
                    It.Is<CheckoutPaymentRecord>( x => x.PaymentParameters.CardNumber == TestDataGenerator.MaskedCardNumber)), 
                    Times.Once());
            Assert.IsTrue(result.IsPaymentSuccessful);
        }

        [Test]
        public void If_the_bank_fails_proccess_the_payment_the_api_should_indicate_this()
        {
            // setup
            var successResult = BankPaymentResult.NewFailure();
            bankMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentParameters>())).Returns(successResult);
            var paymentParameters = TestDataGenerator.CreateValidPaymentParameters();

            // test
            var result = paymentController.Post(paymentParameters);

            // verify 
            // - payment is saved with masked number
            // - status is correct
            paymentStorageMock.Verify(ps =>
                ps.SavePayment(
                    It.Is<CheckoutPaymentRecord>(x => x.PaymentParameters.CardNumber == TestDataGenerator.MaskedCardNumber)),
                    Times.Once());
            Assert.IsFalse(result.IsPaymentSuccessful);
        }

        [Test]
        public void Exception_is_thrown_for_malformed_cardnumber()
        {
            // setup
            var successResult = BankPaymentResult.NewFailure();
            bankMock.Setup(x => x.ProcessPayment(It.IsAny<BankPaymentParameters>())).Returns(successResult);
            var paymentParameters = TestDataGenerator.CreateValidPaymentParameters();
            paymentParameters.CardNumber = "";

            // test & assert
            Assert.Throws<ArgumentException>(() => paymentController.Post(paymentParameters));
        }

    }
}
