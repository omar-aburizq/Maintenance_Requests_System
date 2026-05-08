using Application.Repositories;
using Application.Services.RoleService.DTOs;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IGenericRepository<Role> _roleRepository;
        public RoleService(IGenericRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<List<GetAllRolesDto>> GetAllRoles()
        {
            var roles = _roleRepository.GetAll();

            var result = await roles.Select(x => new GetAllRolesDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code

            }).ToListAsync();
            return result;

        }
    }
}
