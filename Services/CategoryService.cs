using ExpenseTracker.Dtos;
using ExpenseTracker.Models;
using ExpenseTracker.Repositories.interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExpenseTracker.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponseDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto, int userId)
        {
            if (await _categoryRepository.CategoryExistsAsync(categoryCreateDto.Name, userId))
            {
                throw new InvalidOperationException("Category already exists");
            }

            var category = new Category
            {
                Name = categoryCreateDto.Name,
                Description = categoryCreateDto.Description,
                IsActive = true,
                UserId = userId
            };

            var createdCategory = await _categoryRepository.CreateAsync(category);

            return new CategoryResponseDto
            {
                Id = createdCategory.Id,
                Name = createdCategory.Name,
                Description = createdCategory.Description,
                IsActive = createdCategory.IsActive
            };
        }

        public async Task<List<CategoryResponseDto>> GetAllCategoriesAsync(int userId)
        {
            var categories = await _categoryRepository.GetByUserIdAsync(userId);
            return categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive
            }).ToList();
        }
    }
}