using AutoMapper;
using InsideStore.Domain.DTO.Request.Product;
using InsideStore.Domain.DTO.Response.Product;
using InsideStore.Domain.Entities;

namespace InsideStore.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateProductRequest, Product>();
        CreateMap<Product, AllProductResponse>();
    }
}