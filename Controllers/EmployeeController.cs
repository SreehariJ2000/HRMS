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

        public async Task<IActionResult> Dashboard()
        {
            var model = await _leaveService.GetEmployeeDashboardAsync();
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

            var (success, message) = await _leaveService.ApplyLeaveAsync(model);

            if (!success)
            {
                TempData["ErrorMessage"] = message;
                return View(model);
            }

            TempData["SuccessMessage"] = message;
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelLeave(int id)
        {
            var (success, message) = await _leaveService.CancelLeaveAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = message;
            }
            else
            {
                TempData["ErrorMessage"] = message;
            }
            return Redirect(Request.Headers["Referer"].ToString() ?? "/Employee/Dashboard");
        }

        public async Task<IActionResult> LeaveHistory(string searchString, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;
            int pageSize = 10;
            var history = await _leaveService.GetPaginatedLeaveHistoryAsync(searchString, pageNumber ?? 1, pageSize);
            return View(history);
        }

        public async Task<IActionResult> LeaveBalance()
        {
            var balances = await _leaveService.GetLeaveBalancesAsync();
            return View(balances);
        }
    }
}
