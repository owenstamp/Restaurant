using System;

namespace Restaurant.Domain.Entities
{
    public class MenuItem
    {
        public Guid Id { get; private set; }          // Maps to MenuItemID
        public string Name { get; private set; }
        public string Category { get; private set; }  // e.g. Appetizer, Main, Dessert
        public decimal Price { get; private set; }
        public string SpecialDietaryInfo { get; private set; } // e.g., Vegan, Gluten-Free
        public bool IsAvailable { get; private set; }

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
        public void UpdatePrice(decimal newPrice)
        {
            // Possibly add checks like "Cannot be negative"
            Price = newPrice;
        }

        public void SetAvailability(bool available)
        {
            IsAvailable = available;
        }
    }
}
