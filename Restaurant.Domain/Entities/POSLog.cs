using System;

namespace Restaurant.Domain.Entities
{
    public class POSLog
    {
        public Guid Id { get; private set; }       // POSLogID
        public Guid OrderId { get; private set; }    // Link to Orders.OrderID
        public Guid ProcessedBy { get; private set; } // Link to Staff.StaffID
        public DateTime Timestamp { get; private set; }
        public decimal TotalAmount { get; private set; }
        public string DeviceID { get; private set; }  // Note: property name "DeviceID"

        // Parameterless constructor for EF Core
        protected POSLog()
        {
            // EF Core can use this constructor to materialize data.
        }

        // Your primary constructor – parameter names should ideally match property names
        public POSLog(Guid orderId, Guid processedBy, decimal totalAmount, string deviceID = null)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            ProcessedBy = processedBy;
            TotalAmount = totalAmount;
            Timestamp = DateTime.UtcNow;
            DeviceID = deviceID;
        }
    }
}
