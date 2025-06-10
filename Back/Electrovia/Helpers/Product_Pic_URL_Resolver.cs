using AutoMapper;
using Electrovia.DTOs;
using Electrovia_Core.Entities;

namespace Electrovia.Helpers
{
    public class Product_Pic_URL_Resolver : IValueResolver<Products, ProductDTO, string>
    {
        private readonly IConfiguration _configuration;
        public Product_Pic_URL_Resolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Products source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["ApiBaseURL"]}{source.PictureUrl}";
            else
                return string.Empty;
        }
    }
}