using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CreateTask([FromBody] Models.Task task)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdTask = await _taskService.CreateTask(task);
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, createdTask);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _taskService.GetTask(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTasksByUser(int userId)
        {
            var tasks = await _taskService.GetTasksByUser(userId);
            return Ok(tasks);
        }
    }
}
