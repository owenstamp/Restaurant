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
    /// <seealso cref="Restaurant.Domain.Interfaces.IStaffRepository" />
    public class StaffRepository : IStaffRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly AppDbContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="StaffRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public StaffRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Staff GetById(Guid id)
        {
            return _context.Staff.Find(id);
        }
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Staff> GetAll()
        {
            return _context.Staff.ToList();
        }
        /// <summary>
        /// Adds the specified staff.
        /// </summary>
        /// <param name="staff">The staff.</param>
        public void Add(Staff staff)
        {
            _context.Staff.Add(staff);
            _context.SaveChanges();
        }
        /// <summary>
        /// Updates the specified staff.
        /// </summary>
        /// <param name="staff">The staff.</param>
        public void Update(Staff staff)
        {
            _context.Staff.Update(staff);
            _context.SaveChanges();
        }
    }
}
