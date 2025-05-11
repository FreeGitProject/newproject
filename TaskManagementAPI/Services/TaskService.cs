using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;

namespace TaskManagementAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _dbContext;

        public TaskService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Models.Task> GetTask(int id)
        {
            return await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Models.Task>> GetAllTasks()
        {
            return await _dbContext.Tasks.ToListAsync();
        }

        public async Task<Models.Task> CreateTask(Models.Task task)
        {
            task.CreatedDate = DateTime.UtcNow;
            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();
            return task;
        }

        public async Task<List<Models.Task>> GetTasksByUser(int userId)
        {
            return await _dbContext.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }
    }
}
