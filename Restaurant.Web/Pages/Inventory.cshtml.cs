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
    public class InventoryModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public string ItemName { get; set; }

        [BindProperty]
        public int QuantityOnHand { get; set; }

        [BindProperty]
        public int ReorderLevel { get; set; }

        [BindProperty]
        public Guid SupplierId { get; set; }

        public List<InventoryItem> InventoryItems { get; set; } = new();
        public List<Supplier> Suppliers { get; set; } = new();

        public string Message { get; set; }

        private readonly IInventoryItemRepository _inventoryRepo;
        private readonly ISupplierRepository _supplierRepo;
        public InventoryModel(AppDbContext context, IInventoryItemRepository inventoryRepo, ISupplierRepository supplierRepo)
        {
            _context = context;
            _inventoryRepo = inventoryRepo;
            _supplierRepo = supplierRepo;
        }


        public void OnGet()
        {
            InventoryItems = _inventoryRepo.GetAll().ToList();
            Suppliers = _supplierRepo.GetAll().ToList();
        }

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


        public string GetSupplierName(Guid supplierId)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == supplierId);
            return supplier != null ? supplier.SupplierName : "Unknown";
        }
    }
}
