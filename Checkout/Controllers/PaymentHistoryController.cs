using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.PublicDtos;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentHistoryController : Controller
    {
        [HttpGet]
        public CheckoutPaymentParameters Index(Guid paymentId)
        {
            return new CheckoutPaymentParameters();
        }
    }
}