using API.DTO;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ApiBaseController
{
    public IBasketRepository _basketRepository;
    private readonly IMapper _mapper;

    public BasketController(IBasketRepository basketRepository, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
    {
        var basket = await _basketRepository.GetBasketAsync(id);

        return Ok(basket ?? new CustomerBasket(id));
    }

    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerCartDto basket)
    {
        var updatedBasket = await _basketRepository.UpdateBasketAsync(_mapper.Map<CustomerCartDto, CustomerBasket>(basket));

        return Ok(updatedBasket);   
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteBasket(string id)
    {
     await _basketRepository.DeleteBasketAsync(id);

     return NoContent();   
    }
}
