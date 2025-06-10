using System.Text;
using System.Security.Claims;
using Electrovia_Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Electrovia_Core.Entities.identity;
using Microsoft.Extensions.Configuration;

namespace Electrovia_Services
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration _conf;
        public TokenServices(IConfiguration conf)
        {
            _conf = conf;
        }

        public async Task<string> CreateToken(AppUser user, UserManager<AppUser> userManager)
        {
            //private claims[user_defined]
            var auth_claims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, user.DisplayName!),
                new Claim(ClaimTypes.Email, user.Email!)
            };
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
                auth_claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["JWT:key"]!));

            var token = new JwtSecurityToken(
                 // pyload (Register & private )
                 issuer: _conf["JWT:Validissuer"],
                 audience: _conf["JWT:Validaudience"],
                 expires: DateTime.Now.AddDays(double.Parse(_conf["JWT:Duration"]!)),
                 claims: auth_claims,

                 signingCredentials: new SigningCredentials(key,SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
