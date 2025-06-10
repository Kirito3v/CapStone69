namespace Electrovia_Core.Entities.identity
{
    public class Address
       {
        public int id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? User { get; set; }
    }
}