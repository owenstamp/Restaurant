using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Restaurant.Data;
using Restaurant.Domain.Entities;
using System.Linq;

namespace Restaurant.Web.Pages // 👈 This is the key part!
{
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;

        public LoginModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public bool LoginFailed { get; set; }

        public void OnGet()
        {
            // Optional: clear session on visiting login
            // HttpContext.Session.Clear();
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                LoginFailed = true;
                TempData["Debug"] = "❌ Empty fields";
                return Page();
            }

            var trimmedUsername = Username.Trim().ToLower();
            var allUsers = _context.UserAccounts.ToList();
            TempData["Debug"] = $"👥 Loaded {allUsers.Count} users";

            var account = allUsers.FirstOrDefault(u => u.Username?.Trim().ToLower() == trimmedUsername);

            if (account == null)
            {
                TempData["Debug"] = "🚫 No matching user.";
                LoginFailed = true;
                return Page();
            }

            if (!BCrypt.Net.BCrypt.Verify(Password, account.PasswordHash))
            {
                TempData["Debug"] = "🔐 Password incorrect.";
                LoginFailed = true;
                return Page();
            }

            HttpContext.Session.SetString("Username", account.Username);
            HttpContext.Session.SetString("UserRole", account.Role ?? "User");
            HttpContext.Session.SetString("StaffId", account.StaffId.ToString());

            TempData["Debug"] = $"🎉 Logged in as {account.Username}";

            return RedirectToPage("/Index");
        }

    }
}