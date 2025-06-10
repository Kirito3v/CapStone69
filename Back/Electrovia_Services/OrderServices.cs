using Electrovia_Core.Entities;
using Electrovia_Core.Entities.Order_Aggregate;
using Electrovia_Core.interfaces;
using Electrovia_Core.Services;
using Electrovia_Core.Specifications.Order_Spec;

namespace Electrovia_Services
{
    public class OrderServices : IOrderServices
    {
        #region Constructor 
        private readonly ICartRepositories _cart_Repo;
        private readonly IUnitOFWork _unitOFWork;
        private readonly IPaymentService _PaymentService;

        public OrderServices(ICartRepositories cart_Repo, IUnitOFWork unitOFWork, IPaymentService paymentService)
        {
            _cart_Repo = cart_Repo;
            _unitOFWork = unitOFWork;
            _PaymentService = paymentService;
        }
        #endregion

        #region Create_order 
        public async Task<Order> CreateOrderAsync(string User_Email, string cart_id, int Delivery_Medthod_id, Address Shipping_Address)
        {
            // get cart from cart repo
            var cart = await _cart_Repo.GetCart(cart_id);

            // select items from at cart from product repo 
            var OrderItems = new List<OrderItem>();
            foreach (var item in cart.Items)
            {
                var product = await _unitOFWork.Repository<Products>()!.GetbyIdAsync(item.Id);
                var pro_order = new Product_Order(product.Id, product.Name, product.PictureUrl);
                var orderitem = new OrderItem(pro_order, product.Price, item.quantity);
                OrderItems.Add(orderitem);
            }

            // calculate subtotal
            var subtotal = OrderItems.Sum(item => item.Price * item.Quantity);

            // get Delivery method from DeliveryMethod Repo
            var deliverymethod = await _unitOFWork.Repository<DeliveryMethod>()!.GetbyIdAsync(Delivery_Medthod_id);

            // Create Order 
            var spec = new Order_With_PaymentIntentId_Specification(cart.PaymentIntentId!);
            var existingOrder = await _unitOFWork.Repository<Order>()!.GetEntityAsync(spec);
            if (existingOrder is not null)
            {
                _unitOFWork.Repository<Order>()!.Delete(existingOrder);
                await _PaymentService.CreateOrUpdatePaymentIntent(cart.Id!);
            }

            var order = new Order(User_Email, Shipping_Address, deliverymethod, OrderItems, subtotal, cart.PaymentIntentId!);
            await _unitOFWork.Repository<Order>()!.AddAsync(order);

            // Save to DataBase 
            var result = await _unitOFWork.Complete();
            if (result > 0) return order;
            return null!;
        }
        
        #endregion

        public async Task<IReadOnlyList<Order>> GetOrderForUser(string User_Email)
        {
            var spec = new Order_Specifications(User_Email);
            var orders = await _unitOFWork.Repository<Order>()!.GetAllAsync(spec);
            return orders;
        }

        public async Task<Order> GetOrderByIdForUser(string User_Email, int order_id)
        {
            var spec = new Order_Specifications(User_Email, order_id);
            var orders = await _unitOFWork.Repository<Order>()!.GetEntityAsync(spec);
            return orders;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethod()
        {
            var deliverymethod = await _unitOFWork.Repository<DeliveryMethod>()!.GetAllAsync();
            return deliverymethod;
        }

    }
}
