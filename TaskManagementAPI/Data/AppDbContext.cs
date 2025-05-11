using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships if needed
            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId);

            //modelBuilder.Entity<TaskComment>()
            //    .HasOne(tc => tc.Task)
            //    .WithMany(t => t.Comments)
            //    .HasForeignKey(tc => tc.TaskId);

            //modelBuilder.Entity<TaskComment>()
            //    .HasOne(tc => tc.User)
            //    .WithMany(u => u.Comments)
            //    .HasForeignKey(tc => tc.UserId);
            // In your DbContext's OnModelCreating method
            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(tc => tc.TaskId)
                .OnDelete(DeleteBehavior.Cascade); // Keep this for Tasks

            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(tc => tc.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Change to NoAction
        }
    }
}
