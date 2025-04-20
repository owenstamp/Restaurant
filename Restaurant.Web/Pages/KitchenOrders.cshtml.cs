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
    public class KitchenOrdersModel : PageModel
    {
        private readonly AppDbContext _context;

        public KitchenOrdersModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; } = new();
        [BindProperty] public Guid OrderId { get; set; }
        [BindProperty] public string Action { get; set; }
        public string Message { get; set; }

        public Dictionary<Guid, string> MenuItemNames { get; set; } = new();


        public void OnGet()
        {
            Orders = _context.Orders
                .Include(o => o.Items)
                .Where(o => o.Status == Order.OrderStatus.Received || o.Status == Order.OrderStatus.Preparing)
                .OrderBy(o => o.OrderDateTime)
                .ToList();
            MenuItemNames = _context.MenuItems
    .ToDictionary(m => m.Id, m => m.Name);

        }

        public IActionResult OnPost()
        {
            var order = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == OrderId);
            MenuItemNames = _context.MenuItems
    .ToDictionary(m => m.Id, m => m.Name);

            if (order == null)
            {
                Message = "Order not found.";
                OnGet();
                return Page();
            }

            if (Action == "Preparing")
            {
                order.UpdateStatus(Order.OrderStatus.Preparing);
                Message = $"Order {order.Id} marked as Preparing.";
            }
            else if (Action == "Ready")
            {
                order.UpdateStatus(Order.OrderStatus.Ready);
                Message = $"Order {order.Id} marked as Ready.";
            }

            _context.SaveChanges();
            OnGet();
            return Page();
        }
    }
}
