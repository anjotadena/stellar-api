using API.DTO;
using AutoMapper;
using Core.Entities;

namespace API.Helpers;

public class ProductUrlResolver : IValueResolver<Product, ProductResponseDto, string>
{
    private readonly IConfiguration _configuration;

    public ProductUrlResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string Resolve(Product source, ProductResponseDto destination, string destMember, ResolutionContext context)
    {
        if (!string.IsNullOrEmpty(source.PictureUrl))
        {
            return $"{_configuration["ApiUrl"]}/{source.PictureUrl}";
        }

        return null;
    }
}