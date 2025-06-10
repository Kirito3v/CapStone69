using System.ComponentModel.DataAnnotations;

namespace Electrovia.DTOs
{
    public class Cart_item_DTO
    {
        [Required]
        public int id { get; set; }

        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? Description { get; set; }
        
        [Required]
        public string? Brand { get; set; }
        
        [Required]
        public string? Type { get; set; }
        
        [Required]
        public string? PictureURL { get; set; }
        
        
        [Required]
        [Range(1 , int.MaxValue,ErrorMessage ="Price Must Be Greater Than Zero!!")]
        public int quantity { get; set; }
        
        
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Quantity Must Be One Item At Least")]
        public decimal price { get; set; }
    }
}