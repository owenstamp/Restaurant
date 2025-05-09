using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;

namespace Restaurant.Web.Pages
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class KitchenOrdersModel : PageModel
    {
        /// <summary>
        /// The order repo
        /// </summary>
        private readonly IOrderRepository _orderRepo;
        /// <summary>
        /// The menu repo
        /// </summary>
        private readonly IMenuItemRepository _menuRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="KitchenOrdersModel"/> class.
        /// </summary>
        /// <param name="orderRepo">The order repo.</param>
        /// <param name="menuRepo">The menu repo.</param>
        public KitchenOrdersModel(
            IOrderRepository orderRepo,
            IMenuItemRepository menuRepo)
        {
            _orderRepo = orderRepo;
            _menuRepo = menuRepo;
        }

        // DTO for a single ordered item
        /// <summary>
        /// 
        /// </summary>
        public class ItemDto
        {
            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            public string Name { get; init; }
            /// <summary>
            /// Gets the quantity.
            /// </summary>
            /// <value>
            /// The quantity.
            /// </value>
            public int Quantity { get; init; }
        }

        // ViewModel for each order
        /// <summary>
        /// 
        /// </summary>
        public class OrderWithItems
        {
            /// <summary>
            /// Gets the identifier.
            /// </summary>
            /// <value>
            /// The identifier.
            /// </value>
            public Guid Id { get; init; }
            /// <summary>
            /// Gets the ordered at.
            /// </summary>
            /// <value>
            /// The ordered at.
            /// </value>
            public DateTime OrderedAt { get; init; }
            /// <summary>
            /// Gets the status.
            /// </summary>
            /// <value>
            /// The status.
            /// </value>
            public string Status { get; init; }
            /// <summary>
            /// Gets the items.
            /// </summary>
            /// <value>
            /// The items.
            /// </value>
            public List<ItemDto> Items { get; init; }
        }

        // Exposed to the Razor page
        /// <summary>
        /// Gets or sets the orders.
        /// </summary>
        /// <value>
        /// The orders.
        /// </value>
        public List<OrderWithItems> Orders { get; set; } = new();
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        [BindProperty] public Guid OrderId { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        // GET handler: include both Received & Preparing orders
        /// <summary>
        /// Called when [get].
        /// </summary>
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
        /// <summary>
        /// Called when [post mark as prepared].
        /// </summary>
        /// <returns></returns>
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
