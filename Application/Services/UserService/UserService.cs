using Application.Repositories;
using Application.Services.UserService.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        public UserService(IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task CreateUser(CreateUserDto input)
        {
            if (await _userRepository.GetAll().AnyAsync(x => x.Email.ToLower().Trim() == input.Email.ToLower().Trim()))
                throw new Exception("Email Already Exist");

            if (await _userRepository.GetAll().AnyAsync(x => x.PhoneNumber.Trim() == input.PhoneNumber.Trim()))
                throw new Exception("PhoneNumber Already Exist");

            if (!await _roleRepository.GetAll().AnyAsync(x => x.Id == input.RoleId))
                throw new Exception("Role not found");

            var data = new User
            {
                Id = Guid.NewGuid(),
                Name = input.Name.Trim(),
                Email = input.Email.ToLower().Trim(),
                PhoneNumber = input.PhoneNumber.Trim(),
                Location = input.Location?.Trim(),
                RoleId = input.RoleId,
            };
            var PasswordHasher = new PasswordHasher<User>();  // Install Microsoft.Extensions.Identity.Core
            data.Password = PasswordHasher.HashPassword(data, input.Password);

            await _userRepository.InsertAsync(data);
            await _userRepository.SaveChangesAsync();

        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new Exception("User not found");

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();

        }

        public async Task<List<GetAllUsersDto>> GetAllUsers(string? name, string? email)
        {
            name = !String.IsNullOrEmpty(name) ? name.ToLower().Trim() : null;
            email = ! String.IsNullOrEmpty(email) ? email.ToLower().Trim() : null;

            var users = _userRepository.GetAll();

            if (name != null)
                users = users.Where(x => x.Name.ToLower().Trim().Contains(name));

            if (email != null)
                users = users.Where(x => x.Email.ToLower().Trim().Contains(email));

            users = users.Include(x => x.Role);

            var result = await users.Select(x => new GetAllUsersDto
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Location = x.Location,
                RoleName = x.Role.Name,
            }).ToListAsync();

            return result;
        }

        public async Task<GetUserDto> GetUserById(Guid id)
        {
            var user = await _userRepository.GetAll().Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                throw new Exception("User not found");

            var result = new GetUserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Location = user.Location,
                RoleName = user.Role.Name,
            };
            return result;
        }

        public async Task UpdateUser(Guid id, UpdateUserDto input)
        {
            if (await _userRepository.GetAll().AnyAsync(x => x.Email.ToLower().Trim() == input.Email.ToLower().Trim() && x.Id != id))
                throw new Exception("email already exist");

            if (await _userRepository.GetAll().AnyAsync(x => x.PhoneNumber.Trim() == input.PhoneNumber.Trim() && x.Id != id))
                throw new Exception("phoneNumber already exist");

            if (!await _roleRepository.GetAll().AnyAsync(x => x.Id == input.RoleId))
                throw new Exception("Role not found");

            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new Exception("User not found");

            user.Name = input.Name.Trim();
            user.Email = input.Email.ToLower().Trim();
            user.PhoneNumber = input.PhoneNumber.Trim();
            user.Location = input.Location?.Trim();
            user.RoleId = input.RoleId;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }

    }
}
