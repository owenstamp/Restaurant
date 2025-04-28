using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Restaurant.Web.Pages
{
    public class StaffDashboardModel : PageModel
    {
        private readonly IStaffScheduleRepository _scheduleRepo;
        private readonly IStaffRepository _staffRepo;
        private readonly IShiftSwapRequestRepository _swapRepo;
        private readonly ITimeOffRequestRepository _timeOffRepo;

        public StaffDashboardModel(
            IStaffScheduleRepository scheduleRepo,
            IStaffRepository staffRepo,
            IShiftSwapRequestRepository swapRepo,
            ITimeOffRequestRepository timeOffRepo)
        {
            _scheduleRepo = scheduleRepo;
            _staffRepo = staffRepo;
            _swapRepo = swapRepo;
            _timeOffRepo = timeOffRepo;
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
        public List<ShiftSwapRequest> ManagerSwapApprovals { get; set; } = new();
        public string CalendarEventsJson { get; set; } = "[]";
        public string Message { get; set; }

        public void OnGet()
        {
            LoadData();
        }

        public IActionResult OnPost()
        {
            // 1) Identify current staff
            var staffIdStr = HttpContext.Session.GetString("StaffId");
            var role = HttpContext.Session.GetString("UserRole");
            if (!Guid.TryParse(staffIdStr, out var staffId))
            {
                Message = "You must be logged in.";
                LoadData();
                return Page();
            }

            // 2) Request a shift swap
            if (CalendarAction == "RequestSwap")
            {
                var origIdStr = Request.Form["OriginalShiftId"];
                var targetIdStr = Request.Form["TargetStaffId"];
                if (!Guid.TryParse(origIdStr, out var origShiftId)
                    || !Guid.TryParse(targetIdStr, out var targetStaffId))
                {
                    Message = "Invalid swap parameters.";
                    LoadData();
                    return Page();
                }

                var origShift = _scheduleRepo.GetById(origShiftId);
                var targetShift = _scheduleRepo
                    .GetByStaffId(targetStaffId)
                    .FirstOrDefault(s =>
                        s.ShiftDate == DateTime.Parse(Request.Form["TargetDate"]) &&
                        s.ShiftStartTime == TimeSpan.Parse(Request.Form["TargetStartTime"]) &&
                        s.ShiftEndTime == TimeSpan.Parse(Request.Form["TargetEndTime"])
                    );

                if (origShift == null || targetShift == null || origShift.StaffId != staffId)
                {
                    Message = "Invalid or missing shifts.";
                    LoadData();
                    return Page();
                }
                if ((origShift.ShiftEndTime - origShift.ShiftStartTime) !=
                    (targetShift.ShiftEndTime - targetShift.ShiftStartTime))
                {
                    Message = "Shifts must be same length.";
                    LoadData();
                    return Page();
                }

                var targetStaffRole = _staffRepo.GetById(targetStaffId)?.Role ?? "";
                var swap = new ShiftSwapRequest(staffId, targetStaffId, origShiftId);

                if (targetStaffRole == "Manager")
                    HandleFinalApproval(swap);
                else
                    swap.SetPendingRecipient();

                _swapRepo.Add(swap);
                Message = "Swap request submitted.";
                return RedirectToPage();
            }

            // 3) Accept/Decline by recipient or manager
            if (SwapRequestId != Guid.Empty && !string.IsNullOrEmpty(SwapAction))
            {
                var swap = _swapRepo.GetById(SwapRequestId);
                if (swap != null)
                {
                    if (SwapAction == "Accept" &&
                        swap.TargetStaffId == staffId &&
                        swap.Status == ShiftSwapRequest.SwapStatus.PendingRecipient)
                    {
                        swap.ApproveByRecipient();
                        _swapRepo.Update(swap);
                        Message = "You approved the swap. Awaiting manager review.";
                    }
                    else if (SwapAction == "Decline" && swap.TargetStaffId == staffId)
                    {
                        swap.Reject();
                        _swapRepo.Update(swap);
                        Message = "Swap declined.";
                    }
                    else if (role == "Manager" &&
                             swap.Status == ShiftSwapRequest.SwapStatus.ApprovedByRecipient)
                    {
                        if (SwapAction == "ManagerApprove")
                        {
                            HandleFinalApproval(swap);
                            Message = "Swap approved and executed.";
                        }
                        else if (SwapAction == "ManagerReject")
                        {
                            swap.Reject();
                            _swapRepo.Update(swap);
                            Message = "Swap rejected.";
                        }
                    }
                }

                LoadData();
                return Page();
            }

            // 4) Manager adds a shift
            if (CalendarAction == "AddShift" && role == "Manager")
            {
                if (ShiftEndTime <= ShiftStartTime)
                {
                    Message = "End time must be after start.";
                    LoadData();
                    return Page();
                }

                var newShift = new StaffSchedule(
                    StaffId, ShiftDate, ShiftStartTime, ShiftEndTime,
                    StaffSchedule.ShiftStatus.Active
                );
                _scheduleRepo.Add(newShift);
                Message = "Shift added.";
                return RedirectToPage();
            }

            // 5) Request time off
            if (string.IsNullOrEmpty(CalendarAction) || CalendarAction == "RequestTimeOff")
            {
                if (EndDate < StartDate || string.IsNullOrWhiteSpace(Reason))
                {
                    Message = "All fields are required.";
                    LoadData();
                    return Page();
                }

                var req = new TimeOffRequest(staffId, StartDate, EndDate, Reason);
                _timeOffRepo.Add(req);
                Message = "Time-off request submitted.";
                return RedirectToPage();
            }

            return RedirectToPage();
        }

        private void HandleFinalApproval(ShiftSwapRequest swap)
        {
            swap.Approve();
            _swapRepo.Update(swap);

            // swap the actual shifts
            var origShift = _scheduleRepo.GetById(swap.OriginalShiftId);
            if (origShift == null) return;

            var match = _scheduleRepo
                .GetByStaffId(swap.TargetStaffId)
                .FirstOrDefault(s =>
                    s.ShiftDate == origShift.ShiftDate &&
                    s.ShiftStartTime == origShift.ShiftStartTime &&
                    s.ShiftEndTime == origShift.ShiftEndTime &&
                    s.Id != origShift.Id
                );
            if (match == null) return;

            origShift.ReassignTo(match.StaffId);
            match.ReassignTo(swap.RequestingStaffId);

            _scheduleRepo.Update(origShift);
            _scheduleRepo.Update(match);
        }

        private void LoadData()
        {
            StaffList = _staffRepo.GetAll().OrderBy(s => s.FirstName).ToList();
            AllShifts = _scheduleRepo.GetAll()
                                      .OrderBy(s => s.ShiftDate)
                                      .ThenBy(s => s.ShiftStartTime)
                                      .ToList();

            var staffIdStr = HttpContext.Session.GetString("StaffId");
            var role = HttpContext.Session.GetString("UserRole");
            if (!Guid.TryParse(staffIdStr, out var staffId)) return;

            MyShifts = AllShifts.Where(s => s.StaffId == staffId).ToList();
            IncomingSwapRequests = _swapRepo.GetPendingForStaff(staffId).ToList();
            ManagerSwapApprovals = role == "Manager"
                ? _swapRepo.GetPendingForStaff(staffId)
                            .Where(r => r.Status == ShiftSwapRequest.SwapStatus.ApprovedByRecipient)
                            .ToList()
                : new List<ShiftSwapRequest>();

            // Build calendar JSON
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
