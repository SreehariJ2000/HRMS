using HRMS.Models;
using HRMS.ViewModels;

namespace HRMS.Services.Interfaces
{
    public interface IAccountService
    {
        Task<User?> AuthenticateAsync(string email, string password);
    }
}
