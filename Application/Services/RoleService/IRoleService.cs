using Application.Services.RoleService.DTOs;

namespace Application.Services.RoleService
{
    public interface IRoleService
    {
        List<GetAllRolesDto> GetAllRoles();
    }
}
