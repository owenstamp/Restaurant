using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMenuItemRepository
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        MenuItem GetById(Guid id);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<MenuItem> GetAll();
        /// <summary>
        /// Gets the available items.
        /// </summary>
        /// <returns></returns>
        IEnumerable<MenuItem> GetAvailableItems();

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Add(MenuItem item);
        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Update(MenuItem item);

        // Possibly a search by category
        /// <summary>
        /// Gets the by category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        IEnumerable<MenuItem> GetByCategory(string category);
    }
}
