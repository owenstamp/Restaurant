using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;

namespace Restaurant.Data.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly AppDbContext _context;
        public MenuItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public MenuItem GetById(Guid id)
        {
            return _context.MenuItems.Find(id);
        }
        public IEnumerable<MenuItem> GetAll()
        {
            return _context.MenuItems.ToList();
        }
        public void Add(MenuItem item)
        {
            _context.MenuItems.Add(item);
            _context.SaveChanges();
        }
        public void Update(MenuItem item)
        {
            _context.MenuItems.Update(item);
            _context.SaveChanges();
        }
        public IEnumerable<MenuItem> GetByCategory(string category)
        {
            return _context.MenuItems.Where(m => m.Category == category).ToList();
        }
    }
}
