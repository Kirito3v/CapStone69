using Electrovia_Core.Entities.Order_Aggregate;
using Electrovia_Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electrovia_Core.Specifications.Order_Spec
{
    public class Order_With_PaymentIntentId_Specification : BaseSpecification<Order>
    {
        public Order_With_PaymentIntentId_Specification(string paymentIntentId) :base (O => O.Payment_intent_Id ==  paymentIntentId)
        {
        }
    }
}
