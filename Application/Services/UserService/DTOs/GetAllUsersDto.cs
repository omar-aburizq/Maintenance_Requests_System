namespace Application.Services.UserService.DTOs
{
    public class GetAllUsersDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Location { get; set; }
        public string RoleName { get; set; }
    }
}
