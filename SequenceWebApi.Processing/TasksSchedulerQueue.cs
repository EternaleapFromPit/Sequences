namespace SequenceWebApi.Processing
{
    public class TasksSchedulerQueue : ITasksScheduler
    {
        public async Task<TaskDto> Create(TaskDto task)
        {
            new Publisher().PublishTaskMessage(task);
            await Task.Delay(100);
            return new TaskDto();
        }
    }
}