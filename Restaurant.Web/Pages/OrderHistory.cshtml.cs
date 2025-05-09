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
    public class OrderHistoryModel : PageModel
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
        /// Initializes a new instance of the <see cref="OrderHistoryModel"/> class.
        /// </summary>
        /// <param name="orderRepo">The order repo.</param>
        /// <param name="menuRepo">The menu repo.</param>
        public OrderHistoryModel(IOrderRepository orderRepo, IMenuItemRepository menuRepo)
        {
            _orderRepo = orderRepo;
            _menuRepo = menuRepo;
        }

        // All past orders (could be paged or filtered if you like)
        /// <summary>
        /// Gets or sets the past orders.
        /// </summary>
        /// <value>
        /// The past orders.
        /// </value>
        public List<Order> PastOrders { get; set; } = new();

        // The one the user clicked on (if any)
        /// <summary>
        /// Gets or sets the selected order.
        /// </summary>
        /// <value>
        /// The selected order.
        /// </value>
        public Order? SelectedOrder { get; set; }

        // A helper dictionary so your Razor can show MenuItem Names by Id
        /// <summary>
        /// Gets or sets the menu item names.
        /// </summary>
        /// <value>
        /// The menu item names.
        /// </value>
        public Dictionary<Guid, string> MenuItemNames { get; set; } = new();

        // Optional query‐param to drill into one order
        /// <summary>
        /// Called when [get].
        /// </summary>
        /// <param name="selectedOrderId">The selected order identifier.</param>
        /// <returns></returns>
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
