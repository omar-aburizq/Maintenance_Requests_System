using Application.Repositories;
using Application.Services.RoleService.DTOs;
using Domain.Entities;

namespace Application.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _roleRepository;
        public RoleService(IGenericRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public List<GetAllRolesDto> GetAllRoles()
        {
            var roles = _roleRepository.GetAll();

            var result = roles.Select(x => new GetAllRolesDto
            {
                Id = x.Id,
                Name = x.Name,

            }).ToList();
            return result;

        }
    }
}
