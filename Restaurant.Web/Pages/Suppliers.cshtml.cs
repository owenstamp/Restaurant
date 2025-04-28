using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;

namespace Restaurant.Web.Pages
{
    public class SuppliersModel : PageModel
    {
        private readonly ISupplierRepository _repo;
        public SuppliersModel(ISupplierRepository repo)
        {
            _repo = repo;
        }

        public List<Supplier> Suppliers { get; set; } = new();
        [BindProperty] public string SupplierName { get; set; }
        [BindProperty] public string ContactInfo { get; set; }
        [BindProperty] public string ProductTypes { get; set; }
        public string Message { get; set; }

        public void OnGet()
        {
            Suppliers = _repo.GetAll().ToList();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(SupplierName))
            {
                Message = "Name can’t be blank.";
                OnGet();
                return Page();
            }

            var supplier = new Supplier(SupplierName, ContactInfo, ProductTypes);

            _repo.Add(supplier);
            Message = "Supplier added!";
            OnGet();
            return Page();
        }
    }
}
