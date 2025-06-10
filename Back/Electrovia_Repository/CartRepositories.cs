using System.Text.Json;
using StackExchange.Redis;
using Electrovia_Core.Entities;
using Electrovia_Core.interfaces;

namespace Electrovia_Repository
{
    public class CartRepositories : ICartRepositories
    {
        private readonly IDatabase _database;
        public CartRepositories(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }


        public async Task<bool> DeleteCart(string Cart_id) => await _database.KeyDeleteAsync(Cart_id);
       
        
        public async Task<Customer_Cart> GetCart(string Cart_id)
        {
            var cart = await _database.StringGetAsync(Cart_id);
             return cart.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Customer_Cart>(cart);
        }

        public async Task<Customer_Cart> UpdateCart(Customer_Cart Cus_Cart)
        {
            var create_Update = await _database.StringSetAsync(Cus_Cart.Id, JsonSerializer.Serialize(Cus_Cart), TimeSpan.FromDays(15));
            if (create_Update == false) return null!;
            return await GetCart(Cus_Cart.Id!);
        }
    }
}
