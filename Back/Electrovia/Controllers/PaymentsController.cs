using Stripe;
using Electrovia.DTOs;
using Electrovia.Errors;
using Microsoft.AspNetCore.Mvc;
using Electrovia_Core.Services;
using Microsoft.AspNetCore.Authorization;
using Electrovia_Core.Entities.Order_Aggregate;


namespace Electrovia.Controllers
{
    public class PaymentsController : BaseApiController
    {
        #region Constructor 
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentsController> _logger;
        private const string _WhSecret = "whsec_085ab30bb1ea0574c5cfa8f93b4db184863b116ed7ff54f1a36989f751991b5f";
        public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        #endregion

        #region CreateOrUpdatePaymentIntent 
        [Authorize]
        [ProducesResponseType(typeof(Customer_Cart_DTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIsResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{cartId}")]
        public async Task<ActionResult<Customer_Cart_DTO>> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await _paymentService.CreateOrUpdatePaymentIntent(cartId);
            if (cart is null) return BadRequest(new APIsResponse(400, "A Problem With Your Cart "));
            return Ok(cart);
        }

        #endregion

        #region StripeWebhook 

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            Order order;
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _WhSecret);
            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

            switch (stripeEvent.Type)
            {
                case EventTypes.PaymentIntentSucceeded: 
                    order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, true);
                    _logger.LogInformation("Payment is Succeeded ya Hamada", paymentIntent.Id);
                    break;

                case EventTypes.PaymentIntentPaymentFailed:
                    order = await _paymentService.UpdatePaymentIntentToSucceededOrFailed(paymentIntent.Id, false);
                    _logger.LogInformation("Payment is Failed ya Hamada :(", paymentIntent.Id);
                    break;
            }
            return Ok();
        }

        #endregion 
    }
}
