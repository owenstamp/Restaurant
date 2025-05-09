using Restaurant.Domain.Entities;

public interface ITimeOffRequestRepository
{
    /// <summary>
    /// Gets the by identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    TimeOffRequest GetById(Guid id);
    /// <summary>
    /// Adds the specified request.
    /// </summary>
    /// <param name="request">The request.</param>
    void Add(TimeOffRequest request);
    /// <summary>
    /// Updates the specified request.
    /// </summary>
    /// <param name="request">The request.</param>
    void Update(TimeOffRequest request);
    /// <summary>
    /// Gets the by staff identifier.
    /// </summary>
    /// <param name="staffId">The staff identifier.</param>
    /// <returns></returns>
    IEnumerable<TimeOffRequest> GetByStaffId(Guid staffId);
    /// <summary>
    /// Gets the pending requests.
    /// </summary>
    /// <returns></returns>
    IEnumerable<TimeOffRequest> GetPendingRequests();
}
