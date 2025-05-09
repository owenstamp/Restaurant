using System;

namespace Restaurant.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Reservation
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }
        /// <summary>
        /// Gets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        public Guid CustomerId { get; private set; }
        /// <summary>
        /// Gets the reservation date time.
        /// </summary>
        /// <value>
        /// The reservation date time.
        /// </value>
        public DateTime ReservationDateTime { get; private set; }
        /// <summary>
        /// Gets the number of guests.
        /// </summary>
        /// <value>
        /// The number of guests.
        /// </value>
        public int NumberOfGuests { get; private set; }
        /// <summary>
        /// Gets the table number.
        /// </summary>
        /// <value>
        /// The table number.
        /// </value>
        public string TableNumber { get; private set; }
        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public ReservationStatus Status { get; private set; }
        /// <summary>
        /// Gets the created at.
        /// </summary>
        /// <value>
        /// The created at.
        /// </value>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public enum ReservationStatus
        {
            /// <summary>
            /// The confirmed
            /// </summary>
            Confirmed,
            /// <summary>
            /// The cancelled
            /// </summary>
            Cancelled,
            /// <summary>
            /// The no show
            /// </summary>
            NoShow
        }

        // Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Reservation"/> class.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="reservationDateTime">The reservation date time.</param>
        /// <param name="numberOfGuests">The number of guests.</param>
        /// <param name="tableNumber">The table number.</param>
        public Reservation(Guid customerId, DateTime reservationDateTime, int numberOfGuests, string tableNumber)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            ReservationDateTime = reservationDateTime;
            NumberOfGuests = numberOfGuests;
            TableNumber = tableNumber;
            Status = ReservationStatus.Confirmed;
            CreatedAt = DateTime.UtcNow;
        }

        // Example methods (domain rules)
        /// <summary>
        /// Cancels this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Reservation is already inactive.</exception>
        public void Cancel()
        {
            if (Status == ReservationStatus.Cancelled || Status == ReservationStatus.NoShow)
                throw new InvalidOperationException("Reservation is already inactive.");

            Status = ReservationStatus.Cancelled;
        }

        /// <summary>
        /// Marks the no show.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot mark a cancelled reservation as no-show.</exception>
        public void MarkNoShow()
        {
            if (Status == ReservationStatus.Cancelled)
                throw new InvalidOperationException("Cannot mark a cancelled reservation as no-show.");

            Status = ReservationStatus.NoShow;
        }
    }
}
