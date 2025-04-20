using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    public interface IOrderRepository
    {
        /// <summary>
        /// Retrieves an order by its unique identifier.
        /// </summary>
        Order GetById(Guid orderId);

        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        IEnumerable<Order> GetAll();

        /// <summary>
        /// Adds a new order to the repository.
        /// </summary>
        void Add(Order order);

        /// <summary>
        /// Updates an existing order in the repository.
        /// </summary>
        void Update(Order order);

        // You may add additional methods based on your project requirements:
        // IEnumerable<Order> GetByStatus(Order.OrderStatus status);
        // IEnumerable<Order> GetByReservationId(Guid reservationId);
    }
}
