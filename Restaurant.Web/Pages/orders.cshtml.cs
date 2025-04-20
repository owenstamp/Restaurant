using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class OrdersModel : PageModel
    {
        private readonly AppDbContext _context;

        public OrdersModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Guid MenuItemId { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        [BindProperty]
        public string SpecialRequest { get; set; }

        public List<MenuItem> MenuItems { get; set; } = new();
        public List<Order> RecentOrders { get; set; } = new();
        public string Message { get; set; }

        public void OnGet()
        {
            LoadData();
        }

        public IActionResult OnPost()
        {
            LoadData();

            if (!ModelState.IsValid || MenuItemId == Guid.Empty || Quantity <= 0)
            {
                Message = "Please select a menu item and enter a valid quantity.";
                return Page();
            }

            var menuItem = _context.MenuItems.FirstOrDefault(m => m.Id == MenuItemId);
            if (menuItem == null)
            {
                Message = "Selected menu item not found.";
                return Page();
            }

            var orderItem = new OrderItem(MenuItemId, Quantity, menuItem.Price, SpecialRequest);
            var staffIdString = HttpContext.Session.GetString("StaffId");

            if (!Guid.TryParse(staffIdString, out Guid staffId))
            {
                Message = "Invalid staff session. Please log in again.";
                return Page();
            }

            var order = new Order(staffId); //  Now passing the required waitstaffId
            order.AddItem(orderItem);


            _context.Orders.Add(order);
            _context.SaveChanges();

            Message = "Order placed successfully.";
            LoadData();
            return Page();
        }

        private void LoadData()
        {
            MenuItems = _context.MenuItems.Where(m => m.IsAvailable).ToList();
            RecentOrders = _context.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.OrderDateTime)
                .Take(5)
                .ToList();
        }

        public string GetMenuItemName(Guid id)
        {
            var item = MenuItems.FirstOrDefault(m => m.Id == id);
            return item != null ? item.Name : "Unknown";
        }
    }
}
