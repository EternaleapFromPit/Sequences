namespace SequenceWebApi.Processing
{
    public interface ITasksScheduler
    {
        Task<TaskDto> Create(TaskDto task);
    }
}