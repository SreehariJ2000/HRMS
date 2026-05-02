using HRMS.ViewModels;

namespace HRMS.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<EmployeeVM>> GetAllEmployeesAsync();
        Task<HRMS.Helpers.PaginatedList<EmployeeVM>> GetPaginatedEmployeesAsync(string? searchString, int pageNumber, int pageSize);
        Task<EmployeeVM?> GetEmployeeByIdAsync(int id);
        Task<(bool Success, string Message)> CreateEmployeeAsync(EmployeeVM model);
        Task<(bool Success, string Message)> UpdateEmployeeAsync(EmployeeVM model);
        Task<(bool Success, string Message)> DeleteEmployeeAsync(int id);
    }
}
