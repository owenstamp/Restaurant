using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;

namespace Restaurant.Data.Repositories
{
    public class POSLogRepository : IPOSLogRepository
    {
        private readonly AppDbContext _context;
        public POSLogRepository(AppDbContext context)
        {
            _context = context;
        }
        public POSLog GetById(Guid id)
        {
            return _context.POSLogs.Find(id);
        }
        public void Add(POSLog log)
        {
            _context.POSLogs.Add(log);
            _context.SaveChanges();
        }
        public IEnumerable<POSLog> GetByOrderId(Guid orderId)
        {
            return _context.POSLogs.Where(l => l.OrderId == orderId).ToList();
        }
    }
}
