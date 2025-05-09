using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;

namespace Restaurant.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Restaurant.Domain.Interfaces.IShiftSwapRequestRepository" />
    public class ShiftSwapRequestRepository : IShiftSwapRequestRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShiftSwapRequestRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ShiftSwapRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public ShiftSwapRequest GetById(Guid id)
        {
            return _context.ShiftSwapRequests.FirstOrDefault(s => s.Id == id);
        }

        /// <summary>
        /// Adds the specified swap request.
        /// </summary>
        /// <param name="swapRequest">The swap request.</param>
        public void Add(ShiftSwapRequest swapRequest)
        {
            _context.ShiftSwapRequests.Add(swapRequest);
            _context.SaveChanges();
        }

        /// <summary>
        /// Updates the specified swap request.
        /// </summary>
        /// <param name="swapRequest">The swap request.</param>
        /// <exception cref="System.InvalidOperationException">Swap update failed due to concurrency issue.</exception>
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

        /// <summary>
        /// Gets the pending for staff.
        /// </summary>
        /// <param name="staffId">The staff identifier.</param>
        /// <returns></returns>
        public IEnumerable<ShiftSwapRequest> GetPendingForStaff(Guid staffId)
        {
            return _context.ShiftSwapRequests
                .Where(s => s.TargetStaffId == staffId && s.Status == ShiftSwapRequest.SwapStatus.PendingRecipient)
                .ToList();
        }
    }
}
