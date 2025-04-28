using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Restaurant.Domain.Interfaces
{
    public interface IShiftSwapRequestRepository
    {
        ShiftSwapRequest GetById(Guid id);
        void Add(ShiftSwapRequest swapRequest);
        void Update(ShiftSwapRequest swapRequest);
        IEnumerable<ShiftSwapRequest> GetPendingForStaff(Guid staffId);
    }
}
