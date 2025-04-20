using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Data;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class InventoryModel : PageModel
    {
        private readonly AppDbContext _context;

        public InventoryModel(AppDbContext context)
        {
            _context = context;
        }

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

        public void OnGet()
        {
            InventoryItems = _context.InventoryItems.ToList();
            Suppliers = _context.Suppliers.ToList();
        }

        public IActionResult OnPost()
        {
            Suppliers = _context.Suppliers.ToList();
            //
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(ItemName))
            {
                Message = "Please fill in all required fields.";
                InventoryItems = _context.InventoryItems.ToList();
                return Page();
            }

            var newItem = new InventoryItem(ItemName, QuantityOnHand, ReorderLevel, SupplierId);
            _context.InventoryItems.Add(newItem);
            _context.SaveChanges();

            Message = "Inventory item added successfully.";
            InventoryItems = _context.InventoryItems.ToList();
            return Page();
        }

        public string GetSupplierName(Guid supplierId)
        {
            var supplier = _context.Suppliers.FirstOrDefault(s => s.Id == supplierId);
            return supplier != null ? supplier.SupplierName : "Unknown";
        }
    }
}
