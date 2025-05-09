using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Customer GetById(Guid id);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Customer> GetAll();
        /// <summary>
        /// Adds the specified customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        void Add(Customer customer);
        /// <summary>
        /// Updates the specified customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        void Update(Customer customer);


        /// <summary>
        /// Gets the by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Customer GetByEmail(string email);
    }
}
