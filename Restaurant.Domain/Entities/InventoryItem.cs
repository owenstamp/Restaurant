using System;

namespace Restaurant.Domain.Entities
{
    public class InventoryItem
    {
        public Guid Id { get; private set; }             // InventoryItemID
        public string ItemName { get; private set; }
        public int QuantityOnHand { get; private set; }
        public int ReorderLevel { get; private set; }
        public Guid SupplierId { get; private set; }      // Link to Suppliers

        public InventoryItem(string itemName, int quantityOnHand, int reorderLevel, Guid supplierId)
        {
            Id = Guid.NewGuid();
            ItemName = itemName;
            QuantityOnHand = quantityOnHand;
            ReorderLevel = reorderLevel;
            SupplierId = supplierId;
        }

        // Domain logic
        public void AddStock(int amount)
        {
            if (amount < 0) throw new InvalidOperationException("Cannot add negative stock.");
            QuantityOnHand += amount;
        }

        public void RemoveStock(int amount)
        {
            if (amount < 0) throw new InvalidOperationException("Cannot remove negative stock.");
            if (QuantityOnHand - amount < 0) throw new InvalidOperationException("Not enough stock.");
            QuantityOnHand -= amount;
        }

        public bool NeedsReorder() => QuantityOnHand <= ReorderLevel;
    }
}
