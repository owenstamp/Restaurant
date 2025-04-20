using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Data;
using Restaurant.Domain.Entities;
using System;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly AppDbContext _context;

        public ProfileModel(AppDbContext context)
        {
            _context = context;
        }

        public UserAccount? User { get; set; }
        public Staff? Staff { get; set; }

        public IActionResult OnGet()
        {
            var username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToPage("/Login");
            }

            User = _context.UserAccounts.FirstOrDefault(u => u.Username == username);

            if (User != null)
            {
                Staff = _context.Staff.FirstOrDefault(s => s.Id == User.StaffId);
            }

            return Page();
        }
    }
}
