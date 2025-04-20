using System;
using System.Collections.Generic;

namespace Restaurant.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; }             // Matches "OrderID"
        public Guid? ReservationId { get; private set; } // If linked to a Reservation, else null
        public Guid WaitstaffId { get; private set; }    // Waitstaff → Staff.StaffID
        public DateTime OrderDateTime { get; private set; }
        public decimal TotalAmount { get; private set; }
        public OrderStatus Status { get; private set; }

        // Keep a collection of OrderItems
        private List<OrderItem> _items = new List<OrderItem>();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public enum OrderStatus
        {
            Received,
            Preparing,
            Ready,
            Served
        }

        // Constructor
        public Order(Guid waitstaffId, Guid? reservationId = null)
        {
            Id = Guid.NewGuid();
            WaitstaffId = waitstaffId;
            ReservationId = reservationId;
            OrderDateTime = DateTime.UtcNow;
            Status = OrderStatus.Received;
        }

        // Add an order item
        public void AddItem(OrderItem item)
        {
            _items.Add(item);
            RecalculateTotal();
        }

        // Example domain rule: recalc total
        private void RecalculateTotal()
        {
            decimal sum = 0;
            foreach (var item in _items)
            {
                sum += item.Price * item.Quantity;
            }
            TotalAmount = sum;
        }

        public void UpdateStatus(OrderStatus newStatus)
        {
            // e.g., you could validate valid transitions (Received -> Preparing, etc.)
            Status = newStatus;
        }
    }
}
