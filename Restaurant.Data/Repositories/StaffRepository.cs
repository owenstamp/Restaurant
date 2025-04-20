using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;

namespace Restaurant.Data.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        private readonly AppDbContext _context;
        public StaffRepository(AppDbContext context)
        {
            _context = context;
        }
        public Staff GetById(Guid id)
        {
            return _context.Staff.Find(id);
        }
        public IEnumerable<Staff> GetAll()
        {
            return _context.Staff.ToList();
        }
        public void Add(Staff staff)
        {
            _context.Staff.Add(staff);
            _context.SaveChanges();
        }
        public void Update(Staff staff)
        {
            _context.Staff.Update(staff);
            _context.SaveChanges();
        }
    }
}
