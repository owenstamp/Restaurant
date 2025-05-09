using System;

namespace Restaurant.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Staff
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }    // StaffID
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
        /// Gets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public string Role { get; private set; } // e.g. Waitstaff, Chef, Manager
        /// <summary>
        /// Gets the hire date.
        /// </summary>
        /// <value>
        /// The hire date.
        /// </value>
        public DateTime HireDate { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Staff"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <param name="role">The role.</param>
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

        /// <summary>
        /// Updates the contact information.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="phone">The phone.</param>
        /// <exception cref="System.ArgumentException">
        /// Email cannot be blank. - email
        /// or
        /// Phone cannot be blank. - phone
        /// </exception>
        public void UpdateContactInfo(string email, string phone)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be blank.", nameof(email));
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone cannot be blank.", nameof(phone));

            Email = email;
            PhoneNumber = phone;
        }
    }
}
