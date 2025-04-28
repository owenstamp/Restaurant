using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;

namespace Restaurant.Data.Repositories
{
    public class ShiftSwapRequestRepository : IShiftSwapRequestRepository
    {
        private readonly AppDbContext _context;

        public ShiftSwapRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public ShiftSwapRequest GetById(Guid id)
        {
            return _context.ShiftSwapRequests.FirstOrDefault(s => s.Id == id);
        }

        public void Add(ShiftSwapRequest swapRequest)
        {
            _context.ShiftSwapRequests.Add(swapRequest);
            _context.SaveChanges();
        }

        public void Update(ShiftSwapRequest swapRequest)
        {
            try
            {
                _context.Attach(swapRequest);
                _context.Entry(swapRequest).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InvalidOperationException("Swap update failed due to concurrency issue.", ex);
            }
        }

        public IEnumerable<ShiftSwapRequest> GetPendingForStaff(Guid staffId)
        {
            return _context.ShiftSwapRequests
                .Where(s => s.TargetStaffId == staffId && s.Status == ShiftSwapRequest.SwapStatus.PendingRecipient)
                .ToList();
        }
    }
}
