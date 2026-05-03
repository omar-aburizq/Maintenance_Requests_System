using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructuer.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
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
        
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Emploeey)
                .WithMany(u => u.CreatedRequests)
                .HasForeignKey(r => r.EmploeeyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Technician)
                .WithMany(u => u.AssignedRequests)
                .HasForeignKey(r => r.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);

            var relationShips = modelBuilder.Model
                .GetEntityTypes().SelectMany(e => e.GetForeignKeys());

            foreach (var relationship in relationShips)
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); // SeedData Confguration
            
            base.OnModelCreating(modelBuilder);
        }

    }
}
