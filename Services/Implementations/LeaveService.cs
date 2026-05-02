using HRMS.Helpers;
using HRMS.Models;
using HRMS.Models.Enums;
using HRMS.Repositories.Interfaces;
using HRMS.Services.Interfaces;
using HRMS.ViewModels;

namespace HRMS.Services.Implementations
{
    public class LeaveService : ILeaveService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IApplicationUserService _applicationUserService;

        public LeaveService(
            ILeaveRequestRepository leaveRequestRepository,
            ILeaveBalanceRepository leaveBalanceRepository,
            IUserRepository userRepository,
            IApplicationUserService applicationUserService)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _leaveBalanceRepository = leaveBalanceRepository;
            _userRepository = userRepository;
            _applicationUserService = applicationUserService;
        }
        public async Task<EmployeeDashboardVM> GetEmployeeDashboardAsync()
        {
            var userId = _applicationUserService.GetUserId() ?? throw new UnauthorizedAccessException("User not found.");
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var currentYear = DateTime.UtcNow.Year;
            var balances = await _leaveBalanceRepository.GetByUserAsync(userId, currentYear);
            var requests = await _leaveRequestRepository.GetByUserIdAsync(userId);

            return new EmployeeDashboardVM
            {
                EmployeeName = $"{user.FirstName} {user.LastName}",
                EmployeeCode = user.EmployeeCode,
                Department = user.Department,
                LeaveBalances = balances.Select(b => new LeaveBalanceVM
                {
                    LeaveType = b.LeaveType,
                    LeaveTypeName = FormatLeaveTypeName(b.LeaveType),
                    TotalAllocated = b.TotalAllocated,
                    Used = b.Used,
                    Balance = b.Balance
                }).ToList(),
                RecentRequests = requests.Take(10).Select(MapToDetailVM).ToList()
            };
        }
        public async Task<AdminDashboardVM> GetAdminDashboardAsync()
        {
            var today = DateTime.UtcNow.Date;
            var pendingRequests = await _leaveRequestRepository.GetPendingRequestsAsync();

            return new AdminDashboardVM
            {
                TotalEmployees = await _userRepository.GetEmployeeCountAsync(),
                PendingRequests = await _leaveRequestRepository.GetCountByStatusAsync(LeaveStatus.Pending),
                ApprovedToday = await _leaveRequestRepository.GetCountByStatusAndDateAsync(LeaveStatus.Approved, today),
                RejectedToday = await _leaveRequestRepository.GetCountByStatusAndDateAsync(LeaveStatus.Rejected, today),
                PendingLeaveRequests = pendingRequests.Select(MapToDetailVMWithUser).ToList()
            };
        }
        public async Task<(bool Success, string Message)> ApplyLeaveAsync(LeaveRequestVM model)
        {
            var userId = _applicationUserService.GetUserId() ?? throw new UnauthorizedAccessException("User not found.");
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return (false, "User not found.");

            if (model.ToDate < model.FromDate)
                return (false, "To Date cannot be earlier than From Date.");

            if (model.FromDate.Date < user.DateOfJoining.Date)
                return (false, "Cannot apply for leave before your Date of Joining.");

            int requestedDays = DateHelper.CalculateBusinessDays(model.FromDate, model.ToDate);
            if (requestedDays <= 0)
                return (false, "The selected date range contains only weekends. Please select valid working days.");

            bool hasOverlap = await _leaveRequestRepository.HasOverlappingRequestAsync(userId, model.FromDate, model.ToDate);
            if (hasOverlap)
                return (false, "You already have a leave request (Pending or Approved) that overlaps with the selected dates.");

            var currentYear = model.FromDate.Year;
            var leaveBalance = await _leaveBalanceRepository.GetAsync(userId, model.LeaveType, currentYear);
            if (leaveBalance == null)
            {

                await _leaveBalanceRepository.InitializeBalancesForUserAsync(userId, currentYear);
                leaveBalance = await _leaveBalanceRepository.GetAsync(userId, model.LeaveType, currentYear);
            }

            if (leaveBalance == null)
                return (false, "Leave balance record not found. Please contact administrator.");

            if (requestedDays > leaveBalance.Balance)
                return (false, $"Insufficient leave balance. Available: {leaveBalance.Balance} days, Requested: {requestedDays} days.");


            var leaveRequest = new LeaveRequest
            {
                UserId = userId,
                LeaveType = model.LeaveType,
                FromDate = model.FromDate,
                ToDate = model.ToDate,
                RequestedDays = requestedDays,
                Reason = model.Reason,
                Status = LeaveStatus.Pending
            };

            await _leaveRequestRepository.AddAsync(leaveRequest);
            return (true, $"Leave application submitted successfully for {requestedDays} working day(s).");
        }

        public async Task<(bool Success, string Message)> ApproveLeaveAsync(int leaveRequestId, string? adminRemarks)
        {
            var request = await _leaveRequestRepository.GetByIdAsync(leaveRequestId);
            if (request == null)
                return (false, "Leave request not found.");

            if (request.Status != LeaveStatus.Pending)
                return (false, "Only pending requests can be approved.");

            var leaveBalance = await _leaveBalanceRepository.GetAsync(request.UserId, request.LeaveType, request.FromDate.Year);
            if (leaveBalance == null)
                return (false, "Leave balance record not found.");

            if (request.RequestedDays > leaveBalance.Balance)
                return (false, $"Insufficient leave balance to approve. Available: {leaveBalance.Balance}, Required: {request.RequestedDays}.");

            leaveBalance.Used += request.RequestedDays;
            await _leaveBalanceRepository.UpdateAsync(leaveBalance);

            request.Status = LeaveStatus.Approved;
            request.AdminRemarks = adminRemarks;
            await _leaveRequestRepository.UpdateAsync(request);

            return (true, "Leave request approved successfully.");
        }

        public async Task<(bool Success, string Message)> RejectLeaveAsync(int leaveRequestId, string? adminRemarks)
        {
            var request = await _leaveRequestRepository.GetByIdAsync(leaveRequestId);
            if (request == null)
                return (false, "Leave request not found.");

            if (request.Status != LeaveStatus.Pending)
                return (false, "Only pending requests can be rejected.");

            request.Status = LeaveStatus.Rejected;
            request.AdminRemarks = adminRemarks;
            await _leaveRequestRepository.UpdateAsync(request);

            return (true, "Leave request rejected.");
        }

        public async Task<List<LeaveRequestDetailVM>> GetLeaveHistoryAsync()
        {
            var userId = _applicationUserService.GetUserId() ?? throw new UnauthorizedAccessException("User not found.");
            var requests = await _leaveRequestRepository.GetByUserIdAsync(userId);
            return requests.Select(MapToDetailVM).ToList();
        }

        public async Task<List<LeaveRequestDetailVM>> GetAdminLeaveHistoryAsync()
        {
            var requests = await _leaveRequestRepository.GetAllRequestsAsync();
            return requests.Where(r => r.Status != LeaveStatus.Pending)
                           .Select(MapToDetailVMWithUser)
                           .ToList();
        }

        public async Task<List<LeaveRequestDetailVM>> GetAllPendingRequestsAsync()
        {
            var requests = await _leaveRequestRepository.GetPendingRequestsAsync();
            return requests.Select(MapToDetailVMWithUser).ToList();
        }

        public async Task<List<LeaveBalanceVM>> GetLeaveBalancesAsync()
        {
            var userId = _applicationUserService.GetUserId() ?? throw new UnauthorizedAccessException("User not found.");
            var currentYear = DateTime.UtcNow.Year;
            var balances = await _leaveBalanceRepository.GetByUserAsync(userId, currentYear);
            return balances.Select(b => new LeaveBalanceVM
            {
                LeaveType = b.LeaveType,
                LeaveTypeName = FormatLeaveTypeName(b.LeaveType),
                TotalAllocated = b.TotalAllocated,
                Used = b.Used,
                Balance = b.Balance
            }).ToList();
        }
        private static LeaveRequestDetailVM MapToDetailVM(LeaveRequest lr)
        {
            return new LeaveRequestDetailVM
            {
                Id = lr.Id,
                UserId = lr.UserId,
                LeaveType = lr.LeaveType,
                FromDate = lr.FromDate,
                ToDate = lr.ToDate,
                RequestedDays = lr.RequestedDays,
                Reason = lr.Reason,
                Status = lr.Status,
                AdminRemarks = lr.AdminRemarks,
                CreatedAt = lr.CreationTime
            };
        }
        private static LeaveRequestDetailVM MapToDetailVMWithUser(LeaveRequest lr)
        {
            var vm = MapToDetailVM(lr);
            if (lr.User != null)
            {
                vm.EmployeeName = $"{lr.User.FirstName} {lr.User.LastName}";
                vm.EmployeeCode = lr.User.EmployeeCode;
                vm.Department = lr.User.Department;
            }
            return vm;
        }
        private static string FormatLeaveTypeName(LeaveType leaveType)
        {
            return leaveType switch
            {
                LeaveType.CasualLeave => "Casual Leave",
                LeaveType.SickLeave => "Sick Leave",
                LeaveType.EarnedLeave => "Earned Leave",
                _ => leaveType.ToString()
            };
        }
    }
}
