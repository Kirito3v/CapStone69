using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electrovia_Core.Entities
{
    public class Products : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductBrandId { get; set; }  // Foreign_key
        public int ProductTypeId { get; set; }   // Foreign_key
        public ProductBrand? ProductBrand { get; set; } // Navigational property[one]
        public ProductType? ProductType { get; set; }   // Navigational property[one]
    }
}
