using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepository)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
        }

        public async Task<Order?> CreateOrderAsync(string email, int paymentMethodId, string cartId, Core.Entities.OrderAggregate.Address address)
        {
            var basket = await _basketRepository.GetBasketAsync(cartId);

            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);

                items.Add(orderItem);
            }

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(paymentMethodId);

            // calculate subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            var order = new Order(items, email, address, deliveryMethod, subtotal);

            _unitOfWork.Repository<Order>().Add(order);

            var result = await _unitOfWork.Complete();

            if (result <= 0) 
            {
                return null;
            }

            await _basketRepository.DeleteBasketAsync(cartId);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string email)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(email);

            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }

        public async Task<Order> GetOrdersByIdAsync(int id, string email)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, email);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }
    }
}