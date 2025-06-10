using System.ComponentModel.DataAnnotations;

namespace Electrovia.DTOs
{
    public class AddressDTO
    {

        [Required]
        public string? FirstNAme { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? street { get; set; }

        [Required]
        public string? city { get; set; }

        [Required]
        public string? country { get; set; }
    }
}
