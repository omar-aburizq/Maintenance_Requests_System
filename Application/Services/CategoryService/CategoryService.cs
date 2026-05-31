using Application.Repositories;
using Application.Services.CategoryService.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        public CategoryService(IGenericRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task CreateCategory(CreateCategoryDto input)
        {
            if (await _categoryRepository.GetAll().AnyAsync(x => x.Name.ToLower().Trim() == input.Name.ToLower().Trim()))
                throw new Exception("category name already exist");

            var data = new Category
            {
                Id = Guid.NewGuid(),
                Name = input.Name,
                Description = input.Description?.Trim(),
            };
            await _categoryRepository.InsertAsync(data);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task DeleteCategory(Guid id)
        {
            var category = await _categoryRepository.GetAll().Include(x => x.TechnicianCategories).Include(x => x.Requests).FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                throw new Exception("category not found");

            if (category.TechnicianCategories.Any())
                throw new Exception("there are technicians under this category");

            if (category.Requests.Any())
                throw new Exception("there are requests under this category");

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task<List<GetAllCategoriesDto>> GetAllCategories()
        {
            var data = _categoryRepository.GetAll();
            var result = await data.Select(x => new GetAllCategoriesDto
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
            return result;
        }

        public async Task<GetCategoryByIdDto> GetCategoryById(Guid id)
        {
            var data = await _categoryRepository.GetByIdAsync(id);

            if (data == null)
                throw new Exception("category not found");

            var result = new GetCategoryByIdDto
            {
                Id = data.Id,
                Name = data.Name,
                Description = data.Description,
            };
            return result;
        }

        public async Task UpdateCategory(Guid id, UpdateCategoryDto input)
        {
            var data = await _categoryRepository.GetByIdAsync(id);

            if (data == null)
                throw new Exception("category not found");

            if (await _categoryRepository.GetAll().AnyAsync(x => x.Name.ToLower().Trim() == input.Name.ToLower().Trim() && x.Id != id))
                throw new Exception("CategoryName Already Exist");

            data.Name = input.Name;
            data.Description = input.Description?.Trim();

            _categoryRepository.Update(data);
            await _categoryRepository.SaveChangesAsync();
        }
    }
}
