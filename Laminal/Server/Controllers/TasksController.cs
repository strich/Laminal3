using Laminal.Server.Models;
using Laminal.Shared.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stl.Fusion;
using Stl.Fusion.Server;

namespace Laminal.Server.Controllers
{
    public class TasksController : BaseAPIController, ITaskService
    {
        readonly ITaskService _taskService;

        public TasksController(AppDbContext context, ILogger<TasksController> logger, IConfiguration config, ITaskService taskService)
            : base(context, logger, config)
        {
            _context = context;
            _logger = logger;
            _taskService = taskService;
        }

        [HttpGet, Publish]
        public async Task<List<Shared.Models.Task>> GetTasks(int projectId)
            => await _taskService.GetTasks(projectId);

        [HttpGet, Publish]
        public async Task<Shared.Models.Task> GetTask(int taskId)
        {
            return await _taskService.GetTask(taskId);
        }

        [HttpGet, Publish]
        public async Task<Shared.Models.TaskProperty> GetTaskProperty(int taskId, string name)
        {
            return await _taskService.GetTaskProperty(taskId, name);
        }

        //[HttpPatch("{id}")]
        //public async Task PatchTask(int id, [FromBody] JsonPatchDocument<Shared.Models.Task> patchDoc)
        //{
        //    await _taskService.PatchTask(id, patchDoc);
        //    //return Ok(patchDoc);
        //}

        [HttpPost]
        public async ValueTask SetTaskProperty(int taskId, Shared.Models.TaskProperty property)
        {
            await _taskService.SetTaskProperty(taskId, property);
        }
    }
}
