using Application.Services.UserService.DTOs;

namespace Application.Services.UserService
{
    public interface IUserService
    {
        public Task CreateUser(CreateUserDto input);
        public Task<List<GetAllUsersDto>> GetAllUsers(string? name , string? email);
        public Task<GetUserDto> GetUserById(Guid id);
        public Task UpdateUser(Guid id , UpdateUserDto input);
        public Task DeleteUser(Guid id);
    }
}
