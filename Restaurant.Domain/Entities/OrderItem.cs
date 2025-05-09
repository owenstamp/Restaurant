using System;

namespace Restaurant.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }
        // "OrderItemID"
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        public Guid OrderId { get; set; }
        /// <summary>
        /// Gets the menu item identifier.
        /// </summary>
        /// <value>
        /// The menu item identifier.
        /// </value>
        public Guid MenuItemId { get; private set; } // Link to MenuItems.MenuItemID
        /// <summary>
        /// Gets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public int Quantity { get; private set; }
        /// <summary>
        /// Gets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price { get; private set; }   // Store snapshot price
        /// <summary>
        /// Gets the special requests.
        /// </summary>
        /// <value>
        /// The special requests.
        /// </value>
        public string SpecialRequests { get; private set; }


        // Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderItem"/> class.
        /// </summary>
        /// <param name="menuItemId">The menu item identifier.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="price">The price.</param>
        /// <param name="specialRequests">The special requests.</param>
        public OrderItem(Guid menuItemId, int quantity, decimal price, string specialRequests = null)
        {
            Id = Guid.NewGuid();
            MenuItemId = menuItemId;
            Quantity = quantity;
            Price = price;
            SpecialRequests = specialRequests;
        }

        // Example domain logic
        /// <summary>
        /// Updates the quantity.
        /// </summary>
        /// <param name="newQuantity">The new quantity.</param>
        /// <exception cref="System.ArgumentException">Quantity must be positive.</exception>
        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0)
                throw new ArgumentException("Quantity must be positive.");
            Quantity = newQuantity;
        }
    }
}
