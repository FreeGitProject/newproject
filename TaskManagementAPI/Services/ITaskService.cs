using TaskManagementAPI.DTOs.Request;
using TaskManagementAPI.DTOs.Response;

namespace TaskManagementAPI.Services
{
    public interface ITaskService
    {
        Task<ApiResponse<TaskResponseDto>> GetTask(int id);
        Task<ApiResponse<TaskResponseDto>> CreateTask(CreateTaskDto task);
        Task<ApiResponse<TaskResponseDto>> UpdateTask(int id, UpdateTaskDto updateTask);
        Task<ApiResponse<List<TaskResponseDto>>> GetTasksByUser(int userId);
        Task<ApiResponse<List<TaskCommentResponseDto>>> GetTaskComments(int taskId);
        Task<ApiResponse<TaskCommentResponseDto>> AddCommentToTask(CreateCommentDto createCommentDto);
        Task<ApiResponse<List<TaskResponseDto>>> GetAllTasks();
    }
}
