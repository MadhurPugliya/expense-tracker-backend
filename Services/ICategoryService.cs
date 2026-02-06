using ExpenseTracker.Dtos;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto, int userId);
        Task<List<CategoryResponseDto>> GetAllCategoriesAsync(int userId);
    }
}