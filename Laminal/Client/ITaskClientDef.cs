using Laminal.Shared.Services;
using RestEase;

namespace Laminal.Client
{
    [BasePath("tasks")]
    public interface ITaskClientDef
    {
        [Get(nameof(GetTask))]
        Task<Laminal.Shared.Models.Task> GetTask(int taskId, CancellationToken cancellationToken = default);
        [Get(nameof(GetTasks))]
        Task<IList<int>> GetTasks(int projectId, CancellationToken cancellationToken = default);
        [Get(nameof(GetTaskProperty))]
        Task<Laminal.Shared.Models.TaskProperty> GetTaskProperty(int tpId, CancellationToken cancellationToken = default);
        [Post(nameof(SetTaskProperty))]
        Task SetTaskProperty([Body] SetTaskPropertyCommand command, CancellationToken cancellationToken = default);
        [Post(nameof(CreateTask))]
        Task CreateTask([Body] CreateTaskCommand command, CancellationToken cancellationToken = default);
    }
}
