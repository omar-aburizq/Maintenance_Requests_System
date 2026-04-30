using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructuer.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<RequestDetail> RequestDetails { get; set; }
        public DbSet<RequestHistory> RequestHistories { get; set; }
        public DbSet<TechnicianCategory> TechnicianCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var relationShips = modelBuilder.Model
                .GetEntityTypes().SelectMany(e => e.GetForeignKeys());

            foreach (var relationship in relationShips)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);
        }

    }
}
