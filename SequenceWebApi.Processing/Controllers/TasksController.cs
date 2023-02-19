using Microsoft.AspNetCore.Mvc;

namespace SequenceWebApi.Processing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;
        private readonly ITasksScheduler _taskScheduler;

        public TasksController(ILogger<TasksController> logger, ITasksScheduler taskScheduler)
        {
            _logger = logger;
            _taskScheduler = taskScheduler;
        }

        [HttpGet(Name = "GetTasks")]
        public async IAsyncEnumerable<TaskDto> Get()
        {
            foreach (var task in new List<TaskDto>())
                yield return task;
        }

        [HttpPost(Name = "CreateTask")]
        public async Task<TaskDto> Post(TaskDto task)
        {
            return await _taskScheduler.Create(task);
        }
    }
}