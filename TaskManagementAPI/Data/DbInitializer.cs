using TaskManagementAPI.Models;

namespace TaskManagementAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return; // DB has been seeded
            }

            var users = new User[]
            {
            new User { Username = "admin", Password = "admin123", Role = "Admin" },
            new User { Username = "user1", Password = "user123", Role = "User" },
            new User { Username = "user2", Password = "user123", Role = "User" }
            };

            foreach (var user in users)
            {
                context.Users.Add(user);
            }
            context.SaveChanges();

            var tasks = new Models.Task[]
            {
            new Models.Task
            {
                Title = "Complete API",
                Description = "Finish the task management API",
                CreatedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(7),
                UserId = users[1].Id
            },
            new Models.Task
            {
                Title = "Write Documentation",
                Description = "Document the API endpoints",
                CreatedDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(5),
                UserId = users[1].Id
            }
            };

            foreach (var task in tasks)
            {
                context.Tasks.Add(task);
            }
            context.SaveChanges();
        }
    }
}
