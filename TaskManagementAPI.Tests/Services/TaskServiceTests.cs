using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using TaskManagementAPI.Controllers;
using TaskManagementAPI.Data;
using TaskManagementAPI.DTOs.Request;
using TaskManagementAPI.DTOs.Response;
using TaskManagementAPI.Mappings;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Tests.Services
{
    public class TaskServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<TaskService>> _mockLogger;

        public TaskServiceTests()
        {
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test
                .Options;

            _context = new AppDbContext(options);

            // Seed test data
            SeedTestData();

            // Configure AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new TaskManagementProfile());
            });
            _mapper = config.CreateMapper();

            // Mock logger
            _mockLogger = new Mock<ILogger<TaskService>>();

            // Create service instance
            _taskService = new TaskService(_context, _mapper);
        }

        private void SeedTestData()
        {
            var users = new List<User>
            {
                new User { Id = 1, Username = "user1", Password = "pass1", Role = "User" },
                new User { Id = 2, Username = "user2", Password = "pass2", Role = "User" },
                new User { Id = 3, Username = "admin", Password = "admin123", Role = "Admin" }
            };

            var tasks = new List<Models.Task>
            {
                new Models.Task
                {
                    Id = 1,
                    Title = "Task 1",
                    Description = "Description 1",
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    DueDate = DateTime.UtcNow.AddDays(5),
                    IsCompleted = false,
                    UserId = 1,
                    User = users[0]
                },
                new Models.Task
                {
                    Id = 2,
                    Title = "Task 2",
                    Description = "Description 2",
                    CreatedDate = DateTime.UtcNow.AddDays(-1),
                    DueDate = DateTime.UtcNow.AddDays(3),
                    IsCompleted = true,
                    UserId = 1,
                    User = users[0]
                },
                new Models.Task
                {
                    Id = 3,
                    Title = "Task 3",
                    Description = "Description 3",
                    CreatedDate = DateTime.UtcNow,
                    DueDate = null,
                    IsCompleted = false,
                    UserId = 2,
                    User = users[1]
                }
            };

            var comments = new List<TaskComment>
            {
                new TaskComment
                {
                    Id = 1,
                    Content = "Comment 1",
                    CreatedDate = DateTime.UtcNow.AddHours(-1),
                    TaskId = 1,
                    UserId = 1,
                    User = users[0]
                },
                new TaskComment
                {
                    Id = 2,
                    Content = "Comment 2",
                    CreatedDate = DateTime.UtcNow.AddHours(-2),
                    TaskId = 1,
                    UserId = 2,
                    User = users[1]
                }
            };

            _context.Users.AddRange(users);
            _context.Tasks.AddRange(tasks);
            _context.TaskComments.AddRange(comments);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTask_ReturnsTask_WhenTaskExists()
        {
            // Act
            var result = await _taskService.GetTask(1);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("Task 1", result.Data.Title);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Act
            var result = await _taskService.GetTask(999);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Contains("not found", result.Message);
        }

        [Fact]
        public async System.Threading.Tasks.Task GetTasksByUser_ReturnsOnlyUsersTasks()
        {
            // Act
            var result = await _taskService.GetTasksByUser(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count);
            Assert.All(result.Data, t => Assert.Equal(1, t.UserId));
        }

        [Fact]
        public async System.Threading.Tasks.Task CreateTask_ReturnsCreatedTask()
        {
            // Arrange
            var newTaskDto = new CreateTaskDto
            {
                Title = "New Task",
                Description = "New Description",
                UserId = 1
            };

            // Act
            var result = await _taskService.CreateTask(newTaskDto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("New Task", result.Data.Title);
            Assert.Equal(1, result.Data.UserId);

            // Verify it was saved to database
            var dbTask = await _context.Tasks.FindAsync(result.Data.Id);
            Assert.NotNull(dbTask);
        }

        //[Fact]
        //public async Task UpdateTask_ReturnsUpdatedTask()
        //{
        //    // Arrange
        //    var updateDto = new UpdateTaskDto
        //    {
        //        Title = "Updated Title",
        //        Description = "Updated Description",
        //        IsCompleted = true
        //    };

        //    // Act
        //    var result = await _taskService.UpdateTask(1, updateDto);

        //    // Assert
        //    Assert.True(result.Success);
        //    Assert.NotNull(result.Data);
        //    Assert.Equal("Updated Title", result.Data.Title);
        //    Assert.Equal("Updated Description", result.Data.Description);
        //    Assert.True(result.Data.IsCompleted);

        //    // Verify database was updated
        //    var dbTask = await _context.Tasks.FindAsync(1);
        //    Assert.Equal("Updated Title", dbTask.Title);
        //}

        //[Fact]
        //public async System.Threading.Tasks.Task DeleteTask_ReturnsTrue_WhenTaskExists()
        //{
        //    // Act
        //    var result = await _taskService.DeleteTask(1);

        //    // Assert
        //    Assert.True(result.Success);
        //    Assert.True(result.Data);

        //    // Verify it was removed from database
        //    var dbTask = await _context.Tasks.FindAsync(1);
        //    Assert.Null(dbTask);
        //}

        [Fact]
        public async System.Threading.Tasks.Task GetTaskComments_ReturnsComments_WhenTaskExists()
        {
            // Act
            var result = await _taskService.GetTaskComments(1);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Count);
            Assert.All(result.Data, c => Assert.Equal(1, c.TaskId));
        }

        [Fact]
        public async System.Threading.Tasks.Task AddCommentToTask_ReturnsCreatedComment()
        {
            // Arrange
            var newCommentDto = new CreateCommentDto
            {
                Content = "New Comment",
                TaskId = 1,
                UserId = 1
            };

            // Act
            var result = await _taskService.AddCommentToTask(newCommentDto);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal("New Comment", result.Data.Content);

            // Verify it was saved to database
            var dbComment = await _context.TaskComments.FindAsync(result.Data.Id);
            Assert.NotNull(dbComment);
        }

        [Fact]
        public async System.Threading.Tasks.Task AddCommentToTask_ReturnsError_WhenInvalidData()
        {
            // Arrange
            var invalidCommentDto = new CreateCommentDto
            {
                TaskId = 999,  // Non-existent task
                UserId = 999   // Non-existent user
            };

            // Act
            var result = await _taskService.AddCommentToTask(invalidCommentDto);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Contains("error", result.Message.ToLower());
        }
        [Fact]
        public async System.Threading.Tasks.Task GetAllTasks_ReturnsAllTasks_ForAdminUser()
        {
            // Arrange - Create test user with admin role
            var adminUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.Role, "Admin")
            }, "TestAuthentication"));

            var controller = new TasksController(_taskService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = adminUser }
                }
            };

            // Act
            var result = await controller.GetAllTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<List<TaskResponseDto>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(3, response.Data.Count); // Assuming 3 tasks in seed data
        }

        [Fact]
        public async System.Threading.Tasks.Task GetAllTasks_ReturnsForbidden_ForNonAdminUser()
        {
            // Arrange - Create test user with regular user role
            var regularUser = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "user1"),
                new Claim(ClaimTypes.Role, "User")
            }, "TestAuthentication"));

            var controller = new TasksController(_taskService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { User = regularUser }
                }
            };

            // Act
            var result = await controller.GetAllTasks();

            // Assert
            Assert.IsType<ForbidResult>(result);
        }
    }
}