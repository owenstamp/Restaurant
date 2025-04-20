using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Data;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class ReservationsModel : PageModel
    {
        private readonly AppDbContext _context;

        public ReservationsModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DateTime ReservationDateTime { get; set; }

        [BindProperty]
        public int NumberOfGuests { get; set; }

        [BindProperty]
        public string TableNumber { get; set; }

        public List<Reservation> Reservations { get; set; } = new();
        public string Message { get; set; }

        public void OnGet()
        {
            Reservations = _context.Reservations
                .OrderBy(r => r.ReservationDateTime)
                .Where(r => r.ReservationDateTime >= DateTime.UtcNow)
                .ToList();
        }

        public IActionResult OnPost()
        {
            // TEMP: until customer login is ready
            var dummyCustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            if (!ModelState.IsValid || NumberOfGuests <= 0 || string.IsNullOrWhiteSpace(TableNumber))
            {
                Message = "Please enter all reservation details.";
                OnGet();
                return Page();
            }

            var reservation = new Reservation(dummyCustomerId, ReservationDateTime, NumberOfGuests, TableNumber);
            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            Message = "Reservation created successfully.";
            OnGet();
            return Page();
        }
    }
}
