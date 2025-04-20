using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    public interface IInventoryItemRepository
    {
        InventoryItem GetById(Guid id);
        void Add(InventoryItem item);
        void Update(InventoryItem item);
        IEnumerable<InventoryItem> GetAll();

        // Possibly for checking reorder
        IEnumerable<InventoryItem> GetBelowReorderLevel();
    }

    public interface ISupplierRepository
    {
        Supplier GetById(Guid id);
        void Add(Supplier supplier);
        void Update(Supplier supplier);
        IEnumerable<Supplier> GetAll();
    }
}
