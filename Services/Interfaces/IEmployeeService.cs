using HRMS.ViewModels;

namespace HRMS.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeVM>> GetAllEmployeesAsync();
        Task<EmployeeVM?> GetEmployeeByIdAsync(int id);
        Task<(bool Success, string Message)> CreateEmployeeAsync(EmployeeVM model);
        Task<(bool Success, string Message)> UpdateEmployeeAsync(EmployeeVM model);
        Task<(bool Success, string Message)> DeleteEmployeeAsync(int id);
    }
}
