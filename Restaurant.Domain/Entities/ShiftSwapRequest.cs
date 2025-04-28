using System;

namespace Restaurant.Domain.Entities
{
    public class ShiftSwapRequest
    {
        public Guid Id { get; private set; }               // SwapID
        public Guid RequestingStaffId { get; private set; }
        public Guid TargetStaffId { get; private set; }
        public Guid OriginalShiftId { get; private set; }
        public SwapStatus Status { get; set; }
        public DateTime RequestedAt { get; private set; }

        public enum SwapStatus
        {
            PendingRecipient,        // Awaiting other staff member approval
            ApprovedByRecipient,     // Staff accepted, waiting for manager
            Approved,                // Fully approved by manager
            Rejected                 // Rejected at any stage
        }

        public ShiftSwapRequest(Guid requestingStaffId, Guid targetStaffId, Guid originalShiftId)
        {
            Id = Guid.NewGuid();
            RequestingStaffId = requestingStaffId;
            TargetStaffId = targetStaffId;
            OriginalShiftId = originalShiftId;
            Status = SwapStatus.PendingRecipient;
            RequestedAt = DateTime.UtcNow;
        }

        public void SetPendingRecipient()
        {
            Status = SwapStatus.PendingRecipient;
        }

        public void ApproveByRecipient()
        {
            if (Status != SwapStatus.PendingRecipient)
                throw new InvalidOperationException("Cannot mark as recipient-approved unless pending.");
            Status = SwapStatus.ApprovedByRecipient;
        }

        public void Approve()
        {
            if (Status == SwapStatus.Rejected)
                throw new InvalidOperationException("Cannot approve a request that was rejected.");
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
