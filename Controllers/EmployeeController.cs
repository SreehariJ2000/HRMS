using System.Security.Claims;
using HRMS.Services.Interfaces;
using HRMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly ILeaveService _leaveService;

        public EmployeeController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = GetCurrentUserId();
            var model = await _leaveService.GetEmployeeDashboardAsync(userId);
            return View(model);
        }

        [HttpGet]
        public IActionResult ApplyLeave()
        {
            return View(new LeaveRequestVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyLeave(LeaveRequestVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = GetCurrentUserId();
            var (success, message) = await _leaveService.ApplyLeaveAsync(userId, model);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, message);
                return View(model);
            }

            TempData["SuccessMessage"] = message;
            return RedirectToAction("Dashboard");
        }

        public async Task<IActionResult> LeaveHistory()
        {
            var userId = GetCurrentUserId();
            var history = await _leaveService.GetLeaveHistoryAsync(userId);
            return View(history);
        }

        public async Task<IActionResult> LeaveBalance()
        {
            var userId = GetCurrentUserId();
            var balances = await _leaveService.GetLeaveBalancesAsync(userId);
            return View(balances);
        }
    }
}
