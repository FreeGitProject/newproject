namespace TaskManagementAPI.Services
{
    public interface ITaskService
    {
        Task<Models.Task> GetTask(int id);
        Task<List<Models.Task>> GetAllTasks();
        Task<Models.Task> CreateTask(Models.Task task);
        Task<List<Models.Task>> GetTasksByUser(int userId);
    }
}
