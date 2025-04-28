using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class TimeOffRequestsModel : PageModel
    {
        private readonly ITimeOffRequestRepository _timeOffRepo;
        private readonly IStaffRepository _staffRepo;

        public TimeOffRequestsModel(
            ITimeOffRequestRepository timeOffRepo,
            IStaffRepository staffRepo)
        {
            _timeOffRepo = timeOffRepo;
            _staffRepo = staffRepo;
        }

        public List<TimeOffRequest> Requests { get; set; } = new();
        public List<Staff> StaffList { get; set; } = new();
        public string Message { get; set; }

        public void OnGet()
        {
            // Load all pending requests and staff for the dropdown
            Requests = _timeOffRepo.GetPendingRequests().ToList();
            StaffList = _staffRepo.GetAll().ToList();
        }

        public IActionResult OnPostApprove(Guid requestId)
        {
            var req = _timeOffRepo.GetById(requestId);
            if (req is not null)
            {
                req.Approve();
                _timeOffRepo.Update(req);
                Message = "Time-off request approved.";
            }
            else
            {
                Message = "Request not found.";
            }

            OnGet();
            return Page();
        }

        public IActionResult OnPostReject(Guid requestId)
        {
            var req = _timeOffRepo.GetById(requestId);
            if (req is not null)
            {
                req.Deny();
                _timeOffRepo.Update(req);
                Message = "Time-off request denied.";
            }
            else
            {
                Message = "Request not found.";
            }

            OnGet();
            return Page();
        }
    }
}
