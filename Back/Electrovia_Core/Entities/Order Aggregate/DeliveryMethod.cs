using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electrovia_Core.Entities.Order_Aggregate
{
    public class DeliveryMethod : BaseEntity
    {
        #region Constructor
        public DeliveryMethod()
        {

        }
        public DeliveryMethod(string? shortName, string? description, string? time, decimal cost)
        {
            ShortName = shortName;
            Description = description;
            Time = time;
            Cost = cost;
        }

        #endregion  

        public string? ShortName { get; set; }
        public string? Description { get; set; }
        public string? Time { get; set; }
        public decimal Cost { get; set; }
    }
}
