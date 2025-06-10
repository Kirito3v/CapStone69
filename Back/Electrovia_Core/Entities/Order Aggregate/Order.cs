
namespace Electrovia_Core.Entities.Order_Aggregate
{
    public class Order :BaseEntity
    {
        #region Constructor 
        public Order()
        {
            
        }
        public Order(string user_Email, Address shipping_Address, DeliveryMethod delivery_Method, ICollection<OrderItem> order_Items, decimal subTotal, string PaymentIntentid)
        {
            User_Email = user_Email;
            Shipping_Address = shipping_Address;
            Delivery_Method = delivery_Method;
            Order_Items = order_Items;
            SubTotal = subTotal;
            Payment_intent_Id = PaymentIntentid;
        } 

        #endregion

        public string? User_Email { get; set; }
        public DateTimeOffset Order_Date { get; set; } = DateTimeOffset.Now;
        public OrderStatus Order_Status { get; set; } = OrderStatus.Pending;
        public Address? Shipping_Address { get; set; }
        public DeliveryMethod? Delivery_Method { get; set; }
        public ICollection<OrderItem> Order_Items { get; set; } = new HashSet<OrderItem>();
        public decimal SubTotal { get; set; }
        public decimal Get_Total() => SubTotal + Delivery_Method!.Cost;
        public string? Payment_intent_Id { get; set; }  
    }
}