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
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class TimeOffRequestsModel : PageModel
    {
        /// <summary>
        /// The time off repo
        /// </summary>
        private readonly ITimeOffRequestRepository _timeOffRepo;
        /// <summary>
        /// The staff repo
        /// </summary>
        private readonly IStaffRepository _staffRepo;
        /// <summary>
        /// The staff schedule repo
        /// </summary>
        private readonly IStaffScheduleRepository _StaffScheduleRepo;
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOffRequestsModel"/> class.
        /// </summary>
        /// <param name="timeOffRepo">The time off repo.</param>
        /// <param name="staffRepo">The staff repo.</param>
        /// <param name="StaffScheduleRepo">The staff schedule repo.</param>
        public TimeOffRequestsModel(
            ITimeOffRequestRepository timeOffRepo,
            IStaffRepository staffRepo,
            IStaffScheduleRepository StaffScheduleRepo)
        {
            _timeOffRepo = timeOffRepo;
            _staffRepo = staffRepo;
            _StaffScheduleRepo = StaffScheduleRepo;
        }

        /// <summary>
        /// Gets or sets the requests.
        /// </summary>
        /// <value>
        /// The requests.
        /// </value>
        public List<TimeOffRequest> Requests { get; set; } = new();
        /// <summary>
        /// Gets or sets the staff list.
        /// </summary>
        /// <value>
        /// The staff list.
        /// </value>
        public List<Staff> StaffList { get; set; } = new();
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Called when [get].
        /// </summary>
        public void OnGet()
        {
            // Load all pending requests and staff for the dropdown
            Requests = _timeOffRepo.GetPendingRequests().ToList();
            StaffList = _staffRepo.GetAll().ToList();
        }
        /// <summary>
        /// Called when [post approve].
        /// </summary>
        /// <param name="requestId">The request identifier.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Called when [post reject].
        /// </summary>
        /// <param name="requestId">The request identifier.</param>
        /// <returns></returns>
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
