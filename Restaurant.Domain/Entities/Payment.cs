using System;

namespace Restaurant.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; private set; }       // PaymentID
        public Guid OrderId { get; private set; }  // Link to Orders
        public decimal AmountPaid { get; private set; }
        public DateTime PaymentDateTime { get; private set; }
        public PaymentMethod Method { get; private set; }
        public PaymentStatus Status { get; private set; }

        public enum PaymentMethod
        {
            CreditCard,
            Cash,
            Digital
        }

        public enum PaymentStatus
        {
            Pending,
            Completed
        }

        public Payment(Guid orderId, decimal amountPaid, PaymentMethod method)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            AmountPaid = amountPaid;
            Method = method;
            Status = PaymentStatus.Pending;
            PaymentDateTime = DateTime.UtcNow;
        }

        public void MarkCompleted()
        {
            Status = PaymentStatus.Completed;
        }
    }
}
