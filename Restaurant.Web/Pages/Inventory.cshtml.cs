using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Data;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class InventoryModel : PageModel
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>
        /// The name of the item.
        /// </value>
        [BindProperty]
        public string ItemName { get; set; }

        /// <summary>
        /// Gets or sets the quantity on hand.
        /// </summary>
        /// <value>
        /// The quantity on hand.
        /// </value>
        [BindProperty]
        public int QuantityOnHand { get; set; }

        /// <summary>
        /// Gets or sets the reorder level.
        /// </summary>
        /// <value>
        /// The reorder level.
        /// </value>
        [BindProperty]
        public int ReorderLevel { get; set; }

        /// <summary>
        /// Gets or sets the supplier identifier.
        /// </summary>
        /// <value>
        /// The supplier identifier.
        /// </value>
        [BindProperty]
        public Guid SupplierId { get; set; }

        /// <summary>
        /// Gets or sets the inventory items.
        /// </summary>
        /// <value>
        /// The inventory items.
        /// </value>
        public List<InventoryItem> InventoryItems { get; set; } = new();
        /// <summary>
        /// Gets or sets the suppliers.
        /// </summary>
        /// <value>
        /// The suppliers.
        /// </value>
        public List<Supplier> Suppliers { get; set; } = new();

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// The inventory repo
        /// </summary>
        private readonly IInventoryItemRepository _inventoryRepo;
        /// <summary>
        /// The supplier repo
        /// </summary>
        private readonly ISupplierRepository _supplierRepo;
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryModel"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="inventoryRepo">The inventory repo.</param>
        /// <param name="supplierRepo">The supplier repo.</param>
        public InventoryModel(AppDbContext context, IInventoryItemRepository inventoryRepo, ISupplierRepository supplierRepo)
        {
            _context = context;
            _inventoryRepo = inventoryRepo;
            _supplierRepo = supplierRepo;
        }


        /// <summary>
        /// Called when [get].
        /// </summary>
        public void OnGet()
        {
            InventoryItems = _inventoryRepo.GetAll().ToList();
            Suppliers = _supplierRepo.GetAll().ToList();
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            Suppliers = _supplierRepo.GetAll().ToList();
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(ItemName))
            {
                Message = "Please fill in all required fields.";
                InventoryItems = _inventoryRepo.GetAll().ToList();
                return Page();
            }

            var newItem = new InventoryItem(ItemName, QuantityOnHand, ReorderLevel, SupplierId);
            _inventoryRepo.Add(newItem);

            Message = "Inventory item added successfully.";
            InventoryItems = _inventoryRepo.GetAll().ToList();
            return Page();
        }


        /// <summary>
        /// Gets the name of the supplier.
        /// </summary>
        /// <param name="supplierId">The supplier identifier.</param>
        /// <returns></returns>
        public string GetSupplierName(Guid supplierId)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == supplierId);
            return supplier != null ? supplier.SupplierName : "Unknown";
        }
    }
}
