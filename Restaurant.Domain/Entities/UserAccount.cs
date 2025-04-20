using System;

namespace Restaurant.Domain.Entities
{
    public class UserAccount
    {
        public Guid Id { get; private set; }        // UserID
        public Guid StaffId { get; private set; }   // Link to Staff
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public string Role { get; private set; }    // e.g. Admin, Waitstaff
        public bool IsActive { get; private set; }

        public UserAccount(Guid staffId, string username, string passwordHash, string role)
        {
            Id = Guid.NewGuid();
            StaffId = staffId;
            Username = username;
            PasswordHash = passwordHash;
            Role = role;
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void UpdatePassword(string newHash)
        {
            PasswordHash = newHash;
        }
    }
}
