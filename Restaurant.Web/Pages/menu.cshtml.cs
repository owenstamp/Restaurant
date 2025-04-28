using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class MenuModel : PageModel
    {
        private readonly IMenuItemRepository _menuRepo;

        public MenuModel(IMenuItemRepository menuRepo)
        {
            _menuRepo = menuRepo;
        }

        public List<MenuItem> MenuItems { get; set; } = new();

        [BindProperty] public string Name { get; set; }
        [BindProperty] public string Category { get; set; }
        [BindProperty] public decimal Price { get; set; }
        [BindProperty] public string SpecialDietaryInfo { get; set; }
        [BindProperty] public bool IsAvailable { get; set; }
        public string Message { get; set; }

        public void OnGet()
        {
            // Load all menu items
            MenuItems = _menuRepo.GetAll().ToList();
        }

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
