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
    public class TaskService : DbServiceBase<AppDbContext>, ITaskService
    {
        ILogger<BaseAPIController> _logger;
        IConfiguration _configuration;
        readonly IDbEntityResolver<int, Shared.Models.Task> _taskResolver;
        readonly IDbEntityResolver<int, Shared.Models.Project> _projectResolver;
        readonly IDbEntityResolver<int, Shared.Models.TaskProperty> _taskPropertyResolver;

        public TaskService(IServiceProvider services, ILogger<BaseAPIController> logger, IConfiguration config,
            IDbEntityResolver<int, Shared.Models.Task> taskResolver, IDbEntityResolver<int, Project> projectResolver, 
            IDbEntityResolver<int, TaskProperty> taskPropertyResolver) : base(services)
        {
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
            var dbContext = CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);

            var project = await dbContext.Projects.Include(p => p.Tasks)
                .ThenInclude(t => t.Properties)
                .FirstAsync(p => p.Id == projectId).ConfigureAwait(false);

            // HACK to work around lack of circular ref handling for now:
            foreach(var task in project.Tasks) task.OwnerProject = null;

            return await Task.FromResult(project.Tasks);
        }

        public virtual async Task SetTaskProperty(SetTaskPropertyCommand command, CancellationToken cancellationToken = default)
        {
            if(Computed.IsInvalidating())
            {
                _ = GetTaskProperty(command.TaskPropertyId, cancellationToken);
                return;
            }

            await using var context = await CreateCommandDbContext().ConfigureAwait(false);
            var tp = await context.TaskProperties.FindAsync(command.TaskPropertyId);
            tp.Value = command.Value;
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
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
