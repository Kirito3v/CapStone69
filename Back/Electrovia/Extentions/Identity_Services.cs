using System.Text;
using Electrovia_Services;
using Electrovia_Core.Services;
using Microsoft.AspNetCore.Identity;
using Electrovia_Repository.Identity;
using Electrovia_Core.Entities.identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Electrovia.Extentions
{
    public static class Identity_Services
    {
        public static IServiceCollection Add_Identity_Services(this IServiceCollection services, IConfiguration _conf)
        {

            services.AddScoped<ITokenServices, TokenServices>();
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<App_identity_DbContext>();
           
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme   = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = _conf["JWT:Validissuer"],

                    ValidateAudience = true,
                    ValidAudience = _conf["JWT:Validaudience"],

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf["JWT:key"]!))
                };
            });

            return services;
        }
    }
}
