using System;
using System.Collections.Generic;
using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReservationRepository
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Reservation GetById(Guid id);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Reservation> GetAll();
        /// <summary>
        /// Adds the specified reservation.
        /// </summary>
        /// <param name="reservation">The reservation.</param>
        void Add(Reservation reservation);
        /// <summary>
        /// Updates the specified reservation.
        /// </summary>
        /// <param name="reservation">The reservation.</param>
        void Update(Reservation reservation);
        /// <summary>
        /// Gets the by date range.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        IEnumerable<Reservation> GetByDateRange(DateTime start, DateTime end);
    }
}
