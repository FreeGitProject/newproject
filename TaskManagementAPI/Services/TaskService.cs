﻿using AutoMapper;
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
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == createCommentDto.TaskId);
            var user = await _dbContext.Users.FirstOrDefaultAsync(t => t.Id == createCommentDto.UserId);
            
            if(task == null || user == null)
            {
                return new ApiResponse<TaskCommentResponseDto>
                {
                    Success = false,
                    Message = "error",//"task or user not found",
                    Data = null
                };

            }
           
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
        public async Task<ApiResponse<List<TaskResponseDto>>> GetAllTasks()
        {
            
                var tasks = await _dbContext.Tasks
                    .Include(t => t.User)
                    .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                    .ToListAsync();

                var result = _mapper.Map<List<TaskResponseDto>>(tasks);
                return new ApiResponse<List<TaskResponseDto>>
                {
                    Success = true,
                    Data = result
                };
            
        }

        public async Task<ApiResponse<TaskResponseDto>> UpdateTask(int id, UpdateTaskDto updateTask)
        {
            var existingTask = await _dbContext.Tasks.FindAsync(id);
            if (existingTask == null)
            {
                return new ApiResponse<TaskResponseDto>
                {
                    Message = "Task is not found",
                    Success=false,
                }
                ;
            }

            _mapper.Map(updateTask, existingTask);

            await _dbContext.SaveChangesAsync();

            var result = _mapper.Map<TaskResponseDto>(existingTask);
            return new ApiResponse<TaskResponseDto>
            {
                Success = true,
                Message = "Update sucessfully",
                Data = result
            };
        }
    }
}
