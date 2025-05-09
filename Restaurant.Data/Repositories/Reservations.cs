using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Restaurant.Domain.Interfaces.IReservationRepository" />
    public class ReservationRepository : IReservationRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly AppDbContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Reservation GetById(Guid id)
        {
            return _context.Reservations.Find(id);
        }
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Reservation> GetAll()
        {
            return _context.Reservations.ToList();
        }
        /// <summary>
        /// Adds the specified reservation.
        /// </summary>
        /// <param name="reservation">The reservation.</param>
        public void Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
        }
        /// <summary>
        /// Updates the specified reservation.
        /// </summary>
        /// <param name="reservation">The reservation.</param>
        public void Update(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            _context.SaveChanges();
        }
        /// <summary>
        /// Gets the by date range.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns></returns>
        public IEnumerable<Reservation> GetByDateRange(DateTime start, DateTime end)
        {
            return _context.Reservations
                           .Where(r => r.ReservationDateTime >= start && r.ReservationDateTime <= end)
                           .ToList();
        }
    }
}
