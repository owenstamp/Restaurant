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
    public class OrdersModel : PageModel
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMenuItemRepository _menuRepo;
        // REMOVED: IHttpContextAccessor dependency

        // Session Key constant remains the same
        private const string SessionKeyCurrentOrderId = "_CurrentOrderId";

        // Constructor updated - no longer needs IHttpContextAccessor
        public OrdersModel(IOrderRepository orderRepo, IMenuItemRepository menuRepo)
        {
            _orderRepo = orderRepo;
            _menuRepo = menuRepo;
            // REMOVED: _httpContextAccessor assignment
        }

        // Properties for the "Add Item" form
        [BindProperty] public Guid MenuItemId { get; set; }
        [BindProperty] public int Quantity { get; set; } = 1;
        [BindProperty] public string? SpecialRequest { get; set; }

        // Properties to display data on the page
        public List<MenuItem> MenuItems { get; set; } = new();
        public Order? CurrentOrder { get; set; }
        public List<OrderItemViewModel> CurrentOrderItems { get; set; } = new();
        public List<Order> RecentOrders { get; set; } = new();
        public string? Message { get; set; }
        public string? ErrorMessage { get; set; }

        // ViewModel remains the same
        public class OrderItemViewModel
        {
            public Guid Id { get; set; }
            public string MenuItemName { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public string? SpecialRequests { get; set; }
            public decimal LineTotal => Quantity * Price;
        }

        // OnGet uses LoadPageData, which now uses HttpContext directly
        public void OnGet()
        {
            LoadPageData();
        }

        // Add Item handler - uses HttpContext directly
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
        public IActionResult OnPostCancelOrder()
        {
            // Use HttpContext directly
            HttpContext.Session.Remove(SessionKeyCurrentOrderId);
            Message = "Current order cancelled.";
            LoadPageData();
            return Page();
        }

        // LoadPageData helper - uses HttpContext directly
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
        public string GetMenuItemName(Guid id)
            => _menuRepo.GetById(id)?.Name ?? "[Unknown Item]";
    }
}