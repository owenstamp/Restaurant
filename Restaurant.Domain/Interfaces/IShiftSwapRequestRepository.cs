using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Restaurant.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IShiftSwapRequestRepository
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        ShiftSwapRequest GetById(Guid id);
        /// <summary>
        /// Adds the specified swap request.
        /// </summary>
        /// <param name="swapRequest">The swap request.</param>
        void Add(ShiftSwapRequest swapRequest);
        /// <summary>
        /// Updates the specified swap request.
        /// </summary>
        /// <param name="swapRequest">The swap request.</param>
        void Update(ShiftSwapRequest swapRequest);
        /// <summary>
        /// Gets the pending for staff.
        /// </summary>
        /// <param name="staffId">The staff identifier.</param>
        /// <returns></returns>
        IEnumerable<ShiftSwapRequest> GetPendingForStaff(Guid staffId);
    }
}
