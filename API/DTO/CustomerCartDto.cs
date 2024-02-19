using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class CustomerCartDto
{
    [Required]
    public string Id { get; set; }

    public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();

    public int? DeliveryMethodId { get; set; }

    public string ClientSecret { get; set; }

    public string PaymentIntentId { get; set; }

    public decimal ShippingPrice { get; set; }
}