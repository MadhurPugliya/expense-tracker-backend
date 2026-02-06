using ExpenseTracker.Data;
using ExpenseTracker.Models;
using ExpenseTracker.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> CategoryExistsAsync(string name, int userId)
        {
            return await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower() && c.UserId == userId);
        }

        public async Task<List<Category>> GetByUserIdAsync(int userId)
        {
            return await _context.Categories.Where(c => c.UserId == userId).ToListAsync();
        }
    }
}