// Restaurant.Domain/Interfaces/IOrderRepository.cs
using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOrderRepository
    {

        /// <summary>
        /// Create a brand‑new order (and save it)
        /// </summary>
        /// <param name="waitstaffId">The waitstaff identifier.</param>
        /// <param name="reservationId">The reservation identifier.</param>
        /// <returns></returns>
        Order CreateOrder(Guid waitstaffId, Guid? reservationId = null);


        /// <summary>
        /// // Add an item to an existing order (and recalc+save)
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="item">The item.</param>
        void AddItemToOrder(Guid orderId, OrderItem item);

        /// <summary>
        /// Updates the order status.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="newStatus">The new status.</param>
        void UpdateOrderStatus(Guid orderId, Order.OrderStatus newStatus);

        /// <summary>
        /// Gets the items for order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        IEnumerable<OrderItem> GetItemsForOrder(Guid orderId);

        /// <summary>
        /// Gets the recent orders.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        IEnumerable<Order> GetRecentOrders(int count);

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Order GetById(Guid id);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Order> GetAll();


    }
}
