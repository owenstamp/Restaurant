using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;

namespace Restaurant.Web.Pages
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class SuppliersModel : PageModel
    {
        /// <summary>
        /// The repo
        /// </summary>
        private readonly ISupplierRepository _repo;
        /// <summary>
        /// Initializes a new instance of the <see cref="SuppliersModel"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        public SuppliersModel(ISupplierRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Gets or sets the suppliers.
        /// </summary>
        /// <value>
        /// The suppliers.
        /// </value>
        public List<Supplier> Suppliers { get; set; } = new();
        /// <summary>
        /// Gets or sets the name of the supplier.
        /// </summary>
        /// <value>
        /// The name of the supplier.
        /// </value>
        [BindProperty] public string SupplierName { get; set; }
        /// <summary>
        /// Gets or sets the contact information.
        /// </summary>
        /// <value>
        /// The contact information.
        /// </value>
        [BindProperty] public string ContactInfo { get; set; }
        /// <summary>
        /// Gets or sets the product types.
        /// </summary>
        /// <value>
        /// The product types.
        /// </value>
        [BindProperty] public string ProductTypes { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Called when [get].
        /// </summary>
        public void OnGet()
        {
            Suppliers = _repo.GetAll().ToList();
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <returns></returns>
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
