using Stripe;
using Electrovia_Core.Entities;
using Electrovia_Core.Services;
using Electrovia_Core.interfaces;
using Microsoft.Extensions.Configuration;
using Electrovia_Core.Entities.Order_Aggregate;
using Electrovia_Core.Specifications.Order_Spec;
using Products = Electrovia_Core.Entities.Products;

namespace Electrovia_Services
{
    public class PaymentService : IPaymentService
    {
        #region Constructors 
        private readonly IConfiguration _conf;
        private readonly ICartRepositories _cartRepo;
        private readonly IUnitOFWork _unitOfWork;
        public PaymentService(IConfiguration conf, ICartRepositories cartRepo, IUnitOFWork unitOfWork)
        {
            _conf = conf;
            _cartRepo = cartRepo;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region CreateOrUpdatePaymentIntent 
        public async Task<Customer_Cart> CreateOrUpdatePaymentIntent(string cartId)
        {
            StripeConfiguration.ApiKey = _conf["StripeSettings:Secret_key"];
            var cart = await _cartRepo.GetCart(cartId);
            if (cart is null) return null!;

            var ShippingPrice = 0m;

            if (cart.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()!.GetbyIdAsync(cart.DeliveryMethodId.Value);
                cart.ShippingCost = deliveryMethod.Cost;
                ShippingPrice = deliveryMethod.Cost;
            }

            if (cart?.Items?.Count > 0)
            {
                foreach (var item in cart.Items)
                {
                    var product = await _unitOfWork.Repository<Products>()!.GetbyIdAsync(item.Id);
                    if (item.price != product.Price)
                        item.price = product.Price;
                }
            }

            PaymentIntent PaymentIntent;
            var service = new PaymentIntentService();
            if (string.IsNullOrEmpty(cart!.PaymentIntentId)) //Create Payment Intent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)cart.Items.Sum(item => item.price * item.quantity * 100) + (long)ShippingPrice * 100,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                PaymentIntent = await service.CreateAsync(options);
                cart.PaymentIntentId = PaymentIntent.Id;
                cart.ClientSecret = PaymentIntent.ClientSecret;
            }
            else //Update Payment Intent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)cart.Items.Sum(item => item.price * item.quantity * 100) + (long)ShippingPrice * 100,
                };
                await service.UpdateAsync(cart.PaymentIntentId, options);
            }
            await _cartRepo.UpdateCart(cart);
            return cart;
        } 

        #endregion

        public async Task<Order> UpdatePaymentIntentToSucceededOrFailed(string paymentintentId, bool isSucceeded)
        {
            var spec = new Order_With_PaymentIntentId_Specification(paymentintentId);
            
            var order = await _unitOfWork.Repository<Order>()!.GetEntityAsync(spec);
            
            if (isSucceeded) order.Order_Status = OrderStatus.PaymentReceived;
            
            else order.Order_Status = OrderStatus.PaymentFailed;
            
            _unitOfWork.Repository<Order>()!.Update(order);
            
            await _unitOfWork.Complete();
            
            return order;
        }
    }
}

