using ExpenseTracker.Dtos;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(UserRegisterDto userDto);

        Task<string> LoginUserAsync(UserLoginDto userDto);
    }
}