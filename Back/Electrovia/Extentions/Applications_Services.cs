using Electrovia.Errors;
using Electrovia.Helpers;
using Electrovia_Repository;
using Microsoft.AspNetCore.Mvc;
using Electrovia_Core.interfaces;
using Electrovia_Core.Services;
using Electrovia_Services;

namespace Electrovia.Extentions
{
    public static class Applications_Services
    {
        public static IServiceCollection Add_Appliction_Services(this IServiceCollection services)
        {
           
            services.AddAutoMapper(typeof(Mapping_profiles)); 
            services.AddScoped(typeof(IUnitOFWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IOrderServices), typeof(OrderServices));
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            services.AddScoped(typeof(ICartRepositories), typeof(CartRepositories));
            services.AddSingleton(typeof(IResponseCacheService), typeof(ResponseCacheServices));

            services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = (action) =>
                {
                    var erros = action.ModelState.Where(P => P.Value!.Errors.Count() > 0)
                                                 .SelectMany(P => P.Value!.Errors)
                                                 .Select(P => P.ErrorMessage)
                                                 .ToArray();
                    var validationErrorResponse = new API_ValidationError_Response()
                    {
                        Errors = erros
                    };
                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });  
            

            return services;
        }
    }
}
