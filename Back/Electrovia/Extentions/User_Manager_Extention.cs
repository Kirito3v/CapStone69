using Electrovia_Core.Entities.identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Electrovia.Extentions
{
    public static class User_Manager_Extention
    {
        public static async Task<AppUser?> Find_address_Async(this UserManager<AppUser> userManager, ClaimsPrincipal claim_user)
        {
            var email = claim_user.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(u=>u.Email == email);
            return user;
        }
    }
}
