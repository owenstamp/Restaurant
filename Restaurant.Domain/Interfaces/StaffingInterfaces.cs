using Restaurant.Domain.Entities;

namespace Restaurant.Domain.Interfaces
{
    public interface IStaffRepository
    {
        Staff GetById(Guid id);
        void Add(Staff staff);
        void Update(Staff staff);
        IEnumerable<Staff> GetAll();
    }

    public interface IStaffScheduleRepository
    {
        StaffSchedule GetById(Guid id);
        void Add(StaffSchedule schedule);
        void Update(StaffSchedule schedule);
        IEnumerable<StaffSchedule> GetByStaffId(Guid staffId);
    }
}
