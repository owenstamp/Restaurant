using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class ReservationsModel : PageModel
    {
        private readonly IReservationRepository _reservationRepo;

        public ReservationsModel(IReservationRepository reservationRepo)
        {
            _reservationRepo = reservationRepo;
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
            // load all future reservations
            Reservations = _reservationRepo
                .GetByDateRange(DateTime.UtcNow, DateTime.MaxValue)
                .OrderBy(r => r.ReservationDateTime)
                .ToList();
        }

        public IActionResult OnPost()
        {
            var dummyCustomerId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            if (!ModelState.IsValid
                || NumberOfGuests <= 0
                || string.IsNullOrWhiteSpace(TableNumber))
            {
                Message = "Please enter all reservation details.";
                OnGet();
                return Page();
            }

            var reservation = new Reservation(dummyCustomerId, ReservationDateTime, NumberOfGuests, TableNumber);

            _reservationRepo.Add(reservation);
            // if your repository doesn’t save automatically, call SaveChanges here
            // e.g. (_reservationRepo as ReservationRepository)?._context.SaveChanges();

            Message = "Reservation created successfully.";
            OnGet();
            return Page();
        }
    }
}
