using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{

    public interface IStaffRepository
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Staff GetById(Guid id);
        /// <summary>
        /// Adds the specified staff.
        /// </summary>
        /// <param name="staff">The staff.</param>
        void Add(Staff staff);
        /// <summary>
        /// Updates the specified staff.
        /// </summary>
        /// <param name="staff">The staff.</param>
        void Update(Staff staff);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Staff> GetAll();
    }

    public interface IStaffScheduleRepository
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        StaffSchedule GetById(Guid id);
        /// <summary>
        /// Adds the specified schedule.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        void Add(StaffSchedule schedule);
        /// <summary>
        /// Updates the specified schedule.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        void Update(StaffSchedule schedule);
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IEnumerable<StaffSchedule> GetAll();
        /// <summary>
        /// Gets the by staff identifier.
        /// </summary>
        /// <param name="staffId">The staff identifier.</param>
        /// <returns></returns>
        IEnumerable<StaffSchedule> GetByStaffId(Guid staffId);
    }
}
