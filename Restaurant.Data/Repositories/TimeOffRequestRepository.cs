using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;
using Microsoft.EntityFrameworkCore;

namespace Restaurant.Data.Repositories
{
    public class TimeOffRequestRepository : ITimeOffRequestRepository
    {
        private readonly AppDbContext _context;
        public TimeOffRequestRepository(AppDbContext context)
        {
            _context = context;
        }
        public TimeOffRequest GetById(Guid id)
        {
            return _context.TimeOffRequests.Find(id);
        }
        public void Add(TimeOffRequest request)
        {
            _context.TimeOffRequests.Add(request);
            _context.SaveChanges();
        }
        public void Update(TimeOffRequest request)
        {
            _context.TimeOffRequests.Update(request);
            _context.SaveChanges();
        }
        public IEnumerable<TimeOffRequest> GetByStaffId(Guid staffId)
        {
            return _context.TimeOffRequests.Where(r => r.StaffId == staffId).ToList();
        }
        public IEnumerable<TimeOffRequest> GetPendingRequests()
        {
            return _context.TimeOffRequests
                           .AsNoTracking()
                           .Where(r => r.Status == TimeOffRequest.RequestStatus.Pending)
                           .ToList();
        }

    }
}
