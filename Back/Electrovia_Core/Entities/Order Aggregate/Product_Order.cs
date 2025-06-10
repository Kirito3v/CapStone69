using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electrovia_Core.Entities.Order_Aggregate
{
    public class Product_Order
    {
        #region Constructor 
        public Product_Order()
        {
            
        }
        public Product_Order(int productid, string? productName, string? pictureUrl)
        {
            Productid = productid;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }
        #endregion 

        public int Productid { get; set; }
        public string? ProductName { get; set; }
        public string? PictureUrl { get; set; }
    }
}
