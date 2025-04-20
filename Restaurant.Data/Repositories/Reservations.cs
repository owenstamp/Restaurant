using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Data.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly AppDbContext _context;
        public ReservationRepository(AppDbContext context)
        {
            _context = context;
        }
        public Reservation GetById(Guid id)
        {
            return _context.Reservations.Find(id);
        }
        public IEnumerable<Reservation> GetAll()
        {
            return _context.Reservations.ToList();
        }
        public void Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
        }
        public void Update(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            _context.SaveChanges();
        }
        public IEnumerable<Reservation> GetByDateRange(DateTime start, DateTime end)
        {
            return _context.Reservations
                           .Where(r => r.ReservationDateTime >= start && r.ReservationDateTime <= end)
                           .ToList();
        }
    }
}
