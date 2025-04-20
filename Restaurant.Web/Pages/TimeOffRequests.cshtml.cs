using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class TimeOffRequestsModel : PageModel
    {
        private readonly AppDbContext _context;

        public TimeOffRequestsModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty] public Guid RequestId { get; set; }
        [BindProperty] public string Action { get; set; }

        public List<TimeOffRequest> Requests { get; set; } = new();
        public List<Staff> StaffList { get; set; } = new();
        public string Message { get; set; }

        public void OnGet()
        {
            Requests = _context.TimeOffRequests
                .OrderByDescending(r => r.RequestedAt)
                .ToList();

            StaffList = _context.Staff.ToList();
        }

        public IActionResult OnPost()
        {
            var request = _context.TimeOffRequests.FirstOrDefault(r => r.Id == RequestId);
            if (request == null)
            {
                Message = "Request not found.";
                OnGet();
                return Page();
            }

            if (request.Status != TimeOffRequest.RequestStatus.Pending)
            {
                Message = "This request has already been handled.";
                OnGet();
                return Page();
            }

            if (Action == "Approve")
            {
                request.Approve();
            }
            else if (Action == "Deny")
            {
                request.Deny();
            }

            _context.SaveChanges();
            Message = $"Request marked as {request.Status}.";
            OnGet();
            return Page();
        }
    }
}
