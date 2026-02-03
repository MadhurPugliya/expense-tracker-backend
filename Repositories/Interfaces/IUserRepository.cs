using ExpenseTracker.Models;

namespace ExpenseTracker.Repositories.interfaces
{
    public interface IUserRepository
    {
        Task<User> RegisterUserAsync(User user);
        Task<bool> EmailExistsAsync(string email);
        Task<User> GetUserByEmailAsync(string email);
    }
}