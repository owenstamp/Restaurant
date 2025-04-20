using Restaurant.Domain.Entities;

public interface IShiftSwapRequestRepository
{
    ShiftSwapRequest GetById(Guid id);
    void Add(ShiftSwapRequest swapRequest);
    void Update(ShiftSwapRequest swapRequest);
    IEnumerable<ShiftSwapRequest> GetPendingSwaps();
}
