using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class ShiftScheduleModel : PageModel
    {
        /// <summary>
        /// The schedule repo
        /// </summary>
        private readonly IStaffScheduleRepository _scheduleRepo;
        /// <summary>
        /// The staff repo
        /// </summary>
        private readonly IStaffRepository _staffRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShiftScheduleModel"/> class.
        /// </summary>
        /// <param name="scheduleRepo">The schedule repo.</param>
        /// <param name="staffRepo">The staff repo.</param>
        public ShiftScheduleModel(
            IStaffScheduleRepository scheduleRepo,
            IStaffRepository staffRepo)
        {
            _scheduleRepo = scheduleRepo;
            _staffRepo = staffRepo;
        }

        /// <summary>
        /// Gets or sets the staff list.
        /// </summary>
        /// <value>
        /// The staff list.
        /// </value>
        public List<Staff> StaffList { get; set; } = new();
        /// <summary>
        /// Gets or sets the staff identifier.
        /// </summary>
        /// <value>
        /// The staff identifier.
        /// </value>
        [BindProperty] public Guid StaffId { get; set; }
        /// <summary>
        /// Gets or sets the shift date.
        /// </summary>
        /// <value>
        /// The shift date.
        /// </value>
        [BindProperty] public DateTime ShiftDate { get; set; }
        /// <summary>
        /// Gets or sets the shift start time.
        /// </summary>
        /// <value>
        /// The shift start time.
        /// </value>
        [BindProperty] public TimeSpan ShiftStartTime { get; set; }
        /// <summary>
        /// Gets or sets the shift end time.
        /// </summary>
        /// <value>
        /// The shift end time.
        /// </value>
        [BindProperty] public TimeSpan ShiftEndTime { get; set; }
        /// <summary>
        /// Gets or sets the shifts.
        /// </summary>
        /// <value>
        /// The shifts.
        /// </value>
        public List<StaffSchedule> Shifts { get; set; } = new();
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
            // Load staff dropdown
            StaffList = _staffRepo.GetAll().ToList();

            // If a specific staff is selected, filter; otherwise show all
            if (StaffId != Guid.Empty)
            {
                Shifts = _scheduleRepo.GetByStaffId(StaffId).ToList();
            }
            else
            {
                Shifts = _scheduleRepo.GetAll().ToList();
            }
        }

        /// <summary>
        /// Called when [post].
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost()
        {
            // Basic validation
            if (ShiftEndTime <= ShiftStartTime)
            {
                Message = "End time must be after start time.";
                OnGet();
                return Page();
            }

            // Create via the domain constructor (Id set inside)
            var newShift = new StaffSchedule(
                StaffId,
                ShiftDate,
                ShiftStartTime,
                ShiftEndTime,
                StaffSchedule.ShiftStatus.Active
            );

            _scheduleRepo.Add(newShift);
            Message = "Shift scheduled successfully.";

            // Redirect so the new shift appears in the list
            return RedirectToPage(new { StaffId });
        }
    }
}
