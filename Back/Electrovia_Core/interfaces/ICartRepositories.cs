using Electrovia_Core.Entities;


namespace Electrovia_Core.interfaces
{
    public interface ICartRepositories
    {
        Task<Customer_Cart> GetCart (string Cart_id);
        Task<Customer_Cart> UpdateCart (Customer_Cart Cus_Cart);
        Task<bool> DeleteCart(string Cart_id);

    }
}
