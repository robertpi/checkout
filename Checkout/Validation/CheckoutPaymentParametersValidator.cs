using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.PublicDtos;
using FluentValidation;
using NodaTime;

namespace Checkout.Validation
{
    public class CheckoutPaymentParametersValidator: AbstractValidator<CheckoutPaymentParameters>
    {
        private readonly IClock clock;

        public CheckoutPaymentParametersValidator(IClock clock)
        {
            this.clock = clock;
            RuleFor(pp => pp.CardNumber).NotNull();
            RuleFor(pp => pp.CardNumber).CreditCard();
            RuleFor(pp => pp.Cvv).NotNull();
            RuleFor(pp => pp.Cvv).Length(3, 4); // most have 3 digits, but amex has 4
            RuleFor(pp => pp.Cvv).Matches(@"\d+");
            RuleFor(pp => pp.ExpiryMonth).InclusiveBetween(1, 12);
            RuleFor(pp => pp.ExpiryYear).Must(DateIsInFuture);
            RuleFor(pp => pp.Amount).GreaterThan(0M);
            RuleFor(pp => pp.Currency).NotNull();
            RuleFor(pp => pp.Currency).Length(3);
            RuleFor(pp => pp.Currency).Matches(@"[A-Za-z]+");
            // a good additional validation rule would be to validate against a list
            // of ISO currency string
        }

        bool DateIsInFuture(CheckoutPaymentParameters paymentParameters, int expiryYear) 
        {
            if (1 > paymentParameters.ExpiryMonth || paymentParameters.ExpiryMonth > 12)
            {
                return false;
            }

            // assume credit card expires after the last day of the given month / year
            var days = CalendarSystem.Iso.GetDaysInMonth(expiryYear, paymentParameters.ExpiryMonth);
            var expiryDate = Instant.FromUtc(expiryYear, paymentParameters.ExpiryMonth, days, 23, 59);

            var now = clock.GetCurrentInstant();
            
            return expiryDate > now;
        }
    }
}
