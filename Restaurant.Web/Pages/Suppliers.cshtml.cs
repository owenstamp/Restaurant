using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Data;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class SuppliersModel : PageModel
    {
        private readonly AppDbContext _context;

        public SuppliersModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string SupplierName { get; set; }

        [BindProperty]
        public string ContactInfo { get; set; }

        [BindProperty]
        public string ProductTypes { get; set; }

        public List<Supplier> Suppliers { get; set; } = new();

        public string Message { get; set; }

        public void OnGet()
        {
            Suppliers = _context.Suppliers.ToList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(SupplierName))
            {
                Message = "Please fill in all required fields.";
                Suppliers = _context.Suppliers.ToList();
                return Page();
            }

            var newSupplier = new Supplier(SupplierName, ContactInfo, ProductTypes);
            _context.Suppliers.Add(newSupplier);
            _context.SaveChanges();

            Message = "Supplier added successfully.";
            Suppliers = _context.Suppliers.ToList();
            return Page();
        }
    }
}
