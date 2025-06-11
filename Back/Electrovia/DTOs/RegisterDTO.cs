using System.ComponentModel.DataAnnotations;

namespace Electrovia.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string? DisplayName { get; set; }
        
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        
        [Required]
        public string? Password { get; set; }
        
        [Required]
        [Phone]
        public string? phone { get; set; }
        
      

    }
}
