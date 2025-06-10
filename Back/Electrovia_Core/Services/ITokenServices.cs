using Microsoft.AspNetCore.Identity;
using Electrovia_Core.Entities.identity;

namespace Electrovia_Core.Services
{
    public interface ITokenServices
    {
        Task<string> CreateToken(AppUser user, UserManager<AppUser> userManager);
    }
}
