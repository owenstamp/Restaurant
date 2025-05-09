using System;

namespace Restaurant.Domain.Entities
{

    public class Customer
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }            // Maps to CustomerID
        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; private set; }
        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; private set; }
        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; private set; }
        /// <summary>
        /// Gets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; private set; }
        /// <summary>
        /// Gets the preferred table location.
        /// </summary>
        /// <value>
        /// The preferred table location.
        /// </value>
        public string PreferredTableLocation { get; private set; } // e.g., Window seat

        // Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Customer"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="preferredTableLocation">The preferred table location.</param>
        public Customer(string firstName, string lastName, string email, string phoneNumber, string preferredTableLocation = null)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            PreferredTableLocation = preferredTableLocation;
        }

        /// <summary>
        /// Updates the contact information.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="phone">The phone.</param>
        public void UpdateContactInfo(string email, string phone)
        {
            Email = email;
            PhoneNumber = phone;
        }

        /// <summary>
        /// Sets the preferred table location.
        /// </summary>
        /// <param name="location">The location.</param>
        public void SetPreferredTableLocation(string location)
        {
            PreferredTableLocation = location;
        }
    }
}
