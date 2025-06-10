using Electrovia_Repository;
using Electrovia_Core.Entities.Order_Aggregate;

namespace Electrovia_Core.Specifications.Order_Spec
{
    public class Order_Specifications : BaseSpecification<Order>
    {
        public Order_Specifications(string email) : base(O => O.User_Email == email)
        {
            include.Add(O => O.Delivery_Method!);
            include.Add(O => O.Order_Items);
            Add_OrederByDesc(O => O.Order_Date);
        }
        public Order_Specifications(string email , int orderId ) : base(O => O.User_Email == email && O.Id == orderId)
        {
            include.Add(O => O.Delivery_Method!);
            include.Add(O => O.Order_Items);
        }
    }
}
