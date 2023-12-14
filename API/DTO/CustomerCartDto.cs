using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class CustomerCartDto
{
    [Required]
    public string Id { get; set; }

    public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
}