using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class ReservationsModel : PageModel
    {
        /// <summary>
        /// The reservation repo
        /// </summary>
        private readonly IReservationRepository _reservationRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationsModel"/> class.
        /// </summary>
        /// <param name="reservationRepo">The reservation repo.</param>
        public ReservationsModel(IReservationRepository reservationRepo)
        {
            _reservationRepo = reservationRepo;
        }

        /// <summary>
        /// Gets or sets the reservation date time.
        /// </summary>
        /// <value>
        /// The reservation date time.
        /// </value>
        [BindProperty]
        public DateTime ReservationDateTime { get; set; }

        /// <summary>
        /// Gets or sets the number of guests.
        /// </summary>
        /// <value>
        /// The number of guests.
        /// </value>
        [BindProperty]
        public int NumberOfGuests { get; set; }

        /// <summary>
        /// Gets or sets the table number.
        /// </summary>
        /// <value>
        /// The table number.
        /// </value>
        [BindProperty]
        public string TableNumber { get; set; }

        /// <summary>
        /// Gets or sets the reservations.
        /// </summary>
        /// <value>
        /// The reservations.
        /// </value>
        public List<Reservation> Reservations { get; set; } = new();

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Called when [get].
        /// </summary>
        public void OnGet()
        {
            // load all future reservations
            Reservations = _reservationRepo
                .GetByDateRange(DateTime.UtcNow, DateTime.MaxValue)
                .OrderBy(r => r.ReservationDateTime)
                .ToList();
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <returns></returns>
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
