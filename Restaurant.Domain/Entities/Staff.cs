using System;

namespace Restaurant.Domain.Entities
{
    public class Staff
    {
        public Guid Id { get; private set; }    // StaffID
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Role { get; private set; } // e.g. Waitstaff, Chef, Manager
        public DateTime HireDate { get; private set; }

        public Staff(string firstName, string lastName, string email, string phoneNumber, string role)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Role = role;
            HireDate = DateTime.UtcNow;
        }
    }
}
