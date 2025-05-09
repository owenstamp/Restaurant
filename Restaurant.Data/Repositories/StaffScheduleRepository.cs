using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;

namespace Restaurant.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Restaurant.Domain.Interfaces.IStaffScheduleRepository" />
    public class StaffScheduleRepository : IStaffScheduleRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly AppDbContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="StaffScheduleRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public StaffScheduleRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StaffSchedule> GetAll() =>
    _context.StaffSchedules.ToList();
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public StaffSchedule GetById(Guid id)
        {
            return _context.StaffSchedules.Find(id);
        }
        /// <summary>
        /// Adds the specified schedule.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        public void Add(StaffSchedule schedule)
        {
            _context.StaffSchedules.Add(schedule);
            _context.SaveChanges();
        }
        /// <summary>
        /// Updates the specified schedule.
        /// </summary>
        /// <param name="schedule">The schedule.</param>
        public void Update(StaffSchedule schedule)
        {
            _context.StaffSchedules.Update(schedule);
            _context.SaveChanges();
        }
        /// <summary>
        /// Gets the by staff identifier.
        /// </summary>
        /// <param name="staffId">The staff identifier.</param>
        /// <returns></returns>
        public IEnumerable<StaffSchedule> GetByStaffId(Guid staffId)
        {
            return _context.StaffSchedules.Where(s => s.StaffId == staffId).ToList();
        }
    }
}
