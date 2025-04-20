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
    public class OrderHistoryModel : PageModel
    {
        private readonly AppDbContext _context;

        public OrderHistoryModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Order> PastOrders { get; set; } = new();
        public Order? SelectedOrder { get; set; }
        public Dictionary<Guid, string> MenuItemNames { get; set; } = new();

        public IActionResult OnGet(Guid? selectedOrderId = null)
        {
            // Only show completed orders
            PastOrders = _context.Orders
                .Include(o => o.Items)
                .Where(o => o.Status == Order.OrderStatus.Served)
                .OrderByDescending(o => o.OrderDateTime)
                .ToList();

            MenuItemNames = _context.MenuItems
                .ToDictionary(m => m.Id, m => m.Name);

            if (selectedOrderId != null)
            {
                SelectedOrder = _context.Orders
                    .Include(o => o.Items)
                    .FirstOrDefault(o => o.Id == selectedOrderId);
            }

            return Page();
        }
    }
}
