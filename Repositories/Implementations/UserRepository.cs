using HRMS.Data;
using HRMS.Models;
using HRMS.Models.Enums;
using HRMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByEmployeeCodeAsync(string employeeCode)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.EmployeeCode == employeeCode);
        }

        public async Task<List<User>> GetAllEmployeesAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Role == UserRole.Employee)
                .OrderBy(u => u.Name)
                .ToListAsync();
        }

        public async Task<int> GetEmployeeCountAsync()
        {
            return await _context.Users
                .CountAsync(u => u.Role == UserRole.Employee);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email && (!excludeUserId.HasValue || u.Id != excludeUserId.Value));
        }

        public async Task<bool> EmployeeCodeExistsAsync(string employeeCode, int? excludeUserId = null)
        {
            return await _context.Users
                .AnyAsync(u => u.EmployeeCode == employeeCode && (!excludeUserId.HasValue || u.Id != excludeUserId.Value));
        }
    }
}
