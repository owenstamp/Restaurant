using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Data;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class MenuModel : PageModel
    {
        private readonly AppDbContext _context;

        public MenuModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Name { get; set; }

        [BindProperty]
        public string Category { get; set; }

        [BindProperty]
        public decimal Price { get; set; }

        [BindProperty]
        public string SpecialDietaryInfo { get; set; }

        [BindProperty]
        public bool IsAvailable { get; set; }

        public List<MenuItem> MenuItems { get; set; } = new();

        public string Message { get; set; }

        public void OnGet()
        {
            MenuItems = _context.MenuItems.ToList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(Name))
            {
                Message = "Please fill in all required fields.";
                MenuItems = _context.MenuItems.ToList();
                return Page();
            }

            var newItem = new MenuItem(Name, Category, Price, SpecialDietaryInfo);
            newItem.SetAvailability(IsAvailable); // ✅ set IsAvailable via method

            _context.MenuItems.Add(newItem);
            _context.SaveChanges();

            Message = "Menu item added successfully.";
            MenuItems = _context.MenuItems.ToList();
            return Page();
        }

    }
}
