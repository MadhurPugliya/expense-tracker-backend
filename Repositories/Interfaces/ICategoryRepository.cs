using ExpenseTracker.Dtos;
using ExpenseTracker.Models;

namespace ExpenseTracker.Repositories.interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<bool> CategoryExistsAsync(string name, int userId);
        Task<List<Category>> GetByUserIdAsync(int userId);
    }
}