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
    public class WaitstaffOrdersModel : PageModel
    {
        private readonly AppDbContext _context;

        public WaitstaffOrdersModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; } = new();
        [BindProperty] public Guid OrderId { get; set; }
        [BindProperty] public string Action { get; set; }
        public Dictionary<Guid, string> MenuItemNames { get; set; } = new();
        public string Message { get; set; }

        public void OnGet()
        {
            Orders = _context.Orders
                .Include(o => o.Items)
                .Where(o => o.Status == Order.OrderStatus.Ready)
                .OrderBy(o => o.OrderDateTime)
                .ToList();

            MenuItemNames = _context.MenuItems
                .ToDictionary(m => m.Id, m => m.Name);
        }

        public IActionResult OnPost()
        {
            var order = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == OrderId);
            if (order == null)
            {
                Message = "Order not found.";
                OnGet();
                return Page();
            }

            if (Action == "Served")
            {
                order.UpdateStatus(Order.OrderStatus.Served);
                Message = $"Order {order.Id} marked as Served.";
                _context.SaveChanges();
            }

            OnGet(); // Refresh view
            return Page();
        }
    }
}
