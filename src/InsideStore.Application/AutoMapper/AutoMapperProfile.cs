using AutoMapper;
using InsideStore.Domain.DTO.Request.Product;
using InsideStore.Domain.DTO.Response.Order;
using InsideStore.Domain.DTO.Response.Product;
using InsideStore.Domain.Entities;

namespace InsideStore.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateProductRequest, Product>();
        CreateMap<Product, ProductResponse>();
        
        CreateMap<OrderItem, OrderItemResponse>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal));

        CreateMap<Order, OrderResponse>()
            //.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
    }
}