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
        [Get(nameof(GetTasks))]
        Task<List<Laminal.Shared.Models.Task>> GetTasks(int projectId);
        [Get(nameof(GetTaskProperty))]
        Task<Laminal.Shared.Models.TaskProperty> GetTaskProperty(int taskId, string name);
        [Post(nameof(SetTaskProperty))]
        Task SetTaskProperty([Body] SetTaskPropertyCommand command);
    }
}
