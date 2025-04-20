using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;

namespace Restaurant.Data.Repositories
{
    public class InventoryItemRepository : IInventoryItemRepository
    {
        private readonly AppDbContext _context;
        public InventoryItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public InventoryItem GetById(Guid id)
        {
            return _context.InventoryItems.Find(id);
        }
        public IEnumerable<InventoryItem> GetAll()
        {
            return _context.InventoryItems.ToList();
        }
        public void Add(InventoryItem item)
        {
            _context.InventoryItems.Add(item);
            _context.SaveChanges();
        }
        public void Update(InventoryItem item)
        {
            _context.InventoryItems.Update(item);
            _context.SaveChanges();
        }
        public IEnumerable<InventoryItem> GetBelowReorderLevel()
        {
            return _context.InventoryItems.Where(i => i.QuantityOnHand <= i.ReorderLevel).ToList();
        }
    }
}
