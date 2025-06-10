using System.Runtime.Serialization;

namespace Electrovia_Core.Entities.Order_Aggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Payment Received")]
        PaymentReceived,
        
        [EnumMember(Value = "Payment Failed")]
        PaymentFailed
    }
}
