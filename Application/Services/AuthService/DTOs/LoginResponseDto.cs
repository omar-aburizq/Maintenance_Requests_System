using Domain.Enums;

namespace Application.Services.AuthService.DTOs
{
    public class LoginResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Location { get; set; }
        public string RoleName { get; set; }
        public SystemRole RoleCode { get; set; }

        public string AcessToken { get; set; }
        public string RefershToken { get; set; }
    }
}
