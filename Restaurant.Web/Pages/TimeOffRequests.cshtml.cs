using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Restaurant.Web.Pages
{
    public class TimeOffRequestsModel : PageModel
    {
        private readonly ITimeOffRequestRepository _timeOffRepo;
        private readonly IStaffRepository _staffRepo;
        private readonly IStaffScheduleRepository _StaffScheduleRepo;
        public TimeOffRequestsModel(
            ITimeOffRequestRepository timeOffRepo,
            IStaffRepository staffRepo,
            IStaffScheduleRepository StaffScheduleRepo)
        {
            _timeOffRepo = timeOffRepo;
            _staffRepo = staffRepo;
            _StaffScheduleRepo = StaffScheduleRepo;
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
                _timeOffRepo.Update(req);  // This calls SaveChanges and should persist the new status

                // Create a time off shift (for a single day; add a loop for multi?day)
                var timeOffShift = new StaffSchedule(
                    req.StaffId,
                    req.StartDate.Date,
                    new TimeSpan(0, 0, 0),
                    new TimeSpan(23, 59, 59),
                    StaffSchedule.ShiftStatus.TimeOff
                );
                _StaffScheduleRepo.Add(timeOffShift);

                TempData["SuccessMessage"] = "Time-off request approved.";
            }
            else
            {
                TempData["ErrorMessage"] = "Request not found.";
            }

            // Return a redirect so that a fresh instance of the page is loaded.
            return RedirectToPage();
        }

        public IActionResult OnPostReject(Guid requestId)
        {
            var req = _timeOffRepo.GetById(requestId);
            if (req is not null)
            {
                req.Deny();
                _timeOffRepo.Update(req);
                TempData["SuccessMessage"] = "Time-off request denied.";
            }
            else
            {
                TempData["ErrorMessage"] = "Request not found.";
            }

            return RedirectToPage();
        }





    }
}
