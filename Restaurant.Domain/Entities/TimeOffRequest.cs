using System;

namespace Restaurant.Domain.Entities
{
    public class TimeOffRequest
    {
        public Guid Id { get; private set; }       // RequestID
        public Guid StaffId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string Reason { get; private set; }
        public RequestStatus Status { get; private set; }
        public DateTime RequestedAt { get; private set; }

        public enum RequestStatus
        {
            Pending,
            Approved,
            Denied
        }

        public TimeOffRequest(Guid staffId, DateTime startDate, DateTime endDate, string reason)
        {
            Id = Guid.NewGuid();
            StaffId = staffId;
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
            Status = RequestStatus.Pending;
            RequestedAt = DateTime.UtcNow;
        }

        public void Approve()
        {
            if (Status == RequestStatus.Denied)
                throw new InvalidOperationException("Cannot approve a request already denied.");
            Status = RequestStatus.Approved;
        }

        public void Deny()
        {
            if (Status == RequestStatus.Approved)
                throw new InvalidOperationException("Cannot deny a request already approved.");
            Status = RequestStatus.Denied;
        }
    }
}
