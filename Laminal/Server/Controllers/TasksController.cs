using Laminal.Server.Models;
using Laminal.Shared.Models;
using Laminal.Shared.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stl.Fusion;
using Stl.Fusion.Server;
using System.Security.Cryptography;

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

        [HttpPost]
        public async Task<IActionResult> InitializeDb()
        {

            var project = new Project { /*Id = 1,*/ Name = "Project 1" };
            await _context.Projects.AddAsync(project);

            var resources = new List<Resource>();
            for(int i = 0; i < 2; i++)
            {
                var rsc = new Resource { /*Id = i,*/ Name = $"Resource {i}", OwnerProject = project };
                resources.Add(rsc);
                await _context.Resources.AddAsync(rsc);
            }

            var tasks = new List<Shared.Models.Task>();
            for(int i = 0; i < 10; i++)
            {
                var task = new Shared.Models.Task
                {
                    //Id = i,
                    Name = $"Task {i}",
                    Assignee = resources[i % 2],
                    OwnerProject = project
                };
                tasks.Add(task);
                await _context.Tasks.AddAsync(task);
            }
            project.Resources = resources;
            project.Tasks = tasks;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet, Publish]
        public async Task<List<Shared.Models.Task>> GetTasks(int projectId, CancellationToken cancellationToken)
            => await _taskService.GetTasks(projectId, cancellationToken);

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
