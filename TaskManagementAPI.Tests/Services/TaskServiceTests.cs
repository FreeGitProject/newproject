using Moq;
using TaskManagementAPI.Data;
using TaskManagementAPI.Services;
using TaskManagementAPI.Tests.TestHelpers;

namespace TaskManagementAPI.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly ITaskService _taskService;
        private readonly Mock<AppDbContext> _mockContext;
        private readonly List<Models.Task> _tasks;

        public TaskServiceTests()
        {
            _tasks = new List<Models.Task>
        {
            new Models.Task { Id = 1, Title = "Task 1", UserId = 1 },
            new Models.Task { Id = 2, Title = "Task 2", UserId = 1 },
            new Models.Task { Id = 3, Title = "Task 3", UserId = 2 }
        };

            var mockSet = _tasks.AsQueryable().BuildMockDbSet();

            _mockContext = new Mock<AppDbContext>();
            _mockContext.Setup(c => c.Tasks).Returns(mockSet.Object);

            _taskService = new TaskService(_mockContext.Object);
        }

        [Fact]
        public async Task GetTask_ReturnsTask_WhenTaskExists()
        {
            // Act
            var result = await _taskService.GetTask(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task GetTask_ReturnsNull_WhenTaskDoesNotExist()
        {
            // Act
            var result = await _taskService.GetTask(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetTasksByUser_ReturnsOnlyUsersTasks()
        {
            // Act
            var result = await _taskService.GetTasksByUser(1);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.All(result, t => Assert.Equal(1, t.UserId));
        }
    }
}
