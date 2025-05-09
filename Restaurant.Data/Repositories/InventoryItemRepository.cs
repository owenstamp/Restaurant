using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;

namespace Restaurant.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Restaurant.Domain.Interfaces.IInventoryItemRepository" />
    public class InventoryItemRepository : IInventoryItemRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly AppDbContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryItemRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public InventoryItemRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public InventoryItem GetById(Guid id)
        {
            return _context.InventoryItems.Find(id);
        }
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InventoryItem> GetAll()
        {
            return _context.InventoryItems.ToList();
        }
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(InventoryItem item)
        {
            _context.InventoryItems.Add(item);
            _context.SaveChanges();
        }
        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Update(InventoryItem item)
        {
            _context.InventoryItems.Update(item);
            _context.SaveChanges();
        }
        /// <summary>
        /// Gets the below reorder level.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<InventoryItem> GetBelowReorderLevel()
        {
            return _context.InventoryItems.Where(i => i.QuantityOnHand <= i.ReorderLevel).ToList();
        }
    }
}
