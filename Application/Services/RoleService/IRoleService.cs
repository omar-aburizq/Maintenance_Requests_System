using Application.Services.RoleService.DTOs;

namespace Application.Services.RoleService
{
    public interface IRoleService
    {
        Task<List<GetAllRolesDto>> GetAllRoles();
    }
}
