using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Restaurant.Web.Pages
{
    public class StaffDashboardModel : PageModel
    {
        private readonly AppDbContext _context;

        public StaffDashboardModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty] public Guid StaffId { get; set; }
        [BindProperty] public DateTime ShiftDate { get; set; }
        [BindProperty] public TimeSpan ShiftStartTime { get; set; }
        [BindProperty] public TimeSpan ShiftEndTime { get; set; }
        [BindProperty] public string CalendarAction { get; set; } = string.Empty;

        [BindProperty] public DateTime StartDate { get; set; }
        [BindProperty] public DateTime EndDate { get; set; }
        [BindProperty] public string Reason { get; set; }

        [BindProperty] public Guid SwapRequestId { get; set; }
        [BindProperty] public string SwapAction { get; set; }

        public List<StaffSchedule> MyShifts { get; set; } = new();
        public List<StaffSchedule> AllShifts { get; set; } = new();
        public List<Staff> StaffList { get; set; } = new();
        public List<ShiftSwapRequest> IncomingSwapRequests { get; set; } = new();

        public string CalendarEventsJson { get; set; } = "[]";
        public string Message { get; set; }

        public void OnGet()
        {
            LoadData();
        }

        public IActionResult OnPost()
        {
            var staffIdString = HttpContext.Session.GetString("StaffId");
            var role = HttpContext.Session.GetString("UserRole");

            if (!Guid.TryParse(staffIdString, out Guid staffId))
            {
                Message = "You must be logged in.";
                LoadData();
                return Page();
            }

            // Handle swap approval
            if (SwapRequestId != Guid.Empty && !string.IsNullOrEmpty(SwapAction))
            {
                var swap = _context.ShiftSwapRequests.FirstOrDefault(s =>
                    s.Id == SwapRequestId &&
                    s.TargetStaffId == staffId &&
                    s.Status == ShiftSwapRequest.SwapStatus.Pending);

                if (swap != null)
                {
                    if (SwapAction == "Accept")
                    {
                        swap.Approve();
                        Message = "You accepted the swap.";
                    }
                    else if (SwapAction == "Decline")
                    {
                        swap.Reject();
                        Message = "You declined the swap.";
                    }

                    _context.SaveChanges();
                    return RedirectToPage();
                }

                Message = "This swap is no longer available.";
                LoadData();
                return Page();
            }

            // Handle shift creation (manager)
            if (CalendarAction == "AddShift" && role == "Manager")
            {
                if (ShiftEndTime <= ShiftStartTime)
                {
                    Message = "End time must be after start time.";
                }
                else
                {
                    var shift = new StaffSchedule(
                        StaffId,
                        ShiftDate,
                        ShiftStartTime,
                        ShiftEndTime,
                        StaffSchedule.ShiftStatus.Active
                    );
                    _context.StaffSchedules.Add(shift);
                    _context.SaveChanges();
                    Message = "Shift added.";
                }

                LoadData();
                return RedirectToPage();
            }
            if (string.IsNullOrEmpty(CalendarAction) || CalendarAction == "RequestTimeOff")
            {
                if (EndDate < StartDate || string.IsNullOrWhiteSpace(Reason))
                {
                    Message = "All fields required for time-off.";
                    LoadData();
                    return Page();
                }

                var request = new TimeOffRequest(staffId, StartDate, EndDate, Reason);
                _context.TimeOffRequests.Add(request);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Time-off request submitted.";
                return RedirectToPage();
            }
            return RedirectToPage(); // default if no action matched


        }

        private void LoadData()
        {
            StaffList = _context.Staff.OrderBy(s => s.FirstName).ToList();

            AllShifts = _context.StaffSchedules
                .OrderBy(s => s.ShiftDate)
                .ThenBy(s => s.ShiftStartTime)
                .ToList();

            var staffIdString = HttpContext.Session.GetString("StaffId");
            if (Guid.TryParse(staffIdString, out Guid staffId))
            {
                MyShifts = AllShifts
                    .Where(s => s.StaffId == staffId)
                    .ToList();

                IncomingSwapRequests = _context.ShiftSwapRequests
                    .Where(s => s.TargetStaffId == staffId && s.Status == ShiftSwapRequest.SwapStatus.Pending)
                    .ToList();
            }

            CalendarEventsJson = JsonSerializer.Serialize(AllShifts.Select(shift =>
            {
                var staff = StaffList.FirstOrDefault(s => s.Id == shift.StaffId);
                return new
                {
                    title = $"{staff?.FirstName} {staff?.LastName} ({staff?.Role})",
                    start = $"{shift.ShiftDate:yyyy-MM-dd}T{shift.ShiftStartTime}",
                    end = $"{shift.ShiftDate:yyyy-MM-dd}T{shift.ShiftEndTime}",
                    allDay = false
                };
            }));
        }
    }
}
