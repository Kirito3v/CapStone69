using Electrovia_Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electrovia_Core.Services
{
    public interface IOrderServices
    {
        Task<Order> CreateOrderAsync(string User_Email, string cart_id, int Delivery_Medthod_id, Address Shipping_Address);
        Task<IReadOnlyList<Order>> GetOrderForUser(string User_Email);
        Task<Order> GetOrderByIdForUser(string User_Email, int order_id);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethod();


    }
}
