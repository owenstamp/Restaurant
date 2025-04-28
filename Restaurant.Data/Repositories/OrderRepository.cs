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
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _db;
        public OrderRepository(AppDbContext db) => _db = db;

        public Order CreateOrder(Guid waitstaffId, Guid? reservationId = null)
        {
            var order = new Order(waitstaffId, reservationId);
            _db.Orders.Add(order);
            _db.SaveChanges();
            return order;
        }

        // In Restaurant.Data/Repositories/OrderRepository.cs

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

        public void UpdateOrderStatus(Guid orderId, Order.OrderStatus newStatus)
        {
            var order = _db.Orders.Find(orderId)
                ?? throw new InvalidOperationException($"Order {orderId} not found");

            order.UpdateStatus(newStatus);
            _db.SaveChanges();
        }

        public IEnumerable<Order> GetRecentOrders(int count)
        {
            return _db.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.OrderDateTime)
                .Take(count)
                .ToList();
        }

        public IEnumerable<OrderItem> GetItemsForOrder(Guid orderId) =>
    _db.OrderItems
        .Where(oi => oi.OrderId == orderId)
        .ToList();
   

        public Order GetById(Guid id)
        {
            return _db.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<Order> GetAll()
        {
            return _db.Orders
                .Include(o => o.Items)
                .ToList();
        }


    }
}
