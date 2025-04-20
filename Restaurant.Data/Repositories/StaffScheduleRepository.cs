using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;

namespace Restaurant.Data.Repositories
{
    public class StaffScheduleRepository : IStaffScheduleRepository
    {
        private readonly AppDbContext _context;
        public StaffScheduleRepository(AppDbContext context)
        {
            _context = context;
        }
        public StaffSchedule GetById(Guid id)
        {
            return _context.StaffSchedules.Find(id);
        }
        public void Add(StaffSchedule schedule)
        {
            _context.StaffSchedules.Add(schedule);
            _context.SaveChanges();
        }
        public void Update(StaffSchedule schedule)
        {
            _context.StaffSchedules.Update(schedule);
            _context.SaveChanges();
        }
        public IEnumerable<StaffSchedule> GetByStaffId(Guid staffId)
        {
            return _context.StaffSchedules.Where(s => s.StaffId == staffId).ToList();
        }
    }
}
