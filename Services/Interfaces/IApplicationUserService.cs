namespace HRMS.Services.Interfaces
{
    public interface IApplicationUserService
    {
        int? GetUserId();
        string? GetUserEmail();
        string? GetUserRole();
        string? GetUserName();
    }
}
