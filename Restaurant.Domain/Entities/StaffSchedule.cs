using System;

namespace Restaurant.Domain.Entities
{
    public class StaffSchedule
    {
        public Guid Id { get; private set; }       // ScheduleID
        public Guid StaffId { get; private set; }  // Link to Staff
        public DateTime ShiftDate { get; private set; }
        public TimeSpan ShiftStartTime { get; private set; }
        public TimeSpan ShiftEndTime { get; private set; }
        public ShiftStatus Status { get; private set; }

        public enum ShiftStatus
        {
            Active,
            TimeOff,
            Off
        }

        public StaffSchedule(Guid staffId, DateTime shiftDate, TimeSpan shiftStartTime, TimeSpan shiftEndTime, ShiftStatus status)
        {
            Id = Guid.NewGuid();
            StaffId = staffId;
            ShiftDate = shiftDate;
            ShiftStartTime = shiftStartTime;
            ShiftEndTime = shiftEndTime;
            Status = status;
        }

        public void ReassignTo(Guid newStaffId)
        {
            StaffId = newStaffId;
        }
    }
}
