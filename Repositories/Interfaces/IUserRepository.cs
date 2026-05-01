using HRMS.Models;

namespace HRMS.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByEmployeeCodeAsync(string employeeCode);
        Task<List<User>> GetAllEmployeesAsync();
        Task<int> GetEmployeeCountAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);
        Task<bool> EmployeeCodeExistsAsync(string employeeCode, int? excludeUserId = null);
    }
}
