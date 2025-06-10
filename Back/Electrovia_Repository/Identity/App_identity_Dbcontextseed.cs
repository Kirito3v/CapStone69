using System.Text.Json;
using Electrovia_Core.Entities;
using Electrovia_Core.Entities.identity;
using Microsoft.AspNetCore.Identity;

namespace Electrovia_Repository.Identity
{
    public static class App_identity_Dbcontextseed
    {
        public static async Task IdentitySeedAsync(UserManager<AppUser> users)
        {
            if (!users.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Abdullah Mohamed",
                    Email = "Abdullah5@gmail.com",
                    UserName = "abdullah.bebo",
                    PhoneNumber = "01224316785",
                };
                    await users.CreateAsync(user, "Pas$$w0rd");
            }
        }
    }
}
