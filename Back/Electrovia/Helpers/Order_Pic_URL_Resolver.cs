using AutoMapper;
using Electrovia.DTOs;
using Electrovia_Core.Entities.Order_Aggregate;

namespace Electrovia.Helpers
{
    public class Order_Pic_URL_Resolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;
        public Order_Pic_URL_Resolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product_Order!.PictureUrl))
                return $"{_configuration["ApiBaseURL"]}{source.Product_Order.PictureUrl}";
            else
                return string.Empty;
        }
    }
}