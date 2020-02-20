using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.PaymentStorage;
using Checkout.PublicDtos;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentHistoryController : Controller
    {
        private readonly IPaymentStorage paymentStorage;

        public PaymentHistoryController(IPaymentStorage paymentStorage)
        {
            this.paymentStorage = paymentStorage;
        }

        [HttpGet]
        public ActionResult<CheckoutPaymentRecord> Index(Guid checkoutPaymentId)
        {
            var record = paymentStorage.GetPayment(checkoutPaymentId);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(record);
        }
    }
}