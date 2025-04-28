using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using BCrypt.Net;

namespace Restaurant.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserAccountRepository _accounts;
        public LoginModel(IUserAccountRepository accounts)
        {
            _accounts = accounts;
        }

        [BindProperty] public string Username { get; set; }
        [BindProperty] public string Password { get; set; }
        public bool LoginFailed { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            // 1) Fetch via repo instead of DbContext
            var account = _accounts.GetByUsername(Username);

            // 2) Validate password & active flag
            if (account != null
                && BCrypt.Net.BCrypt.Verify(Password, account.PasswordHash)
                && account.IsActive)
            {
                HttpContext.Session.SetString("UserRole", account.Role);
                HttpContext.Session.SetString("StaffId", account.StaffId.ToString());
                HttpContext.Session.SetString("Username", account.Username);
                return RedirectToPage("/Index");
            }

            // 3) On failure, re-render the page with an error
            LoginFailed = true;
            return Page();
        }
    }
}
