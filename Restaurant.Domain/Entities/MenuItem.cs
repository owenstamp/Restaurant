using System;

namespace Restaurant.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }          // Maps to MenuItemID
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; private set; }  // e.g. Appetizer, Main, Dessert
        /// <summary>
        /// Gets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price { get; private set; }
        /// <summary>
        /// Gets the special dietary information.
        /// </summary>
        /// <value>
        /// The special dietary information.
        /// </value>
        public string SpecialDietaryInfo { get; private set; } // e.g., Vegan, Gluten-Free
        /// <summary>
        /// Gets a value indicating whether this instance is available.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is available; otherwise, <c>false</c>.
        /// </value>
        public bool IsAvailable { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="category">The category.</param>
        /// <param name="price">The price.</param>
        /// <param name="isAvailable">if set to <c>true</c> [is available].</param>
        /// <param name="specialDietaryInfo">The special dietary information.</param>
        public MenuItem(string name, string category, decimal price, bool isAvailable, string specialDietaryInfo = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Category = category;
            Price = price;
            SpecialDietaryInfo = specialDietaryInfo;
            IsAvailable = isAvailable;
        }

        // Domain logic
        /// <summary>
        /// Updates the price.
        /// </summary>
        /// <param name="newPrice">The new price.</param>
        public void UpdatePrice(decimal newPrice)
        {
            // Possibly add checks like "Cannot be negative"
            Price = newPrice;
        }

        /// <summary>
        /// Sets the availability.
        /// </summary>
        /// <param name="available">if set to <c>true</c> [available].</param>
        public void SetAvailability(bool available)
        {
            IsAvailable = available;
        }
    }
}
