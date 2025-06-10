using Electrovia_Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Electrovia.DTOs
{
    public class Customer_Cart_DTO
    {
        [Required]
        public string? Id { get; set; }
        public List<Cart_item_DTO>? Items { get; set; } 
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingCost { get; set; }
    }
}
