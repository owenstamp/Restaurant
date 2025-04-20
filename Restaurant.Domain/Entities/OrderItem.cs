using System;

namespace Restaurant.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; private set; }         // "OrderItemID"
        public Guid MenuItemId { get; private set; } // Link to MenuItems.MenuItemID
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }   // Store snapshot price
        public string SpecialRequests { get; private set; }

        // For EF relationship if needed:
        // public Guid OrderId { get; private set; }
        // public Order Order { get; private set; }

        // Constructor
        public OrderItem(Guid menuItemId, int quantity, decimal price, string specialRequests = null)
        {
            Id = Guid.NewGuid();
            MenuItemId = menuItemId;
            Quantity = quantity;
            Price = price;
            SpecialRequests = specialRequests;
        }

        // Example domain logic
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be positive.");
            Quantity = newQuantity;
        }
    }
}
