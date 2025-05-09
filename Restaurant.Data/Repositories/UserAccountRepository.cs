using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;

namespace Restaurant.Data.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Restaurant.Domain.Interfaces.IUserAccountRepository" />
    public class UserAccountRepository : IUserAccountRepository
    {
        /// <summary>
        /// The context
        /// </summary>
        private readonly AppDbContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public UserAccountRepository(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public UserAccount GetById(Guid id)
        {
            return _context.UserAccounts.Find(id);
        }
        /// <summary>
        /// Gets the by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public UserAccount GetByUsername(string username)
        {
            return _context.UserAccounts.FirstOrDefault(u => u.Username == username);
        }


        /// <summary>
        /// Gets the by staff identifier.
        /// </summary>
        /// <param name="staffId">The staff identifier.</param>
        /// <returns></returns>
        public UserAccount GetByStaffId(Guid staffId) =>     // ← NEW IMPLEMENTATION
            _context.UserAccounts.FirstOrDefault(u => u.StaffId == staffId);

        /// <summary>
        /// Adds the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Add(UserAccount user)
        {
            _context.UserAccounts.Add(user);
            _context.SaveChanges();
        }
        /// <summary>
        /// Updates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Update(UserAccount user)
        {
            _context.UserAccounts.Update(user);
            _context.SaveChanges();
        }
    }
}
