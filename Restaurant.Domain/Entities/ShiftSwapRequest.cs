using System;

namespace Restaurant.Domain.Entities
{
    public class ShiftSwapRequest
    {
        public Guid Id { get; private set; }           // SwapID
        public Guid RequestingStaffId { get; private set; }
        public Guid TargetStaffId { get; private set; }
        public Guid OriginalShiftId { get; private set; }
        public SwapStatus Status { get; private set; }
        public DateTime RequestedAt { get; private set; }

        public enum SwapStatus
        {
            Pending,
            Approved,
            Rejected
        }

        public ShiftSwapRequest(Guid requestingStaffId, Guid targetStaffId, Guid originalShiftId)
        {
            Id = Guid.NewGuid();
            RequestingStaffId = requestingStaffId;
            TargetStaffId = targetStaffId;
            OriginalShiftId = originalShiftId;
            Status = SwapStatus.Pending;
            RequestedAt = DateTime.UtcNow;
        }

        public void Approve()
        {
            if (Status == SwapStatus.Rejected)
                throw new InvalidOperationException("Cannot approve a request that was already rejected.");
            Status = SwapStatus.Approved;
        }

        public void Reject()
        {
            if (Status == SwapStatus.Approved)
                throw new InvalidOperationException("Cannot reject a request that was already approved.");
            Status = SwapStatus.Rejected;
        }
    }
}
