using Microsoft.AspNetCore.Identity;


namespace Electrovia_Core.Entities.identity
{
    public class AppUser : IdentityUser
    {
        public string? DisplayName { get; set; }
        public Address? Address { get; set; }
    }
}
