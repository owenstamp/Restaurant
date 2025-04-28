using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class WaitstaffOrdersModel : PageModel
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMenuItemRepository _menuRepo;

        public WaitstaffOrdersModel(IOrderRepository orderRepo, IMenuItemRepository menuRepo)
        {
            _orderRepo = orderRepo;
            _menuRepo = menuRepo;
        }

        // Orders ready to be served
        public List<Order> Orders { get; set; } = new();

        [BindProperty] public Guid OrderId { get; set; }
        [BindProperty] public string Action { get; set; }  // e.g. "Serve"
        public Dictionary<Guid, string> MenuItemNames { get; set; } = new();
        public string Message { get; set; }

        public void OnGet()
        {
            // 1) Load all orders in "Ready" status
            Orders = _orderRepo
                .GetAll()
                .Where(o => o.Status == Order.OrderStatus.Ready)
                .ToList();

            // 2) Build lookup for item names
            var itemIds = Orders.SelectMany(o => o.Items)
                                .Select(i => i.MenuItemId)
                                .Distinct();

            MenuItemNames = itemIds.ToDictionary(
                id => id,
                id => _menuRepo.GetById(id)?.Name ?? "[Unknown]"
            );
        }

        public IActionResult OnPost()
        {
            // Mark as served if requested
            if (Action == "Served" && OrderId != Guid.Empty)
            {
                var order = _orderRepo.GetById(OrderId);
                if (order != null)
                {
                    _orderRepo.UpdateOrderStatus(OrderId, Order.OrderStatus.Served);
                    Message = "Order served successfully.";
                }
                else
                {
                    Message = "Order not found.";
                }
            }

            // Refresh the list
            OnGet();
            return Page();
        }
    }
}
