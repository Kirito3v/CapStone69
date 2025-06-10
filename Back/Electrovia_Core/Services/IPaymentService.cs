using Electrovia_Core.Entities;
using Electrovia_Core.Entities.Order_Aggregate;

namespace Electrovia_Core.Services
{
    public interface IPaymentService
    {
        Task<Customer_Cart> CreateOrUpdatePaymentIntent(string cartId);
        Task<Order> UpdatePaymentIntentToSucceededOrFailed(string paymentintentId, bool isSucceeded);
    }
}
