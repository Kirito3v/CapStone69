using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electrovia_Core.Entities.Order_Aggregate
{
    public class OrderItem : BaseEntity
    {
        #region Constructor 
        public OrderItem()
        {
            
        }
        public OrderItem(Product_Order product_Order, decimal price, int quantity)
        {
            Product_Order = product_Order;
            Price = price;
            Quantity = quantity;
        }

        #endregion

        public Product_Order? Product_Order { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
