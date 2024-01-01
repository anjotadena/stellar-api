namespace Core.Entities.OrderAggregate;

public class Order : BaseEntity
{
    public Order()
    {
    }

    public Order(IReadOnlyList<OrderItem> orderItems, string buyerEmail, Address shipToAddress, DeliveryMethod deliveryMethod, decimal subtotal)
    {
        BuyerEmail = buyerEmail;
        ShipToAddress = shipToAddress;
        DeliveryMethod = deliveryMethod;
        Subtotal = subtotal;
        OrderItems = orderItems;
    }

    public string BuyerEmail { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public Address ShipToAddress { get; set; }

    public DeliveryMethod DeliveryMethod { get; set; }

    public IReadOnlyList<OrderItem> OrderItems { get; set; }

    public decimal Subtotal { get; set; }

    public OrderStatus Status { get; set; }

    public string PaymentIntentId { get; set; }

    public decimal GetTotal()
    {
        return Subtotal + DeliveryMethod.Price;
    }
}
