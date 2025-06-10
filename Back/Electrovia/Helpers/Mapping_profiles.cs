using AutoMapper;
using Electrovia.DTOs;
using Electrovia_Core.Entities;
using Electrovia_Core.Entities.identity;
using Electrovia_Core.Entities.Order_Aggregate;
using System.Collections.Concurrent;

namespace Electrovia.Helpers
{
    public class Mapping_profiles : Profile
    {
        public Mapping_profiles()
        {
            CreateMap<Products, ProductDTO>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand!.Name))
                .ForMember(d => d.ProductType,  o => o.MapFrom(s => s.ProductType!.Name))
                .ForMember(d => d.PictureUrl,   o => o.MapFrom<Product_Pic_URL_Resolver>());

            CreateMap<Order, OrderDTO2>()
                .ForMember(d => d.Delivery_Method_Name, o => o.MapFrom(s => s.Delivery_Method!.ShortName))
                .ForMember(d => d.Delivery_Method_Cost, o => o.MapFrom(s => s.Delivery_Method!.Cost))
                .ForMember(d => d.Total,                o => o.MapFrom(s => s.Get_Total()));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product_Order!.ProductName))
                .ForMember(d => d.Productid,   o => o.MapFrom(s => s.Product_Order!.Productid))
                .ForMember(d => d.PictureUrl,  o => o.MapFrom(s => s.Product_Order!.PictureUrl))
                .ForMember(d => d.PictureUrl,  o => o.MapFrom<Order_Pic_URL_Resolver>());

            CreateMap<Electrovia_Core.Entities.identity.Address, AddressDTO>().ReverseMap();
            CreateMap<AddressDTO, Electrovia_Core.Entities.Order_Aggregate.Address>();
            CreateMap<Customer_Cart_DTO, Customer_Cart>();
            CreateMap<Cart_item_DTO, Cart_item>();


        }
    }
}
