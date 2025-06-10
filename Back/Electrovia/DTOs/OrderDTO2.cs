using Electrovia_Core.Entities.Order_Aggregate;

namespace Electrovia.DTOs
{
    public class OrderDTO2
    {
        public int id { get; set; }
        public string? User_Email { get; set; }
        public DateTimeOffset Order_Date { get; set; }
        public string? Order_Status { get; set; } 
        public Address? Shipping_Address { get; set; }
        public string? Delivery_Method_Name { get; set; }
        public decimal Delivery_Method_Cost { get; set; }
        public ICollection<OrderItemDTO>? Order_Items { get; set; }
        public decimal SubTotal { get; set; }
        public string Payment_intent_Id { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}
