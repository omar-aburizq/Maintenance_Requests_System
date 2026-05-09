namespace Application.Services.UserService.DTOs
{
    public class CreateUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string? Location { get; set; }
        public Guid RoleId { get; set; }
        public List<Guid>? CategoryIds { get; set; }
    }
}
