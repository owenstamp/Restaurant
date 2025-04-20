using System;
using System.Collections.Generic;
using System.Linq;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using Restaurant.Data;

namespace Restaurant.Data.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly AppDbContext _context;
        public FeedbackRepository(AppDbContext context)
        {
            _context = context;
        }
        public Feedback GetById(Guid id)
        {
            return _context.Feedback.Find(id);
        }
        public void Add(Feedback feedback)
        {
            _context.Feedback.Add(feedback);
            _context.SaveChanges();
        }
    }
}
