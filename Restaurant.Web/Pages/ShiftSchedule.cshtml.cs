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
    public class ShiftScheduleModel : PageModel
    {
        private readonly AppDbContext _context;

        public ShiftScheduleModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty] public Guid StaffId { get; set; }
        [BindProperty] public DateTime ShiftDate { get; set; }
        [BindProperty] public TimeSpan ShiftStartTime { get; set; }
        [BindProperty] public TimeSpan ShiftEndTime { get; set; }

        public List<Staff> StaffList { get; set; } = new();
        public List<StaffSchedule> Shifts { get; set; } = new();
        public string Message { get; set; }

        public void OnGet()
        {
            StaffList = _context.Staff.OrderBy(s => s.FirstName).ToList();
            Shifts = _context.StaffSchedules
                .OrderBy(s => s.ShiftDate)
                .ThenBy(s => s.ShiftStartTime)
                .ToList();
        }

        public IActionResult OnPost()
        {
            if (ShiftEndTime <= ShiftStartTime)
            {
                Message = "End time must be after start time.";
                OnGet();
                return Page();
            }

            var shift = new StaffSchedule(
    StaffId,
    ShiftDate,
    ShiftStartTime,
    ShiftEndTime,
    StaffSchedule.ShiftStatus.Active // default for new shift
);
            _context.StaffSchedules.Add(shift);
            _context.SaveChanges();

            Message = "Shift assigned successfully.";
            OnGet();
            return Page();
        }
    }
}
