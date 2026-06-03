using Application.Services.CategoryService;
using Application.Services.CategoryService.DTOs;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Roles = nameof(SystemRole.Admin))]
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto input)
        {
            await _categoryService.CreateCategory(input);
            return Ok();
        }

        [Authorize(Roles = nameof(SystemRole.Admin))]
        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(Guid id , [FromBody] UpdateCategoryDto input)
        {
            await _categoryService.UpdateCategory(id, input);
            return Ok();
        }

        [Authorize(Roles = $"{nameof(SystemRole.Admin)},{nameof(SystemRole.Employee)},{nameof(SystemRole.Technician)}")]
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [Authorize(Roles = $"{nameof(SystemRole.Admin)},{nameof(SystemRole.Employee)},{nameof(SystemRole.Technician)}")]
        [HttpGet("GetCategoryById")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return Ok(category);
        }

        [Authorize(Roles = nameof(SystemRole.Admin))]
        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok();
        }

    }
}
