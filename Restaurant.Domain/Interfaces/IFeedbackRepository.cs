using System;
using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    public interface IFeedbackRepository
    {
        Feedback GetById(Guid id);
        void Add(Feedback feedback);

        // Add more if needed, e.g.:
        // IEnumerable<Feedback> GetAll();
        // IEnumerable<Feedback> GetByCustomerId(Guid customerId);
        // etc.
    }
}

