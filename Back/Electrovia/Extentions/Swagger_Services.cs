namespace Electrovia.Extentions
{
    public static class Swagger_Services
    {
        public static IServiceCollection Add_SwaggerServices(this IServiceCollection services)
        { 
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        public static WebApplication UseSwagger_Middlewares(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
