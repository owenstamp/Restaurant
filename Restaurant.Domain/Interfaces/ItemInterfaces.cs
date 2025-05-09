using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInventoryItemRepository
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        InventoryItem GetById(Guid id);
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Add(InventoryItem item);
        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Update(InventoryItem item);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<InventoryItem> GetAll();

        /// <summary>
        /// Gets the below reorder level.
        /// </summary>
        /// <returns></returns>
        IEnumerable<InventoryItem> GetBelowReorderLevel();
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISupplierRepository
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Supplier GetById(Guid id);
        /// <summary>
        /// Adds the specified supplier.
        /// </summary>
        /// <param name="supplier">The supplier.</param>
        void Add(Supplier supplier);
        /// <summary>
        /// Updates the specified supplier.
        /// </summary>
        /// <param name="supplier">The supplier.</param>
        void Update(Supplier supplier);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Supplier> GetAll();
    }
}
