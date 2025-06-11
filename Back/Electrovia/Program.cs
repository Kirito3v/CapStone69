using StackExchange.Redis;
using Electrovia.Extentions;
using Electrovia.Middlewars; 
using Electrovia_Repository;
using Electrovia_Repository.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Electrovia_Repository.Identity;
using Electrovia_Core.Entities.identity;
using ELECTROVIA_AI;

namespace Electrovia
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var PythonloggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole(); // Adds console logging
            });


            Logger<PyTimeService> Pythonlogger = new Logger<PyTimeService>(PythonloggerFactory);

            PyTimeService pyTimeService = new PyTimeService(Pythonlogger);

            #region Add services to the container.  

            builder.Services.AddControllers();
            builder.Services.Add_SwaggerServices();
            builder.Services.Add_Appliction_Services();
            builder.Services.Add_Identity_Services(builder.Configuration);
            builder.Services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<App_identity_DbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Identity")));
            
            builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
            {
                var con = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("RedisConnection")!);
                return ConnectionMultiplexer.Connect(con);
            });

            builder.Services.AddSingleton(pyTimeService);

            builder.Services.AddSingleton<SearchWithImageService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });

            #endregion
             
            var app = builder.Build();

            #region Auto_Migration
            
            using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();
            try
            {
                var Context = service.GetRequiredService<StoreDbContext>(); // Ask Explicity 
                await Context.Database.MigrateAsync();
                await StoreDbContaxtSeed.SeedAsync(Context);
                
                var identity_Context = service.GetRequiredService<App_identity_DbContext>(); // Ask Explicity 
                await identity_Context.Database.MigrateAsync();
                var user = service.GetRequiredService<UserManager<AppUser>>();
                await App_identity_Dbcontextseed.IdentitySeedAsync(user);

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error occured During Applying Migration");
            }
            #endregion

            #region Configure the HTTP request pipeline.  

            app.UseMiddleware<ExecptionMiddleWare>();

            if (app.Environment.IsDevelopment())
                app.UseSwagger_Middlewares();
            
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors("MyPolicy");
            #endregion 

            app.Run();
        }
    }
}
