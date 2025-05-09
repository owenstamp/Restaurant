using System;

namespace Restaurant.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class StaffSchedule
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }       // ScheduleID
        /// <summary>
        /// Gets the staff identifier.
        /// </summary>
        /// <value>
        /// The staff identifier.
        /// </value>
        public Guid StaffId { get; private set; }  // Link to Staff
        /// <summary>
        /// Gets the shift date.
        /// </summary>
        /// <value>
        /// The shift date.
        /// </value>
        public DateTime ShiftDate { get; private set; }
        /// <summary>
        /// Gets the shift start time.
        /// </summary>
        /// <value>
        /// The shift start time.
        /// </value>
        public TimeSpan ShiftStartTime { get; private set; }
        /// <summary>
        /// Gets the shift end time.
        /// </summary>
        /// <value>
        /// The shift end time.
        /// </value>
        public TimeSpan ShiftEndTime { get; private set; }
        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public ShiftStatus Status { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public enum ShiftStatus
        {
            /// <summary>
            /// The active
            /// </summary>
            Active,
            /// <summary>
            /// The time off
            /// </summary>
            TimeOff,
            /// <summary>
            /// The off
            /// </summary>
            Off
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffSchedule"/> class.
        /// </summary>
        /// <param name="staffId">The staff identifier.</param>
        /// <param name="shiftDate">The shift date.</param>
        /// <param name="shiftStartTime">The shift start time.</param>
        /// <param name="shiftEndTime">The shift end time.</param>
        /// <param name="status">The status.</param>
        public StaffSchedule(Guid staffId, DateTime shiftDate, TimeSpan shiftStartTime, TimeSpan shiftEndTime, ShiftStatus status)
        {
            Id = Guid.NewGuid();
            StaffId = staffId;
            ShiftDate = shiftDate;
            ShiftStartTime = shiftStartTime;
            ShiftEndTime = shiftEndTime;
            Status = status;
        }

        /// <summary>
        /// Reassigns to.
        /// </summary>
        /// <param name="newStaffId">The new staff identifier.</param>
        public void ReassignTo(Guid newStaffId)
        {
            StaffId = newStaffId;
        }
    }
}
