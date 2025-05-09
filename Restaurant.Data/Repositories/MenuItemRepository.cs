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
    /// <seealso cref="Restaurant.Domain.Interfaces.IMenuItemRepository" />
    public class MenuItemRepository : IMenuItemRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly AppDbContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItemRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public MenuItemRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public MenuItem GetById(Guid id)
        {
            return _context.MenuItems.Find(id);
        }
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MenuItem> GetAll()
        {
            return _context.MenuItems.ToList();
        }
        /// <summary>
        /// Gets the available items.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MenuItem> GetAvailableItems()
        => _context.MenuItems
             .Where(m => m.IsAvailable)
             .ToList();

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(MenuItem item)
        {
            _context.MenuItems.Add(item);
            _context.SaveChanges();
        }
        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Update(MenuItem item)
        {
            _context.MenuItems.Update(item);
            _context.SaveChanges();
        }
        /// <summary>
        /// Gets the by category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public IEnumerable<MenuItem> GetByCategory(string category)
        {
            return _context.MenuItems.Where(m => m.Category == category).ToList();
        }
    }
}
