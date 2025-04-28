using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class StaffModel : PageModel
    {
        private readonly IStaffRepository _staffRepo;

        public StaffModel(IStaffRepository staffRepo)
        {
            _staffRepo = staffRepo;
        }

        // Display all staff members
        public List<Staff> StaffList { get; set; } = new();

        // Bound properties for creating a new Staff
        [BindProperty] public string FirstName { get; set; }
        [BindProperty] public string LastName { get; set; }
        [BindProperty] public string Email { get; set; }
        [BindProperty] public string PhoneNumber { get; set; }
        [BindProperty] public string Role { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
            StaffList = _staffRepo.GetAll().ToList();
        }

        public IActionResult OnPost()
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Role))
            {
                Message = "Please fill in all required fields.";
                OnGet();
                return Page();
            }

            // Create and save
            var newStaff = new Staff(FirstName, LastName, Email, PhoneNumber, Role);

            _staffRepo.Add(newStaff);
            Message = "New staff member added.";

            // Refresh list
            OnGet();
            return Page();
        }
    }
}
