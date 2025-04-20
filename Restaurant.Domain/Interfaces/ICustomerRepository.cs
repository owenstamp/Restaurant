using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Customer GetById(Guid id);
        IEnumerable<Customer> GetAll();
        void Add(Customer customer);
        void Update(Customer customer);

        // Maybe some domain-specific queries:
        Customer GetByEmail(string email);
    }
}
