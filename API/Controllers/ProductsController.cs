using API.DTO;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController : ApiBaseController
{
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<ProductBrand> _productBrandRepository;
    private readonly IGenericRepository<ProductType> _productTypeRepository;
    private readonly IMapper _mapper;

    public ProductsController(
        IGenericRepository<Product> productRepository,
        IGenericRepository<ProductBrand> productBrandRepository,
        IGenericRepository<ProductType> productTypeRepository,
        IMapper mapper
    )
    {
        _productRepository = productRepository;
        _productBrandRepository = productBrandRepository;
        _productTypeRepository = productTypeRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProductResponseDto[]), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ProductResponseDto>>> GetProducts(string sort)
    {
        var spec = new ProductsSpecification(sort);
        var products = await this._productRepository.ListAsync(spec);

        return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductResponseDto>>(products));
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
    {
        var spec = new ProductsSpecification(id);
        var product = await this._productRepository.GetEntityWithSpec(spec);

        if (product is null)
        {
            return NotFound(new ApiResponse(StatusCodes.Status404NotFound));
        }

        return Ok(_mapper.Map<Product, ProductResponseDto>(product));
    }

    [HttpGet("brands")]
    [ProducesResponseType(typeof(ProductBrand[]), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductBrand>>> GetBrands()
    {
        return Ok(await this._productBrandRepository.GetAllAsync());
    }

    [HttpGet("types")]
    [ProducesResponseType(typeof(ProductType[]), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductType>>> GetTypes()
    {
        return Ok(await this._productTypeRepository.GetAllAsync());
    }
}
