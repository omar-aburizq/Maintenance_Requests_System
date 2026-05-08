using Application.Repositories;
using Domain.Entities;

namespace Application.Services.TechnicianService
{
    public class TechnicianCategoryService : ITechnicianCategoryService
    {
        private readonly IGenericRepository<TechnicianCategory> _technicianCategoryRepository;
        public TechnicianCategoryService(IGenericRepository<TechnicianCategory> technicianCategoryRepository)
        {
            _technicianCategoryRepository = technicianCategoryRepository;
        }

    }
}
