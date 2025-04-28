using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant.Web.Pages
{
    public class ShiftScheduleModel : PageModel
    {
        private readonly IStaffScheduleRepository _scheduleRepo;
        private readonly IStaffRepository _staffRepo;

        public ShiftScheduleModel(
            IStaffScheduleRepository scheduleRepo,
            IStaffRepository staffRepo)
        {
            _scheduleRepo = scheduleRepo;
            _staffRepo = staffRepo;
        }

        public List<Staff> StaffList { get; set; } = new();
        [BindProperty] public Guid StaffId { get; set; }
        [BindProperty] public DateTime ShiftDate { get; set; }
        [BindProperty] public TimeSpan ShiftStartTime { get; set; }
        [BindProperty] public TimeSpan ShiftEndTime { get; set; }
        public List<StaffSchedule> Shifts { get; set; } = new();
        public string Message { get; set; }

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
