using Application.Services.CategoryService.DTOs;

namespace Application.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task CreateCategory(CreateCategoryDto input);
        public Task<List<GetAllCategoriesDto>> GetAllCategories();
        public Task<GetCategoryByIdDto> GetCategoryById(Guid id);
        public Task UpdateCategory(Guid id , UpdateCategoryDto input);
        public Task DeleteCategory (Guid id);
    }
}
