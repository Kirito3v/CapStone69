using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electrovia_Core.Entities
{
    public class Cart_item : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public string? Type { get; set; }
        public string? PictureURL { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
    }
}
