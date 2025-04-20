using System;

namespace Restaurant.Domain.Entities
{
    public class Reservation
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public DateTime ReservationDateTime { get; private set; }
        public int NumberOfGuests { get; private set; }
        public string TableNumber { get; private set; }
        public ReservationStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public enum ReservationStatus
        {
            Confirmed,
            Cancelled,
            NoShow
        }

        // Constructor
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
        public void Cancel()
        {
            if (Status == ReservationStatus.Cancelled || Status == ReservationStatus.NoShow)
                throw new InvalidOperationException("Reservation is already inactive.");

            Status = ReservationStatus.Cancelled;
        }

        public void MarkNoShow()
        {
            if (Status == ReservationStatus.Cancelled)
                throw new InvalidOperationException("Cannot mark a cancelled reservation as no-show.");

            Status = ReservationStatus.NoShow;
        }
    }
}
