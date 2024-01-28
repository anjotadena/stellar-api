using API.DTO;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregate;

namespace API.Helpers;
public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Product, ProductResponseDto>()
            .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
            .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
        CreateMap<Core.Entities.Address, AddressDto>().ReverseMap();
        CreateMap<CustomerCartDto, CustomerBasket>();
        CreateMap<CartItemDto, BasketItem>();
        CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();
        CreateMap<Order, OrderToReturnDto>();
        CreateMap<OrderItem, OrderItemDto>();
    }
}