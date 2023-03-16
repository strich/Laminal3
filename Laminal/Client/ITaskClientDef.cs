using Laminal.Shared.Services;
using RestEase;
using Stl;
using Stl.Fusion;
using Stl.Fusion.Authentication;
using Stl.Text;
using System.Reactive;
using System.Reflection;
using System.Runtime.Serialization;

namespace Laminal.Client
{
    [BasePath("tasks")]
    public interface ITaskClientDef
    {
        [Get(nameof(GetTask))]
        Task<Laminal.Shared.Models.Task> GetTask(int taskId, CancellationToken cancellationToken = default);
        [Get(nameof(GetTasks))]
        Task<IList<Laminal.Shared.Models.Task>> GetTasks(int projectId, CancellationToken cancellationToken = default);
        [Get(nameof(GetTaskProperty))]
        Task<Laminal.Shared.Models.TaskProperty> GetTaskProperty(int tpId, CancellationToken cancellationToken = default);
        [Post(nameof(SetTaskProperty))]
        Task SetTaskProperty([Body] SetTaskPropertyCommand command, CancellationToken cancellationToken = default);
    }
}
