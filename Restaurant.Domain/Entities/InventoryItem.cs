using System;

namespace Restaurant.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class InventoryItem
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }             // InventoryItemID
        /// <summary>
        /// Gets the name of the item.
        /// </summary>
        /// <value>
        /// The name of the item.
        /// </value>
        public string ItemName { get; private set; }
        /// <summary>
        /// Gets the quantity on hand.
        /// </summary>
        /// <value>
        /// The quantity on hand.
        /// </value>
        public int QuantityOnHand { get; private set; }
        /// <summary>
        /// Gets the reorder level.
        /// </summary>
        /// <value>
        /// The reorder level.
        /// </value>
        public int ReorderLevel { get; private set; }
        /// <summary>
        /// Gets the supplier identifier.
        /// </summary>
        /// <value>
        /// The supplier identifier.
        /// </value>
        public Guid SupplierId { get; private set; }      // Link to Suppliers

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryItem"/> class.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="quantityOnHand">The quantity on hand.</param>
        /// <param name="reorderLevel">The reorder level.</param>
        /// <param name="supplierId">The supplier identifier.</param>
        public InventoryItem(string itemName, int quantityOnHand, int reorderLevel, Guid supplierId)
        {
            Id = Guid.NewGuid();
            ItemName = itemName;
            QuantityOnHand = quantityOnHand;
            ReorderLevel = reorderLevel;
            SupplierId = supplierId;
        }

        // Domain logic
        /// <summary>
        /// Adds the stock.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <exception cref="System.InvalidOperationException">Cannot add negative stock.</exception>
        public void AddStock(int amount)
        {
            if (amount < 0) throw new InvalidOperationException("Cannot add negative stock.");
            QuantityOnHand += amount;
        }

        /// <summary>
        /// Removes the stock.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <exception cref="System.InvalidOperationException">
        /// Cannot remove negative stock.
        /// or
        /// Not enough stock.
        /// </exception>
        public void RemoveStock(int amount)
        {
            if (amount < 0) throw new InvalidOperationException("Cannot remove negative stock.");
            if (QuantityOnHand - amount < 0) throw new InvalidOperationException("Not enough stock.");
            QuantityOnHand -= amount;
        }

        /// <summary>
        /// Needses the reorder.
        /// </summary>
        /// <returns></returns>
        public bool NeedsReorder() => QuantityOnHand <= ReorderLevel;
    }
}
