using Laminal.Shared.Models;
using Microsoft.AspNetCore.JsonPatch;
using Stl.Fusion;

namespace Laminal.Shared.Services
{
    public interface ITaskService : IComputeService
    {
        [ComputeMethod]
        Task<List<Models.Task>> GetTasks(int projectId);
        [ComputeMethod]
        Task<Models.Task> GetTask(int taskId);
        [ComputeMethod]
        Task<TaskProperty> GetTaskProperty(int taskId, string name);
        //[ComputeMethod]
        ValueTask SetTaskProperty(int taskId, TaskProperty property);
        //Task PatchTask(int id, JsonPatchDocument<Models.Task> patchDoc);
    }
}
