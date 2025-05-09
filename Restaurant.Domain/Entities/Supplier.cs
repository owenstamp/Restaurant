using System;

namespace Restaurant.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Supplier
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }           // SupplierID
        /// <summary>
        /// Gets the name of the supplier.
        /// </summary>
        /// <value>
        /// The name of the supplier.
        /// </value>
        public string SupplierName { get; private set; }
        /// <summary>
        /// Gets the contact information.
        /// </summary>
        /// <value>
        /// The contact information.
        /// </value>
        public string ContactInfo { get; private set; }
        /// <summary>
        /// Gets the product types.
        /// </summary>
        /// <value>
        /// The product types.
        /// </value>
        public string ProductTypes { get; private set; } // e.g. Vegetables, Dairy

        /// <summary>
        /// Initializes a new instance of the <see cref="Supplier"/> class.
        /// </summary>
        /// <param name="supplierName">Name of the supplier.</param>
        /// <param name="contactInfo">The contact information.</param>
        /// <param name="productTypes">The product types.</param>
        public Supplier(string supplierName, string contactInfo, string productTypes)
        {
            Id = Guid.NewGuid();
            SupplierName = supplierName;
            ContactInfo = contactInfo;
            ProductTypes = productTypes;
        }
    }
}
