using API.DTO;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
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
    public async Task<ActionResult<IReadOnlyList<ProductResponseDto>>> GetProducts()
    {
        var spec = new ProductsSpecification();
        var products = await this._productRepository.ListAsync(spec);

        return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductResponseDto>>(products));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
    {
        var spec = new ProductsSpecification(id);
        var product = await this._productRepository.GetEntityWithSpec(spec);

        return Ok(_mapper.Map<Product, ProductResponseDto>(product));
    }

    [HttpGet("brands")]
    public async Task<ActionResult<List<ProductBrand>>> GetBrands()
    {
        return Ok(await this._productBrandRepository.GetAllAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<List<ProductType>>> GetTypes()
    {
        return Ok(await this._productTypeRepository.GetAllAsync());
    }
}
