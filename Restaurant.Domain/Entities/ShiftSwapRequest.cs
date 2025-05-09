using System;

namespace Restaurant.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class ShiftSwapRequest
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }               // SwapID
        /// <summary>
        /// Gets the requesting staff identifier.
        /// </summary>
        /// <value>
        /// The requesting staff identifier.
        /// </value>
        public Guid RequestingStaffId { get; private set; }
        /// <summary>
        /// Gets the target staff identifier.
        /// </summary>
        /// <value>
        /// The target staff identifier.
        /// </value>
        public Guid TargetStaffId { get; private set; }
        /// <summary>
        /// Gets the original shift identifier.
        /// </summary>
        /// <value>
        /// The original shift identifier.
        /// </value>
        public Guid OriginalShiftId { get; private set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public SwapStatus Status { get; set; }
        /// <summary>
        /// Gets the requested at.
        /// </summary>
        /// <value>
        /// The requested at.
        /// </value>
        public DateTime RequestedAt { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public enum SwapStatus
        {
            /// <summary>
            /// The pending recipient
            /// </summary>
            PendingRecipient,        // Awaiting other staff member approval
            /// <summary>
            /// The approved by recipient
            /// </summary>
            ApprovedByRecipient,     // Staff accepted, waiting for manager
            /// <summary>
            /// The approved
            /// </summary>
            Approved,                // Fully approved by manager
            /// <summary>
            /// The rejected
            /// </summary>
            Rejected                 // Rejected at any stage
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShiftSwapRequest"/> class.
        /// </summary>
        /// <param name="requestingStaffId">The requesting staff identifier.</param>
        /// <param name="targetStaffId">The target staff identifier.</param>
        /// <param name="originalShiftId">The original shift identifier.</param>
        public ShiftSwapRequest(Guid requestingStaffId, Guid targetStaffId, Guid originalShiftId)
        {
            Id = Guid.NewGuid();
            RequestingStaffId = requestingStaffId;
            TargetStaffId = targetStaffId;
            OriginalShiftId = originalShiftId;
            Status = SwapStatus.PendingRecipient;
            RequestedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Sets the pending recipient.
        /// </summary>
        public void SetPendingRecipient()
        {
            Status = SwapStatus.PendingRecipient;
        }

        /// <summary>
        /// Approves the by recipient.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot mark as recipient-approved unless pending.</exception>
        public void ApproveByRecipient()
        {
            if (Status != SwapStatus.PendingRecipient)
                throw new InvalidOperationException("Cannot mark as recipient-approved unless pending.");
            Status = SwapStatus.ApprovedByRecipient;
        }

        /// <summary>
        /// Approves this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot approve a request that was rejected.</exception>
        public void Approve()
        {
            if (Status == SwapStatus.Rejected)
                throw new InvalidOperationException("Cannot approve a request that was rejected.");
            Status = SwapStatus.Approved;

        }

        /// <summary>
        /// Rejects this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot reject a request that was already approved.</exception>
        public void Reject()
        {
            if (Status == SwapStatus.Approved)
                throw new InvalidOperationException("Cannot reject a request that was already approved.");
            Status = SwapStatus.Rejected;
        }
    }
}
