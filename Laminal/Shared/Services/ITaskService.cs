using Microsoft.AspNetCore.JsonPatch;
using Stl.CommandR;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using Stl.Fusion.Authentication;
using System.Reactive;
using System.Runtime.Serialization;

namespace Laminal.Shared.Services
{
    public interface ITaskService : IComputeService
    {
        [ComputeMethod]
        Task<IList<int>> GetTasks(int projectId, CancellationToken cancellationToken = default);
        [ComputeMethod]
        Task<Models.Task> GetTask(int taskId, CancellationToken cancellationToken = default);
        [ComputeMethod]
        Task<Shared.Models.TaskProperty> GetTaskProperty(int tpId, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task SetTaskProperty(SetTaskPropertyCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task CreateTask(CreateTaskCommand command, CancellationToken cancellationToken = default);
    }

    [DataContract]
    public sealed record SetTaskPropertyCommand(
        //[property: DataMember] Session Session,
        [property: DataMember] int TaskPropertyId,
        [property: DataMember] string Value
    ) : ICommand<Unit>;//: ISessionCommand<Unit>;

    [DataContract]
    public sealed record CreateTaskCommand(
        //[property: DataMember] Session Session,
        [property: DataMember] int ProjectId,
        [property: DataMember] Models.Task NewTask
    ) : ICommand<Unit>;//: ISessionCommand<Unit>;
}
