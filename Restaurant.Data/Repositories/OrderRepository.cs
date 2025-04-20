using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;

namespace Restaurant.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }
        public Order GetById(Guid id)
        {
            // Include OrderItems for a complete order view
            return _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == id);
        }
        public IEnumerable<Order> GetAll()
        {
            return _context.Orders.Include(o => o.Items).ToList();
        }
        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
        public void Update(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
    }
}
