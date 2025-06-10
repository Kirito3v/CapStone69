
using Electrovia_Core.Entities.identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Electrovia_Repository.Identity
{
    public class App_identity_DbContext : IdentityDbContext<AppUser>
    {
        public App_identity_DbContext(DbContextOptions<App_identity_DbContext> options) : base(options)
        {
        }
    }
}
