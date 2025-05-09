using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class WaitstaffOrdersModel : PageModel
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
        /// Initializes a new instance of the <see cref="WaitstaffOrdersModel"/> class.
        /// </summary>
        /// <param name="orderRepo">The order repo.</param>
        /// <param name="menuRepo">The menu repo.</param>
        public WaitstaffOrdersModel(IOrderRepository orderRepo, IMenuItemRepository menuRepo)
        {
            _orderRepo = orderRepo;
            _menuRepo = menuRepo;
        }

        // Orders ready to be served
        /// <summary>
        /// Gets or sets the orders.
        /// </summary>
        /// <value>
        /// The orders.
        /// </value>
        public List<Order> Orders { get; set; } = new();

        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        [BindProperty] public Guid OrderId { get; set; }
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        [BindProperty] public string Action { get; set; }  // e.g. "Serve"
        /// <summary>
        /// Gets or sets the menu item names.
        /// </summary>
        /// <value>
        /// The menu item names.
        /// </value>
        public Dictionary<Guid, string> MenuItemNames { get; set; } = new();
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Called when [get].
        /// </summary>
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

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <returns></returns>
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
