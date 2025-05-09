using System;

namespace Restaurant.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class TimeOffRequest
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }       // RequestID
        /// <summary>
        /// Gets the staff identifier.
        /// </summary>
        /// <value>
        /// The staff identifier.
        /// </value>
        public Guid StaffId { get; private set; }
        /// <summary>
        /// Gets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; private set; }
        /// <summary>
        /// Gets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; private set; }
        /// <summary>
        /// Gets the reason.
        /// </summary>
        /// <value>
        /// The reason.
        /// </value>
        public string Reason { get; private set; }
        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public RequestStatus Status { get; private set; }
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
        public enum RequestStatus
        {
            /// <summary>
            /// The pending
            /// </summary>
            Pending,
            /// <summary>
            /// The approved
            /// </summary>
            Approved,
            /// <summary>
            /// The denied
            /// </summary>
            Denied
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOffRequest"/> class.
        /// </summary>
        /// <param name="staffId">The staff identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="reason">The reason.</param>
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

        /// <summary>
        /// Approves this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot approve a request already denied.</exception>
        public void Approve()
        {
            if (Status == RequestStatus.Denied)
                throw new InvalidOperationException("Cannot approve a request already denied.");
            Status = RequestStatus.Approved;
        }

        /// <summary>
        /// Denies this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Cannot deny a request already approved.</exception>
        public void Deny()
        {
            if (Status == RequestStatus.Approved)
                throw new InvalidOperationException("Cannot deny a request already approved.");
            Status = RequestStatus.Denied;
        }
    }
}
