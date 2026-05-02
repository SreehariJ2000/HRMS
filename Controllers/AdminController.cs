using HRMS.Services.Interfaces;
using HRMS.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILeaveService _leaveService;

        public AdminController(IEmployeeService employeeService, ILeaveService leaveService)
        {
            _employeeService = employeeService;
            _leaveService = leaveService;
        }
        public async Task<IActionResult> Dashboard()
        {
            var model = await _leaveService.GetAdminDashboardAsync();
            return View(model);
        }

        public async Task<IActionResult> Employees(string searchString, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;
            int pageSize = 10;
            var employees = await _employeeService.GetPaginatedEmployeesAsync(searchString, pageNumber ?? 1, pageSize);
            return View(employees);
        }

        [HttpGet]
        public IActionResult CreateEmployee()
        {
            return View(new EmployeeVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee(EmployeeVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (success, message) = await _employeeService.CreateEmployeeAsync(model);
            if (!success)
            {
                TempData["ErrorMessage"] = message;
                return View(model);
            }

            TempData["SuccessMessage"] = message;
            return RedirectToAction("Employees");
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(EmployeeVM model)
        {
            ModelState.Remove("Password");

            if (!ModelState.IsValid)
                return View(model);

            var (success, message) = await _employeeService.UpdateEmployeeAsync(model);
            if (!success)
            {
                TempData["ErrorMessage"] = message;
                return View(model);
            }

            TempData["SuccessMessage"] = message;
            return RedirectToAction("Employees");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var (success, message) = await _employeeService.DeleteEmployeeAsync(id);
            if (!success)
            {
                TempData["ErrorMessage"] = message;
            }
            else
            {
                TempData["SuccessMessage"] = message;
            }
            return RedirectToAction("Employees");
        }

        public async Task<IActionResult> LeaveRequests(string searchString, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;
            int pageSize = 10;
            var requests = await _leaveService.GetPaginatedPendingRequestsAsync(searchString, pageNumber ?? 1, pageSize);
            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveLeave(int leaveRequestId, string? adminRemarks)
        {
            var (success, message) = await _leaveService.ApproveLeaveAsync(leaveRequestId, adminRemarks);
            if (success)
                TempData["SuccessMessage"] = message;
            else
                TempData["ErrorMessage"] = message;

            return RedirectToAction("LeaveRequests");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectLeave(int leaveRequestId, string? adminRemarks)
        {
            var (success, message) = await _leaveService.RejectLeaveAsync(leaveRequestId, adminRemarks);
            if (success)
                TempData["SuccessMessage"] = message;
            else
                TempData["ErrorMessage"] = message;

            return RedirectToAction("LeaveRequests");
        }

        public async Task<IActionResult> LeaveHistory(string searchString, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;
            int pageSize = 10;
            var history = await _leaveService.GetPaginatedAdminLeaveHistoryAsync(searchString, pageNumber ?? 1, pageSize);
            return View(history);
        }
    }
}
