using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    public class MenuModel : PageModel
    {
        /// <summary>
        /// The menu repo
        /// </summary>
        private readonly IMenuItemRepository _menuRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuModel"/> class.
        /// </summary>
        /// <param name="menuRepo">The menu repo.</param>
        public MenuModel(IMenuItemRepository menuRepo)
        {
            _menuRepo = menuRepo;
        }

        /// <summary>
        /// Gets or sets the menu items.
        /// </summary>
        /// <value>
        /// The menu items.
        /// </value>
        public List<MenuItem> MenuItems { get; set; } = new();

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [BindProperty] public string Name { get; set; }
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [BindProperty] public string Category { get; set; }
        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        [BindProperty] public decimal Price { get; set; }
        /// <summary>
        /// Gets or sets the special dietary information.
        /// </summary>
        /// <value>
        /// The special dietary information.
        /// </value>
        [BindProperty] public string SpecialDietaryInfo { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is available.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is available; otherwise, <c>false</c>.
        /// </value>
        [BindProperty] public bool IsAvailable { get; set; }
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
            // Load all menu items
            MenuItems = _menuRepo.GetAll().ToList();
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            // Simple validation
            if (string.IsNullOrWhiteSpace(Name)
                || string.IsNullOrWhiteSpace(Category)
                || Price <= 0m)
            {
                Message = "Please fill in Name, Category, and a positive Price.";
                OnGet();
                return Page();
            }

            // Create via domain constructor (Id inside) or manually set Id if needed
            MenuItem newItem = new(
                Name,
                Category,
                Price,
                IsAvailable,
                SpecialDietaryInfo

            );

            _menuRepo.Add(newItem);
            Message = "Menu item added successfully.";

            // Redirect to clear the form and reload list
            return RedirectToPage();
        }
    }
}
