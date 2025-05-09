// Restaurant.Data/Repositories/OrderRepository.cs
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Restaurant.Domain.Interfaces.IOrderRepository" />
    public class OrderRepository : IOrderRepository
    {
        /// <summary>
        /// The database
        /// </summary>
        private readonly AppDbContext _db;
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public OrderRepository(AppDbContext db) => _db = db;

        /// <summary>
        /// Create a brand‑new order (and save it)
        /// </summary>
        /// <param name="waitstaffId">The waitstaff identifier.</param>
        /// <param name="reservationId">The reservation identifier.</param>
        /// <returns></returns>
        public Order CreateOrder(Guid waitstaffId, Guid? reservationId = null)
        {
            var order = new Order(waitstaffId, reservationId);
            _db.Orders.Add(order);
            _db.SaveChanges();
            return order;
        }

        // In Restaurant.Data/Repositories/OrderRepository.cs

        /// <summary>
        /// // Add an item to an existing order (and recalc+save)
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="item">The item.</param>
        /// <exception cref="System.InvalidOperationException">Order {orderId} not found</exception>
        public void AddItemToOrder(Guid orderId, OrderItem item)
        {
            // 1) Load the parent Order
            var order = _db.Orders.Find(orderId)
                ?? throw new InvalidOperationException($"Order {orderId} not found");

            // 2) Set the foreign key on the new item
            item.OrderId = orderId;

            // 3) Add the new item entity to the context's DbSet
            _db.OrderItems.Add(item);

            // 4) Manually update the order's total *before* saving
            order.RecalculateTotal(); // Direct incremental update

            // 5) Save changes ONCE.
            _db.SaveChanges();
        }

        /// <summary>
        /// Updates the order status.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <param name="newStatus">The new status.</param>
        /// <exception cref="System.InvalidOperationException">Order {orderId} not found</exception>
        public void UpdateOrderStatus(Guid orderId, Order.OrderStatus newStatus)
        {
            var order = _db.Orders.Find(orderId)
                ?? throw new InvalidOperationException($"Order {orderId} not found");

            order.UpdateStatus(newStatus);
            _db.SaveChanges();
        }

        /// <summary>
        /// Gets the recent orders.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public IEnumerable<Order> GetRecentOrders(int count)
        {
            return _db.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.OrderDateTime)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Gets the items for order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        public IEnumerable<OrderItem> GetItemsForOrder(Guid orderId) =>
    _db.OrderItems
        .Where(oi => oi.OrderId == orderId)
        .ToList();


        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Order GetById(Guid id)
        {
            return _db.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Order> GetAll()
        {
            return _db.Orders
                .Include(o => o.Items)
                .ToList();
        }


    }
}
