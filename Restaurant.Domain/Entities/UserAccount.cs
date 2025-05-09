using System;

namespace Restaurant.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class UserAccount
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }        // UserID
        /// <summary>
        /// Gets the staff identifier.
        /// </summary>
        /// <value>
        /// The staff identifier.
        /// </value>
        public Guid StaffId { get; private set; }   // Link to Staff
        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; private set; }
        /// <summary>
        /// Gets the password hash.
        /// </summary>
        /// <value>
        /// The password hash.
        /// </value>
        public string PasswordHash { get; private set; }
        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public string Role { get; private set; }    // e.g. Admin, Waitstaff
        /// <summary>
        /// Gets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccount"/> class.
        /// </summary>
        /// <param name="staffId">The staff identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="passwordHash">The password hash.</param>
        /// <param name="role">The role.</param>
        public UserAccount(Guid staffId, string username, string passwordHash, string role)
        {
            Id = Guid.NewGuid();
            StaffId = staffId;
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
            IsActive = true;
        }

        /// <summary>
        /// Deactivates this instance.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Updates the password.
        /// </summary>
        /// <param name="newHash">The new hash.</param>
        public void UpdatePassword(string newHash)
        {
            PasswordHash = newHash;
        }
    }
}
