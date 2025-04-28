using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;

namespace Restaurant.Web.Pages
{
    public class KitchenOrdersModel : PageModel
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMenuItemRepository _menuRepo;

        public KitchenOrdersModel(
            IOrderRepository orderRepo,
            IMenuItemRepository menuRepo)
        {
            _orderRepo = orderRepo;
            _menuRepo = menuRepo;
        }

        // DTO for a single ordered item
        public class ItemDto
        {
            public string Name { get; init; }
            public int Quantity { get; init; }
        }

        // ViewModel for each order
        public class OrderWithItems
        {
            public Guid Id { get; init; }
            public DateTime OrderedAt { get; init; }
            public string Status { get; init; }
            public List<ItemDto> Items { get; init; }
        }

        // Exposed to the Razor page
        public List<OrderWithItems> Orders { get; set; } = new();
        [BindProperty] public Guid OrderId { get; set; }
        public string Message { get; set; }

        // GET handler: include both Received & Preparing orders
        public void OnGet()
        {
            var toShow = _orderRepo
                .GetAll()
                .Where(o =>
                    o.Status == Order.OrderStatus.Received ||
                    o.Status == Order.OrderStatus.Preparing)
                .ToList();

            Orders = toShow
                .Select(o =>
                {
                    var raw = _orderRepo.GetItemsForOrder(o.Id).ToList();
                    return new OrderWithItems
                    {
                        Id = o.Id,
                        OrderedAt = o.OrderDateTime,
                        Status = o.Status.ToString(),
                        Items = raw
                            .Select(oi => new ItemDto
                            {
                                Name = _menuRepo.GetById(oi.MenuItemId)?.Name ?? "[Unknown]",
                                Quantity = oi.Quantity
                            })
                            .ToList()
                    };
                })
                .ToList();
        }

        // POST handler: move to Ready
        public IActionResult OnPostMarkAsPrepared()
        {
            var order = _orderRepo.GetById(OrderId);
            if (order == null)
            {
                Message = "Order not found.";
            }
            else
            {
                _orderRepo.UpdateOrderStatus(OrderId, Order.OrderStatus.Ready);
                Message = "Order marked as Ready.";
            }

            OnGet();
            return Page();
        }
    }
}
