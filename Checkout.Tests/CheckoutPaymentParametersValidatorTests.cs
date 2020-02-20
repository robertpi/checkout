using Checkout.Validation;
using NodaTime;
using NodaTime.Testing;
using NUnit.Framework;
using FluentValidation.TestHelper;
using Checkout.PublicDtos;

namespace Checkout.Tests
{
    public class Tests
    {
        private CheckoutPaymentParameters targetPaymentParameters;
        private CheckoutPaymentParametersValidator validator;

        [SetUp]
        public void Setup()
        {
            targetPaymentParameters = TestDataGenerator.CreateValidPaymentParameters();
            var testClock = new FakeClock(Instant.FromUtc(2020, 2, 19, 19, 3));
            validator = new CheckoutPaymentParametersValidator(testClock);
        }

        [Test]
        public void CheckoutPaymentParameters_valid_object_should_have_no_validaton_errors()
        {
            var result = validator.TestValidate(targetPaymentParameters);
            result.ShouldNotHaveAnyValidationErrors();
        }


        [Test]
        public void CardNumber_should_not_be_null()
        {
            targetPaymentParameters.CardNumber = null;
            var result = validator.TestValidate(targetPaymentParameters);
            result.ShouldHaveValidationErrorFor(pp => pp.CardNumber);
        }

        [Test]
        public void CardNumber_should_contain_non_numeric_characters()
        {
            targetPaymentParameters.CardNumber = "mslqkjfdklqs";
            var result = validator.TestValidate(targetPaymentParameters);
            result.ShouldHaveValidationErrorFor(pp => pp.CardNumber);
        }

        [Test]
        public void CardNumber_needs_to_be_a_valid_formular()
        {
            targetPaymentParameters.CardNumber = "2348747897437594";
            var result = validator.TestValidate(targetPaymentParameters);
            result.ShouldHaveValidationErrorFor(pp => pp.CardNumber);
        }

        [Test]
        public void Cvv_should_not_be_null()
        {
            targetPaymentParameters.Cvv = null;
            var result = validator.TestValidate(targetPaymentParameters);
            result.ShouldHaveValidationErrorFor(pp => pp.Cvv);
        }

        [Test]
        public void Cvv_should_not_contain_non_numeric_characters()
        {
            targetPaymentParameters.Cvv = "ABC";
            var result = validator.TestValidate(targetPaymentParameters);
            result.ShouldHaveValidationErrorFor(pp => pp.Cvv);
        }

        [Test]
        public void ExpiryMonth_invalid_month_number_should_fail()
        {
            targetPaymentParameters.ExpiryMonth = -1;
            var result = validator.TestValidate(targetPaymentParameters);
            result.ShouldHaveValidationErrorFor(pp => pp.ExpiryMonth);
        }

        [Test]
        public void ExpiryYear_date_in_past_should_fail()
        {
            targetPaymentParameters.ExpiryYear = 2019;
            var result = validator.TestValidate(targetPaymentParameters);
            result.ShouldHaveValidationErrorFor(pp => pp.ExpiryYear);
        }

        [Test]
        public void Amount_negative_amount_should_fail()
        {
            targetPaymentParameters.Amount = -12;
            var result = validator.TestValidate(targetPaymentParameters);
            result.ShouldHaveValidationErrorFor(pp => pp.Amount);
        }

        [Test]
        public void Currency_should_not_be_null()
        {
            targetPaymentParameters.Currency = null;
            var result = validator.TestValidate(targetPaymentParameters);
            result.ShouldHaveValidationErrorFor(pp => pp.Currency);
        }

        [Test]
        public void Currency_more_than_three_letters_should_fail()
        {
            targetPaymentParameters.Currency = "ABCD";
            var result = validator.TestValidate(targetPaymentParameters);
            result.ShouldHaveValidationErrorFor(pp => pp.Currency);
        }

        [Test]
        public void Currency_none_word_characters_should_fail()
        {
            targetPaymentParameters.Currency = "123";
            var result = validator.TestValidate(targetPaymentParameters);
            result.ShouldHaveValidationErrorFor(pp => pp.Currency);
        }
    }
}