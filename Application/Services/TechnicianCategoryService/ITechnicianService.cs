using Application.Services.TechnicianService.DTOs;

namespace Application.Services.TechnicianService
{
    public interface ITechnicianService
    {
        public Task AssignTechnicianToCategory(AssignTechnicianToCategoryDto input);
        public Task RemoveTechnicianFromCategory(RemoveTechnicianFromCategoryDto input);
        public Task<List<GetTechniciansByCategoryDto>> GetTechniciansByCategory(Guid id);
        public Task<List<GetTechnicianCategoriesDto>> GetTechnicianCategories(Guid id);
    }
}
