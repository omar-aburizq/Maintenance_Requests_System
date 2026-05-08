using Domain.Enums;

namespace Application.Services.RoleService.DTOs
{
    public class GetAllRolesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SystemRole Code { get; set; }
    }
}
