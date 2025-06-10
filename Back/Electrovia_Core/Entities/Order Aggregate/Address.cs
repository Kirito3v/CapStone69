using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Electrovia_Core.Entities.Order_Aggregate
{
    public class Address
    {
        #region Constructor 
        public Address()
        {

        }
        public Address(string? firstName, string? lastName, string? street, string? city, string? country)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            City = city;
            Country = country;
        } 
        #endregion

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }                
}
