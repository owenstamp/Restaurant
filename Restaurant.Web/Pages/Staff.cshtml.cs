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
    public class StaffModel : PageModel
    {
        /// <summary>
        /// The staff repo
        /// </summary>
        private readonly IStaffRepository _staffRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffModel"/> class.
        /// </summary>
        /// <param name="staffRepo">The staff repo.</param>
        public StaffModel(IStaffRepository staffRepo)
        {
            _staffRepo = staffRepo;
        }

        // Display all staff members
        /// <summary>
        /// Gets or sets the staff list.
        /// </summary>
        /// <value>
        /// The staff list.
        /// </value>
        public List<Staff> StaffList { get; set; } = new();

        // Bound properties for creating a new Staff
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [BindProperty] public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [BindProperty] public string LastName { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [BindProperty] public string Email { get; set; }
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        [BindProperty] public string PhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        [BindProperty] public string Role { get; set; }

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
            StaffList = _staffRepo.GetAll().ToList();
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <returns></returns>
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
