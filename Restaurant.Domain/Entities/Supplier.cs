using System;

namespace Restaurant.Domain.Entities
{
    public class Supplier
    {
        public Guid Id { get; private set; }           // SupplierID
        public string SupplierName { get; private set; }
        public string ContactInfo { get; private set; }
        public string ProductTypes { get; private set; } // e.g. Vegetables, Dairy

        public Supplier(string supplierName, string contactInfo, string productTypes)
        {
            Id = Guid.NewGuid();
            SupplierName = supplierName;
            ContactInfo = contactInfo;
            ProductTypes = productTypes;
        }
    }
}
