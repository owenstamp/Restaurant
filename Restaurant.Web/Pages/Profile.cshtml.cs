using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;

namespace Restaurant.Web.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IUserAccountRepository _accountRepo;
        private readonly IStaffRepository _staffRepo;

        public ProfileModel(
            IUserAccountRepository accountRepo,
            IStaffRepository staffRepo)
        {
            _accountRepo = accountRepo;
            _staffRepo = staffRepo;
        }

        // Exposed to the Razor page
        public UserAccount? User { get; set; }
        public Staff? Staff { get; set; }
        public string Message { get; set; }

        // Bind only the fields you allow to be updated
        [BindProperty] public string? Email { get; set; }
        [BindProperty] public string? PhoneNumber { get; set; }

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
