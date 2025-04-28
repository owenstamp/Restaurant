using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    public interface IMenuItemRepository
    {
        MenuItem GetById(Guid id);
        IEnumerable<MenuItem> GetAll();
        IEnumerable<MenuItem> GetAvailableItems();

        void Add(MenuItem item);
        void Update(MenuItem item);

        // Possibly a search by category
        IEnumerable<MenuItem> GetByCategory(string category);
    }
}
