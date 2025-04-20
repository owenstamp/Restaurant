using System;

namespace Restaurant.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }            // Maps to CustomerID
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string PreferredTableLocation { get; private set; } // e.g., Window seat

        // Constructor
        public Customer(string firstName, string lastName, string email, string phoneNumber, string preferredTableLocation = null)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            PreferredTableLocation = preferredTableLocation;
        }

        // Example domain methods
        public void UpdateContactInfo(string email, string phone)
        {
            Email = email;
            PhoneNumber = phone;
        }

        public void SetPreferredTableLocation(string location)
        {
            PreferredTableLocation = location;
        }
    }
}
