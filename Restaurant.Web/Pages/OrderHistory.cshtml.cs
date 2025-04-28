using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class OrderHistoryModel : PageModel
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMenuItemRepository _menuRepo;

        public OrderHistoryModel(IOrderRepository orderRepo, IMenuItemRepository menuRepo)
        {
            _orderRepo = orderRepo;
            _menuRepo = menuRepo;
        }

        // All past orders (could be paged or filtered if you like)
        public List<Order> PastOrders { get; set; } = new();

        // The one the user clicked on (if any)
        public Order? SelectedOrder { get; set; }

        // A helper dictionary so your Razor can show MenuItem Names by Id
        public Dictionary<Guid, string> MenuItemNames { get; set; } = new();

        // Optional query‐param to drill into one order
        public IActionResult OnGet(Guid? selectedOrderId = null)
        {
            // 1) Load all orders, newest first
            PastOrders = _orderRepo
                .GetAll()
                .OrderByDescending(o => o.OrderDateTime)
                .ToList();

            // 2) If the user passed ?selectedOrderId=..., load it
            if (selectedOrderId.HasValue)
            {
                SelectedOrder = _orderRepo.GetById(selectedOrderId.Value);
                if (SelectedOrder == null)
                    return NotFound();    // or just Page() with a message

                // 3) Build the lookup of MenuItemId → Name
                MenuItemNames = SelectedOrder.Items
                    .Select(i => i.MenuItemId)
                    .Distinct()
                    .ToDictionary(
                        id => id,
                        id => _menuRepo.GetById(id)?.Name ?? "[Unknown]"
                    );
            }

            return Page();
        }
    }
}
