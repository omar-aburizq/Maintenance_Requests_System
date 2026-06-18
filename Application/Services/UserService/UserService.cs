using Application.Repositories;
using Application.Services.UserService.DTOs;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;
        private readonly IGenericRepository<TechnicianCategory> _technicianCategoryRepository;
        public UserService(IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository, IGenericRepository<TechnicianCategory> technicianCategoryRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _technicianCategoryRepository = technicianCategoryRepository;
        }

        public async Task CreateUser(CreateUserDto input)
        {
            if (await _userRepository.GetAll().AnyAsync(x => x.Email.ToLower().Trim() == input.Email.ToLower().Trim()))
                throw new Exception("Email Already Exist");

            if (await _userRepository.GetAll().AnyAsync(x => x.PhoneNumber.Trim() == input.PhoneNumber.Trim()))
                throw new Exception("PhoneNumber Already Exist");

            var role = await _roleRepository.GetByIdAsync(input.RoleId);

            if (role == null)
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


            if (role.Code == SystemRole.Technician)
            {
                if (input.CategoryIds == null || !input.CategoryIds.Any())
                    throw new Exception("Technician must have at least one category");

                foreach (var categoryId in input.CategoryIds.Distinct())
                {
                    await _technicianCategoryRepository.InsertAsync(new TechnicianCategory
                    {
                        TechnicianId = data.Id,
                        CategoryId = categoryId,
                    });
                }
                await _technicianCategoryRepository.SaveChangesAsync();
            }

        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new Exception("User not found");

            user.IsActive = false;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }


        public async Task<List<GetAllUsersDto>> GetAllUsers(string? name, string? email)
        {
            name = !string.IsNullOrEmpty(name) ? name.ToLower().Trim() : null;
            email = !string.IsNullOrEmpty(email) ? email.ToLower().Trim() : null;

            var usersQuery = _userRepository.GetAll().Where(u => u.IsActive == true);

            if (name != null)
                usersQuery = usersQuery.Where(x => x.Name.ToLower().Trim().Contains(name));

            if (email != null)
                usersQuery = usersQuery.Where(x => x.Email.ToLower().Trim().Contains(email));


            var users = await usersQuery.Include(x => x.Role).ToListAsync();

            var userDtos = new List<GetAllUsersDto>();

            foreach (var user in users)
            {
                var dto = new GetAllUsersDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Location = user.Location,
                    RoleName = user.Role.Name
                };

                if (user.Role.Code == SystemRole.Technician)
                {
                    var categories = await _technicianCategoryRepository.GetAll().Include(x => x.Category).Where(x => x.TechnicianId == user.Id).ToListAsync();

                    dto.TechnicianCategories = categories.Select(x => new GetUsersTechnicianCategoriesDto
                    {
                        Id = x.CategoryId,
                        Name = x.Category.Name
                    }).ToList();
                }

                userDtos.Add(dto);
            }

            return userDtos;
        }


        public async Task<GetUserDto> GetUserById(Guid id)
        {
            var user = await _userRepository.GetAll().Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id && x.IsActive);

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

            if (user.Role.Code == SystemRole.Technician)
            {
                var categories = await _technicianCategoryRepository.GetAll().Include(x => x.Category).Where(x => x.TechnicianId == user.Id).ToListAsync();

                result.TechnicianCategories = categories.Select(x => new GetUsersTechnicianCategoriesDto
                {
                    Id = x.CategoryId,
                    Name = x.Category.Name
                }).ToList();
            }

            return result;
        }

        public async Task<List<GetAllUsersDto>> GetUsersTechnicians(Guid? categoryId = null)
        {
            var techniciansQuery = _userRepository.GetAll().Include(x => x.Role).Where(x => x.Role.Code == SystemRole.Technician && x.IsActive == true);

            if (categoryId.HasValue)
            {
                var technicianIdsInCategory = _technicianCategoryRepository.GetAll().Where(x => x.CategoryId == categoryId.Value).Select(x => x.TechnicianId);

                techniciansQuery = techniciansQuery.Where(x => technicianIdsInCategory.Contains(x.Id));
            }

            var technicians = await techniciansQuery.ToListAsync();

            var technicianDtos = new List<GetAllUsersDto>();

            foreach (var tech in technicians)
            {
                var dto = new GetAllUsersDto
                {
                    Id = tech.Id,
                    Name = tech.Name,
                    Email = tech.Email,
                    PhoneNumber = tech.PhoneNumber,
                    Location = tech.Location,
                    RoleName = tech.Role.Name,
                };

                var categories = await _technicianCategoryRepository.GetAll().Include(x => x.Category).Where(x => x.TechnicianId == tech.Id).ToListAsync();

                dto.TechnicianCategories = categories.Select(x => new GetUsersTechnicianCategoriesDto
                {
                    Id = x.CategoryId,
                    Name = x.Category.Name
                }).ToList();

                technicianDtos.Add(dto);
            }

            return technicianDtos;
        }

        public async Task UpdateUser(Guid id, UpdateUserDto input)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new Exception("User not found");

            if (await _userRepository.GetAll().AnyAsync(x => x.Email.ToLower().Trim() == input.Email.ToLower().Trim() && x.Id != id))
                throw new Exception("Email Already Exist");

            if (await _userRepository.GetAll().AnyAsync(x => x.PhoneNumber.Trim() == input.PhoneNumber.Trim() && x.Id != id))
                throw new Exception("PhoneNumber Already Exist");

            var role = await _roleRepository.GetByIdAsync(input.RoleId);

            if (role == null)
                throw new Exception("Role not found");

            user.Name = input.Name.Trim();
            user.Email = input.Email.ToLower().Trim();
            user.PhoneNumber = input.PhoneNumber.Trim();
            user.Location = input.Location?.Trim();
            user.RoleId = input.RoleId;

            var oldTechnicianCategories = await _technicianCategoryRepository.GetAll().Where(x => x.TechnicianId == user.Id).ToListAsync();

            foreach (var item in oldTechnicianCategories)
            {
                _technicianCategoryRepository.Delete(item);
            }

            if (role.Code == SystemRole.Technician)
            {
                if (input.CategoryIds == null || !input.CategoryIds.Any())
                    throw new Exception("Technician must have at least one category");

                foreach (var categoryId in input.CategoryIds.Distinct())
                {
                    await _technicianCategoryRepository.InsertAsync(new TechnicianCategory
                    {
                        TechnicianId = user.Id,
                        CategoryId = categoryId
                    });
                }
            }

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
        }

    }
}
