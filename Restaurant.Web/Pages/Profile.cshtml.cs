using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;

namespace Restaurant.Web.Pages
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class ProfileModel : PageModel
    {
        /// <summary>
        /// The account repo
        /// </summary>
        private readonly IUserAccountRepository _accountRepo;
        /// <summary>
        /// The staff repo
        /// </summary>
        private readonly IStaffRepository _staffRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileModel"/> class.
        /// </summary>
        /// <param name="accountRepo">The account repo.</param>
        /// <param name="staffRepo">The staff repo.</param>
        public ProfileModel(
            IUserAccountRepository accountRepo,
            IStaffRepository staffRepo)
        {
            _accountRepo = accountRepo;
            _staffRepo = staffRepo;
        }

        // Exposed to the Razor page
        /// <summary>
        /// Gets the <see cref="T:System.Security.Claims.ClaimsPrincipal" /> for user associated with the executing action.
        /// </summary>
        public UserAccount? User { get; set; }
        /// <summary>
        /// Gets or sets the staff.
        /// </summary>
        /// <value>
        /// The staff.
        /// </value>
        public Staff? Staff { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        // Bind only the fields you allow to be updated
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [BindProperty] public string? Email { get; set; }
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        [BindProperty] public string? PhoneNumber { get; set; }

        /// <summary>
        /// Called when [get].
        /// </summary>
        /// <returns></returns>
        public IActionResult OnGet()
        {
            var staffIdStr = HttpContext.Session.GetString("StaffId");
            if (string.IsNullOrEmpty(staffIdStr)
                || !Guid.TryParse(staffIdStr, out var staffId))
            {
                return RedirectToPage("/Login");
            }

            // Load via repos
            User = _accountRepo.GetByStaffId(staffId);
            Staff = _staffRepo.GetById(staffId);

            if (Staff == null || User == null)
                return NotFound();

            // Pre-fill bind properties
            Email = Staff.Email;
            PhoneNumber = Staff.PhoneNumber;

            return Page();
        }

        /// <summary>
        /// Called when [post update profile].
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostUpdateProfile()
        {
            var staffIdStr = HttpContext.Session.GetString("StaffId");
            if (string.IsNullOrEmpty(staffIdStr)
                || !Guid.TryParse(staffIdStr, out var staffId))
            {
                return RedirectToPage("/Login");
            }

            var staff = _staffRepo.GetById(staffId);
            if (staff == null)
                return NotFound();

            // Apply changes
            staff.UpdateContactInfo(Email ?? staff.Email, PhoneNumber ?? staff.PhoneNumber);
            _staffRepo.Update(staff);

            Message = "Profile updated successfully.";
            return RedirectToPage();
        }
    }
}
