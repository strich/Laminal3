using RestEase;
using System.Reflection;

namespace Laminal.Client
{
    [BasePath("tasks")]
    public interface ITaskClientDef
    {
        [Get(nameof(GetTasks))]
        Task<List<Laminal.Shared.Models.Task>> GetTasks(int projectId, CancellationToken cancellationToken);
    }
}
