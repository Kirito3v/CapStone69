
namespace Electrovia_Core.Entities
{
    public class Customer_Cart
    {
        public string? Id { get; set; }
        public Customer_Cart(string id )
        {
            Id = id;
        }
        public List<Cart_item> Items { get; set; } = new List<Cart_item>();
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId  { get; set; }
        public decimal ShippingCost { get; set; }
    } 
}
