using Domain.Entities;
using Domain.Enums;
using Infrastructuer.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructuer.Data
{
    public class UserSeedData
    {
        private readonly static string adminPassword = "Admin@123";

        public static void UserSeed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Roles
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Name=SystemRole.Admin.ToString() , Code= SystemRole.Admin },
                    new Role { Name=SystemRole.Employee.ToString(),Code= SystemRole.Employee },
                    new Role { Name=SystemRole.Technician.ToString(), Code=SystemRole.Technician }
                };
                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            // Admin User
            if (!context.Users.Any())
            {
                var adminRoleId = context.Roles.FirstOrDefault(r => r.Code == SystemRole.Admin).Id;

                var user = new User
                {
                    Name = "Admin User",
                    Email = "admin@gmail.com",
                    PhoneNumber = "00692712345678",
                    RoleId = adminRoleId,
                };

                var passwordHaser = new PasswordHasher<User>();  // Install Microsoft.Extensions.Identity.Core
                user.Password = passwordHaser.HashPassword(user, adminPassword);

                context.Users.Add(user);
                context.SaveChanges();
            }

        }
    }
}
