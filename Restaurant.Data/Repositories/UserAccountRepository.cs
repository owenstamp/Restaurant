using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;

namespace Restaurant.Data.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly AppDbContext _context;
        public UserAccountRepository(AppDbContext context)
        {
            _context = context;
        }
        public UserAccount GetById(Guid id)
        {
            return _context.UserAccounts.Find(id);
        }
        public UserAccount GetByUsername(string username)
        {
            return _context.UserAccounts.FirstOrDefault(u => u.Username == username);
        }


        public UserAccount GetByStaffId(Guid staffId) =>     // ← NEW IMPLEMENTATION
            _context.UserAccounts.FirstOrDefault(u => u.StaffId == staffId);

        public void Add(UserAccount user)
        {
            _context.UserAccounts.Add(user);
            _context.SaveChanges();
        }
        public void Update(UserAccount user)
        {
            _context.UserAccounts.Update(user);
            _context.SaveChanges();
        }
    }
}
