using TaskManagementAPI.DTOs.Request;
using TaskManagementAPI.DTOs.Response;

namespace TaskManagementAPI.Services
{
    public interface ITaskService
    {
        Task<ApiResponse<TaskResponseDto>> GetTask(int id);
        Task<List<Models.Task>> GetAllTasks();
        Task<ApiResponse<TaskResponseDto>> CreateTask(CreateTaskDto task);
        Task<ApiResponse<List<TaskResponseDto>>> GetTasksByUser(int userId);
    }
}
