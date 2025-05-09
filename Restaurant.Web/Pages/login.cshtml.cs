using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using BCrypt.Net;

namespace Restaurant.Web.Pages
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class LoginModel : PageModel
    {
        /// <summary>
        /// The accounts
        /// </summary>
        private readonly IUserAccountRepository _accounts;
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginModel"/> class.
        /// </summary>
        /// <param name="accounts">The accounts.</param>
        public LoginModel(IUserAccountRepository accounts)
        {
            _accounts = accounts;
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [BindProperty] public string Username { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [BindProperty] public string Password { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [login failed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [login failed]; otherwise, <c>false</c>.
        /// </value>
        public bool LoginFailed { get; set; }

        /// <summary>
        /// Called when [get].
        /// </summary>
        public void OnGet() { }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <returns></returns>
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
