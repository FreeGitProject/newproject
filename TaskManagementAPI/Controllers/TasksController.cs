using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs.Request;
using TaskManagementAPI.DTOs.Response;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TaskResponseDto>>> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            var response = await _taskService.CreateTask(createTaskDto);

            if (!response.Success)
                return BadRequest(response);

            return CreatedAtAction(nameof(GetTask), new { id = response.Data.Id }, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TaskResponseDto>>> GetTask(int id)
        {
            var response = await _taskService.GetTask(id);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async  Task<ActionResult<ApiResponse<List<TaskResponseDto>>>> GetTasksByUser(int userId)
        {
            var response = await _taskService.GetTasksByUser(userId);
            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }
        [HttpGet("{taskId}/comments")]
        public async Task<ActionResult<ApiResponse<List<TaskCommentResponseDto>>>> GetTaskComments(int taskId)
        {
           
                if (taskId <= 0)
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Invalid task ID"
                    });
                }

                var response = await _taskService.GetTaskComments(taskId);

                if (!response.Success)
                {
                    return NotFound(response);
                }

                return Ok(response);
            
            
        }
        [HttpPost("comments")]
        public async Task<ActionResult<ApiResponse<TaskCommentResponseDto>>> AddCommentToTask([FromBody] CreateCommentDto createCommentDto)
        {
            var response = await _taskService.AddCommentToTask(createCommentDto);

            if (!response.Success)
                return BadRequest(response);

            return CreatedAtAction(
            nameof(GetTaskComments), 
            new { taskId = createCommentDto.TaskId }, 
            response);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")] // Only users with Admin role can access
        public async Task<ActionResult<ApiResponse<List<TaskResponseDto>>>> GetAllTasks()
        {

            var response = await _taskService.GetAllTasks();
            return Ok(response);

        }
    }
}
