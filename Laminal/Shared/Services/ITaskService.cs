using Microsoft.AspNetCore.JsonPatch;
using Stl.CommandR;
using Stl.Fusion;
using Stl.Fusion.Authentication;
using System.Reactive;
using System.Runtime.Serialization;

namespace Laminal.Shared.Services
{
    public interface ITaskService : IComputeService
    {
        [ComputeMethod]
        Task<List<Models.Task>> GetTasks(int projectId);
        [ComputeMethod]
        Task<Models.Task> GetTask(int taskId);
        [ComputeMethod]
        Task<Models.TaskProperty> GetTaskProperty(int taskId, string name);
        //[ComputeMethod]
        Task SetTaskProperty(SetTaskPropertyCommand command);
        //Task PatchTask(int id, JsonPatchDocument<Models.Task> patchDoc);
    }

    [DataContract]
    public sealed record SetTaskPropertyCommand(
        //[property: DataMember] Session Session,
        [property: DataMember] int TaskPropertyId,
        [property: DataMember] string Value
    ) : ICommand<Unit>;//: ISessionCommand<Unit>;
}
