using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Data;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class StaffModel : PageModel
    {
        private readonly AppDbContext _context;

        public StaffModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty] public string FirstName { get; set; }
        [BindProperty] public string LastName { get; set; }
        [BindProperty] public string Email { get; set; }
        [BindProperty] public string PhoneNumber { get; set; }
        [BindProperty] public string Role { get; set; }

        public List<Staff> StaffList { get; set; } = new();
        public string Message { get; set; }

        public void OnGet()
        {
            StaffList = _context.Staff.OrderBy(s => s.FirstName).ToList();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(Email))
            {
                Message = "Please fill out all required fields.";
                OnGet();
                return Page();
            }

            var newStaff = new Staff(FirstName, LastName, Email, PhoneNumber, Role);
            _context.Staff.Add(newStaff);
            _context.SaveChanges();

            Message = "Staff member added successfully.";

            // Generate temp password
            string tempPassword = $"{Role}123!";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(tempPassword);

            // Create linked UserAccount
            var userAccount = new UserAccount(
                newStaff.Id,
                username: Email, // use email as the login
                passwordHash: hashedPassword,
                role: Role
            );

            _context.UserAccounts.Add(userAccount);
            _context.SaveChanges();

            // Display temp login info
            Message = $"Staff member added successfully. Temp login: {Email}, Password: {tempPassword}";

            OnGet();
            return Page();
        }
    }
}
