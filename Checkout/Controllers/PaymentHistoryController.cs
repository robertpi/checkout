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
        private readonly IPaymentRepository paymentRepository;
        private readonly ILogger logger;

        public PaymentHistoryController(IPaymentRepository paymentRepository, ILogger<PaymentHistoryController> logger)
        {
            this.paymentRepository = paymentRepository;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<CheckoutPaymentRecord> Index(Guid checkoutPaymentId)
        {
            var record = paymentRepository.GetPayment(checkoutPaymentId);

            if (record == null)
            {
                logger.LogWarning($"Couldn't find payment associated with requested checkoutPaymentId: {checkoutPaymentId}");
                return NotFound();
            }

            return Ok(record);
        }
    }
}