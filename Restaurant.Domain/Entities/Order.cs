using System;
using System.Collections.Generic;

namespace Restaurant.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }             // Matches "OrderID"
        /// <summary>
        /// Gets the reservation identifier.
        /// </summary>
        /// <value>
        /// The reservation identifier.
        /// </value>
        public Guid? ReservationId { get; private set; } // If linked to a Reservation, else null
        /// <summary>
        /// Gets the waitstaff identifier.
        /// </summary>
        /// <value>
        /// The waitstaff identifier.
        /// </value>
        public Guid WaitstaffId { get; private set; }    // Waitstaff → Staff.StaffID
        /// <summary>
        /// Gets the order date time.
        /// </summary>
        /// <value>
        /// The order date time.
        /// </value>
        public DateTime OrderDateTime { get; private set; }
        /// <summary>
        /// Gets the total amount.
        /// </summary>
        /// <value>
        /// The total amount.
        /// </value>
        public decimal TotalAmount { get; internal set; }
        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public OrderStatus Status { get; private set; }

        // Keep a collection of OrderItems
        /// <summary>
        /// The items
        /// </summary>
        private List<OrderItem> _items = new List<OrderItem>();
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        /// <summary>
        /// 
        /// </summary>
        public enum OrderStatus
        {
            /// <summary>
            /// The received
            /// </summary>
            Received,
            /// <summary>
            /// The preparing
            /// </summary>
            Preparing,
            /// <summary>
            /// The ready
            /// </summary>
            Ready,
            /// <summary>
            /// The served
            /// </summary>
            Served
        }

        // Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        /// <param name="waitstaffId">The waitstaff identifier.</param>
        /// <param name="reservationId">The reservation identifier.</param>
        public Order(Guid waitstaffId, Guid? reservationId = null)
        {
            Id = Guid.NewGuid();
            WaitstaffId = waitstaffId;
            ReservationId = reservationId;
            OrderDateTime = DateTime.UtcNow;
            Status = OrderStatus.Received;
        }

        // Add an order item
        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void AddItem(OrderItem item)
        {
            _items.Add(item);
            RecalculateTotal();
        }

        // Example domain rule: recalc total
        /// <summary>
        /// Recalculates the total.
        /// </summary>
        public void RecalculateTotal()
        {
            decimal sum = 0;
            foreach (var item in _items)
            {
                sum += item.Price * item.Quantity;
            }
            TotalAmount = sum;
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        /// <param name="newStatus">The new status.</param>
        public void UpdateStatus(OrderStatus newStatus)
        {
            // e.g., you could validate valid transitions (Received -> Preparing, etc.)
            Status = newStatus;
        }
    }
}
