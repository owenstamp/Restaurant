using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ITimeOffRequestRepository" />
    public class TimeOffRequestRepository : ITimeOffRequestRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly AppDbContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOffRequestRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TimeOffRequestRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public TimeOffRequest GetById(Guid id)
        {
            return _context.TimeOffRequests.Find(id);
        }
        /// <summary>
        /// Adds the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Add(TimeOffRequest request)
        {
            _context.TimeOffRequests.Add(request);
            _context.SaveChanges();
        }
        /// <summary>
        /// Updates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public void Update(TimeOffRequest request)
        {
            _context.TimeOffRequests.Update(request);
            _context.SaveChanges();
        }
        /// <summary>
        /// Gets the by staff identifier.
        /// </summary>
        /// <param name="staffId">The staff identifier.</param>
        /// <returns></returns>
        public IEnumerable<TimeOffRequest> GetByStaffId(Guid staffId)
        {
            return _context.TimeOffRequests.Where(r => r.StaffId == staffId).ToList();
        }
        /// <summary>
        /// Gets the pending requests.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TimeOffRequest> GetPendingRequests()
        {
            return _context.TimeOffRequests
                           .AsNoTracking()
                           .Where(r => r.Status == TimeOffRequest.RequestStatus.Pending)
                           .ToList();
        }

    }
}
