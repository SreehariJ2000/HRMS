using HRMS.Models;
using HRMS.Models.Enums;
using HRMS.Repositories.Interfaces;
using HRMS.Services.Interfaces;
using HRMS.ViewModels;

namespace HRMS.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;

        public EmployeeService(IUserRepository userRepository, ILeaveBalanceRepository leaveBalanceRepository)
        {
            _userRepository = userRepository;
            _leaveBalanceRepository = leaveBalanceRepository;
        }

        public async Task<List<EmployeeVM>> GetAllEmployeesAsync()
        {
            var employees = await _userRepository.GetAllEmployeesAsync();
            return employees.Select(e => new EmployeeVM
            {
                Id = e.Id,
                EmployeeCode = e.EmployeeCode,
                Name = e.Name,
                Email = e.Email,
                Department = e.Department,
                DateOfJoining = e.DateOfJoining
            }).ToList();
        }

        public async Task<EmployeeVM?> GetEmployeeByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null || user.Role != UserRole.Employee)
                return null;

            return new EmployeeVM
            {
                Id = user.Id,
                EmployeeCode = user.EmployeeCode,
                Name = user.Name,
                Email = user.Email,
                Department = user.Department,
                DateOfJoining = user.DateOfJoining
            };
        }

        public async Task<(bool Success, string Message)> CreateEmployeeAsync(EmployeeVM model)
        {
            // Validate uniqueness
            if (await _userRepository.EmailExistsAsync(model.Email))
                return (false, "An employee with this email already exists.");

            if (await _userRepository.EmployeeCodeExistsAsync(model.EmployeeCode))
                return (false, "An employee with this code already exists.");

            if (string.IsNullOrWhiteSpace(model.Password))
                return (false, "Password is required for new employee.");

            var user = new User
            {
                EmployeeCode = model.EmployeeCode,
                Name = model.Name,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Department = model.Department,
                DateOfJoining = model.DateOfJoining,
                Role = UserRole.Employee
            };

            await _userRepository.AddAsync(user);

            // Initialize leave balances for current year
            await _leaveBalanceRepository.InitializeBalancesForUserAsync(user.Id, DateTime.UtcNow.Year);

            return (true, "Employee created successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateEmployeeAsync(EmployeeVM model)
        {
            var user = await _userRepository.GetByIdAsync(model.Id);
            if (user == null)
                return (false, "Employee not found.");

            // Check for email uniqueness (excluding current user)
            if (await _userRepository.EmailExistsAsync(model.Email, model.Id))
                return (false, "An employee with this email already exists.");

            if (await _userRepository.EmployeeCodeExistsAsync(model.EmployeeCode, model.Id))
                return (false, "An employee with this code already exists.");

            user.EmployeeCode = model.EmployeeCode;
            user.Name = model.Name;
            user.Email = model.Email;
            user.Department = model.Department;
            user.DateOfJoining = model.DateOfJoining;

            // Only update password if a new one is provided
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
            }

            await _userRepository.UpdateAsync(user);
            return (true, "Employee updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteEmployeeAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return (false, "Employee not found.");

            if (user.Role == UserRole.Admin)
                return (false, "Cannot delete admin users.");

            await _userRepository.DeleteAsync(id);
            return (true, "Employee deleted successfully.");
        }
    }
}
