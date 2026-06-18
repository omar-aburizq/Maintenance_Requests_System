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

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Employee)
                .WithMany(u => u.CreatedRequests)
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Technician)
                .WithMany(u => u.AssignedRequests)
                .HasForeignKey(r => r.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RequestDetail>()
                .HasOne(rd => rd.Request)
                .WithOne()
                .HasForeignKey<RequestDetail>(rd => rd.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TechnicianCategory>() // Prevents assigning the same category to the same technician more than once. 
                .HasIndex(x => new { x.TechnicianId, x.CategoryId })
                .IsUnique();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); // SeedData Confguration (CategoryConfiguration)
            
            var relationShips = modelBuilder.Model
                .GetEntityTypes().SelectMany(e => e.GetForeignKeys());
            

            foreach (var relationship in relationShips) // Set all foreign key relationships to Restrict to prevent cascade deletes.
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }

    }
}
