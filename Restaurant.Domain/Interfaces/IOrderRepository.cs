// Restaurant.Domain/Interfaces/IOrderRepository.cs
using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    public interface IOrderRepository
    {
        // Create a brand‑new order (and save it)
        Order CreateOrder(Guid waitstaffId, Guid? reservationId = null);

        // Add an item to an existing order (and recalc+save)
        void AddItemToOrder(Guid orderId, OrderItem item);

        // Change the status (e.g., Received → Preparing → Ready → Served)
        void UpdateOrderStatus(Guid orderId, Order.OrderStatus newStatus);

        IEnumerable<OrderItem> GetItemsForOrder(Guid orderId);

        // Fetch the last N orders, most‑recent first
        IEnumerable<Order> GetRecentOrders(int count);

        // Basic retrieval
        Order GetById(Guid id);
        IEnumerable<Order> GetAll();


    }
}
