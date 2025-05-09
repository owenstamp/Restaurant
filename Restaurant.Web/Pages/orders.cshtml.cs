using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;           // Required for Session extensions
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurant.Web.Pages
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class OrdersModel : PageModel
    {
        /// <summary>
        /// The order repo
        /// </summary>
        private readonly IOrderRepository _orderRepo;
        /// <summary>
        /// The menu repo
        /// </summary>
        private readonly IMenuItemRepository _menuRepo;
        // REMOVED: IHttpContextAccessor dependency

        // Session Key constant remains the same
        /// <summary>
        /// The session key current order identifier
        /// </summary>
        private const string SessionKeyCurrentOrderId = "_CurrentOrderId";

        // Constructor updated - no longer needs IHttpContextAccessor
        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersModel"/> class.
        /// </summary>
        /// <param name="orderRepo">The order repo.</param>
        /// <param name="menuRepo">The menu repo.</param>
        public OrdersModel(IOrderRepository orderRepo, IMenuItemRepository menuRepo)
        {
            _orderRepo = orderRepo;
            _menuRepo = menuRepo;
            // REMOVED: _httpContextAccessor assignment
        }

        // Properties for the "Add Item" form
        /// <summary>
        /// Gets or sets the menu item identifier.
        /// </summary>
        /// <value>
        /// The menu item identifier.
        /// </value>
        [BindProperty] public Guid MenuItemId { get; set; }
        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        [BindProperty] public int Quantity { get; set; } = 1;
        /// <summary>
        /// Gets or sets the special request.
        /// </summary>
        /// <value>
        /// The special request.
        /// </value>
        [BindProperty] public string? SpecialRequest { get; set; }

        // Properties to display data on the page
        /// <summary>
        /// Gets or sets the menu items.
        /// </summary>
        /// <value>
        /// The menu items.
        /// </value>
        public List<MenuItem> MenuItems { get; set; } = new();
        /// <summary>
        /// Gets or sets the current order.
        /// </summary>
        /// <value>
        /// The current order.
        /// </value>
        public Order? CurrentOrder { get; set; }
        /// <summary>
        /// Gets or sets the current order items.
        /// </summary>
        /// <value>
        /// The current order items.
        /// </value>
        public List<OrderItemViewModel> CurrentOrderItems { get; set; } = new();
        /// <summary>
        /// Gets or sets the recent orders.
        /// </summary>
        /// <value>
        /// The recent orders.
        /// </value>
        public List<Order> RecentOrders { get; set; } = new();
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string? Message { get; set; }
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string? ErrorMessage { get; set; }

        // ViewModel remains the same
        /// <summary>
        /// 
        /// </summary>
        public class OrderItemViewModel
        {
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>
            /// The identifier.
            /// </value>
            public Guid Id { get; set; }
            /// <summary>
            /// Gets or sets the name of the menu item.
            /// </summary>
            /// <value>
            /// The name of the menu item.
            /// </value>
            public string MenuItemName { get; set; } = string.Empty;
            /// <summary>
            /// Gets or sets the quantity.
            /// </summary>
            /// <value>
            /// The quantity.
            /// </value>
            public int Quantity { get; set; }
            /// <summary>
            /// Gets or sets the price.
            /// </summary>
            /// <value>
            /// The price.
            /// </value>
            public decimal Price { get; set; }
            /// <summary>
            /// Gets or sets the special requests.
            /// </summary>
            /// <value>
            /// The special requests.
            /// </value>
            public string? SpecialRequests { get; set; }
            /// <summary>
            /// Gets the line total.
            /// </summary>
            /// <value>
            /// The line total.
            /// </value>
            public decimal LineTotal => Quantity * Price;
        }

        // OnGet uses LoadPageData, which now uses HttpContext directly
        /// <summary>
        /// Called when [get].
        /// </summary>
        public void OnGet()
        {
            LoadPageData();
        }

        // Add Item handler - uses HttpContext directly
        /// <summary>
        /// Called when [post add item].
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostAddItem()
        {
            if (!ModelState.IsValid || Quantity <= 0 || MenuItemId == Guid.Empty)
            {
                ErrorMessage = "Please select a valid item and quantity.";
                LoadPageData();
                return Page();
            }

            // Use HttpContext directly
            if (!Guid.TryParse(HttpContext.Session.GetString("StaffId"), out var waitstaffId))
            {
                return RedirectToPage("/Login", new { message = "Session expired. Please log in again." });
            }

            // Use HttpContext directly
            var orderIdString = HttpContext.Session.GetString(SessionKeyCurrentOrderId);
            Guid.TryParse(orderIdString, out var currentOrderId);

            if (currentOrderId == Guid.Empty)
            {
                var newOrder = _orderRepo.CreateOrder(waitstaffId, reservationId: null);
                CurrentOrder = _orderRepo.GetById(newOrder.Id);
                if (CurrentOrder == null)
                {
                    ErrorMessage = "Failed to create a new order.";
                    LoadPageData();
                    return Page();
                }
                currentOrderId = CurrentOrder.Id;
                // Use HttpContext directly
                HttpContext.Session.SetString(SessionKeyCurrentOrderId, currentOrderId.ToString());
            }

            var menuItem = _menuRepo.GetById(MenuItemId);
            if (menuItem == null || !menuItem.IsAvailable)
            {
                ErrorMessage = $"Menu item not found or is unavailable.";
                LoadPageData();
                return Page();
            }

            var newItem = new OrderItem(
                menuItemId: menuItem.Id,
                quantity: Quantity,
                price: menuItem.Price,
                specialRequests: SpecialRequest
            );

            try
            {
                _orderRepo.AddItemToOrder(currentOrderId, newItem);
                Message = $"{menuItem.Name} (x{Quantity}) added to the current order.";
            }
            catch (Exception ex)
            {
                // Log the exception ex
                ErrorMessage = $"Error adding item to order: {ex.Message}";
                LoadPageData();
                return Page();
            }

            LoadPageData();
            return Page();
        }

        // Submit Order handler - uses HttpContext directly
        /// <summary>
        /// Called when [post submit order].
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostSubmitOrder()
        {
            // Use HttpContext directly
            var orderIdString = HttpContext.Session.GetString(SessionKeyCurrentOrderId);
            Guid.TryParse(orderIdString, out var currentOrderId);

            if (currentOrderId == Guid.Empty)
            {
                ErrorMessage = "There is no active order to submit.";
                LoadPageData();
                return Page();
            }

            var order = _orderRepo.GetById(currentOrderId);
            if (order == null || !order.Items.Any())
            {
                ErrorMessage = "Cannot submit an empty order.";
                LoadPageData();
                return Page();
            }

            // Optional: Update status
            // _orderRepo.UpdateOrderStatus(currentOrderId, Order.OrderStatus.Received);

            // Use HttpContext directly
            HttpContext.Session.Remove(SessionKeyCurrentOrderId);

            Message = $"Order {order.Id.ToString().Substring(0, 8)}... submitted successfully!";
            LoadPageData();
            return Page();
        }

        // Cancel Order handler - uses HttpContext directly
        /// <summary>
        /// Called when [post cancel order].
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostCancelOrder()
        {
            // Use HttpContext directly
            HttpContext.Session.Remove(SessionKeyCurrentOrderId);
            Message = "Current order cancelled.";
            LoadPageData();
            return Page();
        }

        // LoadPageData helper - uses HttpContext directly
        /// <summary>
        /// Loads the page data.
        /// </summary>
        private void LoadPageData()
        {
            MenuItems = _menuRepo.GetAvailableItems().ToList();
            RecentOrders = _orderRepo.GetRecentOrders(5).ToList();

            // Use HttpContext directly
            var orderIdString = HttpContext.Session.GetString(SessionKeyCurrentOrderId);
            Guid.TryParse(orderIdString, out var currentOrderId);

            if (currentOrderId != Guid.Empty)
            {
                CurrentOrder = _orderRepo.GetById(currentOrderId);
                if (CurrentOrder != null)
                {
                    var items = _orderRepo.GetItemsForOrder(currentOrderId);
                    CurrentOrderItems = items.Select(oi => new OrderItemViewModel
                    {
                        Id = oi.Id,
                        MenuItemName = _menuRepo.GetById(oi.MenuItemId)?.Name ?? "[Unknown]",
                        Quantity = oi.Quantity,
                        Price = oi.Price,
                        SpecialRequests = oi.SpecialRequests
                    }).ToList();
                }
                else
                {
                    // Use HttpContext directly
                    HttpContext.Session.Remove(SessionKeyCurrentOrderId); // Clear invalid ID
                    CurrentOrder = null;
                    CurrentOrderItems.Clear();
                }
            }
            else
            {
                CurrentOrder = null;
                CurrentOrderItems.Clear();
            }
        }

        // REMOVED Session Helper Methods as logic is now inline


        // Helper remains the same
        /// <summary>
        /// Gets the name of the menu item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public string GetMenuItemName(Guid id)
            => _menuRepo.GetById(id)?.Name ?? "[Unknown Item]";
    }
}