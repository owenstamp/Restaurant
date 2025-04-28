using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    public interface IUserAccountRepository
    {
        UserAccount GetById(Guid id);
        UserAccount GetByUsername(string username);
        UserAccount GetByStaffId(Guid staffId);
        void Add(UserAccount user);
        void Update(UserAccount user);
    }
}
