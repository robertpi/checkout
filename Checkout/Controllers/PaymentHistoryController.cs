using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.PaymentStorage;
using Checkout.PublicDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Checkout.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentHistoryController : Controller
    {
        private readonly IPaymentStorage paymentStorage;
        private readonly ILogger logger;

        public PaymentHistoryController(IPaymentStorage paymentStorage, ILogger<PaymentHistoryController> logger)
        {
            this.paymentStorage = paymentStorage;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<CheckoutPaymentRecord> Index(Guid checkoutPaymentId)
        {
            var record = paymentStorage.GetPayment(checkoutPaymentId);

            if (record == null)
            {
                logger.LogWarning($"Couldn't find payment associated with requested checkoutPaymentId: {checkoutPaymentId}");
                return NotFound();
            }

            return Ok(record);
        }
    }
}