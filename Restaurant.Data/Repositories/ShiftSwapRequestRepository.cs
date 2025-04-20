using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;

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
            return _context.ShiftSwapRequests.Find(id);
        }
        public void Add(ShiftSwapRequest swapRequest)
        {
            _context.ShiftSwapRequests.Add(swapRequest);
            _context.SaveChanges();
        }
        public void Update(ShiftSwapRequest swapRequest)
        {
            _context.ShiftSwapRequests.Update(swapRequest);
            _context.SaveChanges();
        }
        public IEnumerable<ShiftSwapRequest> GetPendingSwaps()
        {
            return _context.ShiftSwapRequests.Where(s => s.Status == ShiftSwapRequest.SwapStatus.Pending).ToList();
        }
    }
}
