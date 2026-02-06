using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ExpenseTracker.Dtos;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IFileLoggerService _logger;

        public CategoryController(ICategoryService categoryService, IFileLoggerService logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        #region Category
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDto categoryCreateDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                
                await _logger.LogInformationAsync("Creating category", new { Name = categoryCreateDto.Name, UserId = userId });
                
                var createdCategory = await _categoryService.CreateCategoryAsync(categoryCreateDto, userId);
                
                await _logger.LogInformationAsync("Category created successfully", new { Id = createdCategory.Id, UserId = userId });
                
                return CreatedAtAction(nameof(CreateCategory), new { id = createdCategory.Id }, createdCategory);
            }
            catch (InvalidOperationException ex)
            {
                await _logger.LogWarningAsync("Category creation failed", new { Name = categoryCreateDto.Name, Error = ex.Message });
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Error creating category", ex, new { Name = categoryCreateDto.Name });
                return StatusCode(500, new { message = "An error occurred while creating the category" });
            }
        }

       [HttpGet]
       [Authorize]
       public async Task<IActionResult> GetAllCategories()
       {
           try
           {
               var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
               
               await _logger.LogInformationAsync("Fetching all categories", new { UserId = userId });
               
               var categories = await _categoryService.GetAllCategoriesAsync(userId);
               
               await _logger.LogInformationAsync("Categories fetched successfully", new { Count = categories.Count, UserId = userId });
               
               return Ok(categories);
           }
           catch (Exception ex)
           {
               await _logger.LogErrorAsync("Error fetching categories", ex, new { });
               return StatusCode(500, new { message = "An error occurred while fetching categories" });
           }
       }




        #endregion
    }
}