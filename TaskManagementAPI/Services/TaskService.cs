using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.DTOs.Request;
using TaskManagementAPI.DTOs.Response;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public TaskService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse<TaskResponseDto>> GetTask(int id)
        {
            var task = await _dbContext.Tasks
                .Include(t => t.User)
                .Include(t => t.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                return new ApiResponse<TaskResponseDto> { Success = false, Message = "Task not found" };

            var result = _mapper.Map<TaskResponseDto>(task);
            return new ApiResponse<TaskResponseDto> { Data = result, Success = true };
        }

        public async Task<List<Models.Task>> GetAllTasks()
        {
            return await _dbContext.Tasks.ToListAsync();
        }

        public async Task<ApiResponse<TaskResponseDto>> CreateTask(CreateTaskDto createTaskDto)
        {
            var task = _mapper.Map<Models.Task>(createTaskDto);
            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            var result = _mapper.Map<TaskResponseDto>(task);
            return new ApiResponse<TaskResponseDto> { Data = result, Success = true };
        }

        public async Task<ApiResponse<List<TaskResponseDto>>> GetTasksByUser(int userId)
        {
            var taskLst= await _dbContext.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();
            var result = _mapper.Map<List<TaskResponseDto>>(taskLst);
            return new ApiResponse<List<TaskResponseDto>> { Data = result, Success = true };
        }
        public async Task<ApiResponse<TaskCommentResponseDto>> AddCommentToTask(CreateCommentDto createCommentDto)
        {
           
                var comment = _mapper.Map<TaskComment>(createCommentDto);
                comment.CreatedDate = DateTime.UtcNow;

                await _dbContext.TaskComments.AddAsync(comment);
                await _dbContext.SaveChangesAsync();

                var savedComment = await _dbContext.TaskComments
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == comment.Id);

                var result = _mapper.Map<TaskCommentResponseDto>(savedComment);
                return new ApiResponse<TaskCommentResponseDto>
                {
                    Success = true,
                    Data = result
                };
         
        }

        public async Task<ApiResponse<List<TaskCommentResponseDto>>> GetTaskComments(int taskId)
        {
            var comments = await _dbContext.TaskComments
                     .Include(c => c.User)
                     .Where(c => c.TaskId == taskId)
                     .ToListAsync();

            var result = _mapper.Map<List<TaskCommentResponseDto>>(comments);
            return new ApiResponse<List<TaskCommentResponseDto>>
            {
                Success = true,
                Data = result
            };
        }
    }
}
