using Electrovia_Core.Entities.Order_Aggregate;

namespace Electrovia.DTOs
{
    public class OrderDTO
    {
        public string? Cartid { get; set; }
        public int DeliveryMedthodid { get; set; }
        public AddressDTO? Shipping_Address { get; set; }
    }
}
