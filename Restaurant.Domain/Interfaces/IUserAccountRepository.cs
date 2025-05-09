using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    public interface IUserAccountRepository
    {
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        UserAccount GetById(Guid id);
        /// <summary>
        /// Gets the by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        UserAccount GetByUsername(string username);
        /// <summary>
        /// Gets the by staff identifier.
        /// </summary>
        /// <param name="staffId">The staff identifier.</param>
        /// <returns></returns>
        UserAccount GetByStaffId(Guid staffId);
        /// <summary>
        /// Adds the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        void Add(UserAccount user);
        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        void Update(UserAccount user);
    }
}
