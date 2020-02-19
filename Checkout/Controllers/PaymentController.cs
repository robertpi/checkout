using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.PublicDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : Controller
    {
        [HttpPost]
        public CheckoutPaymentResult Post([FromBody]CheckoutPaymentParameters parameters)
        {
            return new CheckoutPaymentResult
            {
                PaymentId = Guid.NewGuid(),
                IsPaymentSuccessful = true
            };
        }
    }
}