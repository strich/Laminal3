using Laminal.Server.Models;
using Laminal.Shared.Models;
using Laminal.Shared.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stl.Fusion;
using Stl.Fusion.Server;
using System.Security.Cryptography;
using Stl.Fusion.EntityFramework;
using Task = System.Threading.Tasks.Task;

namespace Laminal.Server.Controllers
{
    public class TasksController : BaseAPIController, ITaskService
    {
        private readonly DbHub<AppDbContext> _dbHub;
        readonly ITaskService _taskService;

        public TasksController(DbHub<AppDbContext> dbHub, ILogger<TasksController> logger, IConfiguration config, ITaskService taskService)
            : base(logger, config)
        {
            _logger = logger;
            _dbHub = dbHub;
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> InitializeDb()
        {
            //await using var context = await _dbHub.CreateCommandDbContext(HttpContext.RequestAborted);
            await using var context = _dbHub.CreateDbContext(true);
            
            var project = new Project { /*Id = 1,*/ Name = "Project 1" };
            await context.Projects.AddAsync(project);

            var resources = new List<Resource>();
            for(int i = 0; i < 2; i++)
            {
                var rsc = new Resource { /*Id = i,*/ Name = $"Resource {i}", OwnerProject = project };
                resources.Add(rsc);
                await context.Resources.AddAsync(rsc);
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
                await context.Tasks.AddAsync(task);

                for(int x = 0; x < 3; x++)
                {
                    var tp = new TaskProperty()
                    {
                        Name = $"Property {x}",
                        Value = ""
                    };
                    task.Properties.Add(tp);
                    await context.TaskProperties.AddAsync(tp);
                }
            }
            project.Resources = resources;
            project.Tasks = tasks;

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet, Publish]
        public async Task<IList<int>> GetTasks(int projectId, CancellationToken cancellationToken = default)
            => await _taskService.GetTasks(projectId, cancellationToken);

        [HttpGet, Publish]
        public async Task<Shared.Models.Task> GetTask(int taskId, CancellationToken cancellationToken = default)
        {
            return await _taskService.GetTask(taskId, cancellationToken);
        }

        [HttpGet, Publish]
        public async Task<Shared.Models.TaskProperty> GetTaskProperty(int tpId, CancellationToken cancellationToken = default)
        {
            return await _taskService.GetTaskProperty(tpId, cancellationToken);
        }

        //[HttpPatch("{id}")]
        //public async Task PatchTask(int id, [FromBody] JsonPatchDocument<Shared.Models.Task> patchDoc)
        //{
        //    await _taskService.PatchTask(id, patchDoc);
        //    //return Ok(patchDoc);
        //}

        [HttpPost, Publish]
        public async Task SetTaskProperty([FromBody] SetTaskPropertyCommand command, CancellationToken cancellationToken = default)
        {
            await _taskService.SetTaskProperty(command, cancellationToken);
        }
    }
}
