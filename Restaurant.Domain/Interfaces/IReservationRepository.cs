using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    public interface IReservationRepository
    {
        Reservation GetById(Guid id);
        IEnumerable<Reservation> GetAll();
        void Add(Reservation reservation);
        void Update(Reservation reservation);

        // Maybe a method for finding reservations in a date range:
        IEnumerable<Reservation> GetByDateRange(DateTime start, DateTime end);
    }
}
