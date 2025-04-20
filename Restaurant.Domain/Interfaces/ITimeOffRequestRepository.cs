using Restaurant.Domain.Entities;

public interface ITimeOffRequestRepository
{
    TimeOffRequest GetById(Guid id);
    void Add(TimeOffRequest request);
    void Update(TimeOffRequest request);
    IEnumerable<TimeOffRequest> GetByStaffId(Guid staffId);
    IEnumerable<TimeOffRequest> GetPendingRequests();
}
