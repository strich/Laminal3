using Laminal.Server.Controllers;
using Laminal.Server.Models;
using Laminal.Shared.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Stl.Fusion.EntityFramework;
using System.Threading;
using System.Threading.Tasks;
using Laminal.Shared.Models;
using Task = System.Threading.Tasks.Task;
using Stl.Fusion;

namespace Laminal.Server.Services
{
    public class TaskService : ITaskService
    {
        private readonly DbHub<AppDbContext> _dbHub;
        ILogger<BaseAPIController> _logger;
        IConfiguration _configuration;
        readonly IDbEntityResolver<int, Shared.Models.Task> _taskResolver;
        readonly IDbEntityResolver<int, Shared.Models.Project> _projectResolver;
        readonly IDbEntityResolver<int, Shared.Models.TaskProperty> _taskPropertyResolver;

        public TaskService(DbHub<AppDbContext> dbHub, ILogger<BaseAPIController> logger, IConfiguration config,
            IDbEntityResolver<int, Shared.Models.Task> taskResolver, IDbEntityResolver<int, Project> projectResolver, 
            IDbEntityResolver<int, TaskProperty> taskPropertyResolver)
        {
            _dbHub = dbHub;
            _logger = logger;
            _configuration = config;
            _taskResolver = taskResolver;
            _projectResolver = projectResolver;
            _taskPropertyResolver = taskPropertyResolver;
        }

        public virtual async Task<Shared.Models.Task> GetTask(int taskId, CancellationToken cancellationToken = default)
        {
            var task = await _taskResolver.Get(taskId, cancellationToken);
            //await using var context = _dbHub.CreateDbContext();
            //var task = await context.Tasks.Include(t => t.Properties).FirstOrDefaultAsync(t => t.Id == taskId);
            return task;
        }

        public virtual async Task<Shared.Models.TaskProperty> GetTaskProperty(int tpId, CancellationToken cancellationToken = default)
        {
            //await using var context = _dbHub.CreateDbContext();
            //var task = await context.Tasks.Include(t => t.Properties).FirstOrDefaultAsync(t => t.Id == taskId);
            //return task.Properties.FirstOrDefault(v => v.Name == name);
            return await _taskPropertyResolver.Get(tpId, cancellationToken);
        }

        public virtual async Task<IList<Shared.Models.Task>> GetTasks(int projectId, CancellationToken cancellationToken = default)
        {
            //return new List<Shared.Models.Task>() { new Shared.Models.Task() { Name = "Test" } };
            var project = await _projectResolver.Get(projectId, cancellationToken);
            if(project == null) return new List<Shared.Models.Task>();

            // HACK to work around lack of circular ref handling for now:
            foreach(var task in project.Tasks) task.OwnerProject = null;

            return await Task.FromResult(project.Tasks);
            //await using var context = _dbHub.CreateDbContext();
            //return await context.Tasks.Include(t => t.Properties).ToListAsync();
        }

        public virtual async Task SetTaskProperty(SetTaskPropertyCommand command, CancellationToken cancellationToken = default)
        {
            //await using var context = await _dbHub.CreateCommandDbContext();
            //await using var context = _dbHub.CreateDbContext(true);
            //var tp = await context.TaskProperties.FirstOrDefaultAsync(t => t.Id == command.TaskPropertyId);
            //tp.Value = command.Value;
            //await context.SaveChangesAsync();

            if(Computed.IsInvalidating())
            {
                _ = GetTaskProperty(command.TaskPropertyId, cancellationToken);
                return;
            }

            await using var context = await _dbHub.CreateCommandDbContext();
            var tp = await context.TaskProperties.FindAsync(command.TaskPropertyId);
            tp.Value = command.Value;
            await context.SaveChangesAsync();
        }

        //public async Task PatchTask(int id, JsonPatchDocument<Shared.Models.Task> patchDoc)
        //{
        //    var task = await _context.Tasks.FindAsync(id);
        //    if(task == null)
        //    {
        //        return;
        //        //return new NotFoundResult();
        //    }

        //    patchDoc.ApplyTo(task);
        //    await _context.SaveChangesAsync();

        //    return;
        //    //return new NoContentResult();
        //}
    }
}
