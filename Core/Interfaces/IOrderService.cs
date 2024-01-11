using Core.Entities.OrderAggregate;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string email, int paymentMethod, string cartId, Address address);

        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string email);

        Task<Order> GetOrderByIdAsync(int id, string email);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}