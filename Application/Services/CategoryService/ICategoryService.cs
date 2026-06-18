using Application.Services.CategoryService.DTOs;

namespace Application.Services.CategoryService
{
    public interface ICategoryService
    {
        Task CreateCategory(CreateCategoryDto input);
        Task<List<GetAllCategoriesDto>> GetAllCategories();
        Task<GetCategoryByIdDto> GetCategoryById(Guid id);
        Task UpdateCategory(Guid id, UpdateCategoryDto input);
        Task DeleteCategory(Guid id);
    }
}
