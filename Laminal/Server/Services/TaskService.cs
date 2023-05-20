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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        public virtual async Task CreateTask(CreateTaskCommand command, CancellationToken cancellationToken = default)
        {
            if(Computed.IsInvalidating())
            {
                _ = GetTasks(command.ProjectId, cancellationToken);
                return;
            }
            var context = await CreateCommandDbContext(cancellationToken);

            var newTask = new Shared.Models.Task();
            //var newTask = command.NewTask; // This causes a crash due to accessing the class
            for(int x = 0; x < 3; x++)
            {
                var tp = new TaskProperty()
                {
                    Name = $"Property {x}",
                    Value = ""
                };
                newTask.Properties.Add(tp);
                await context.TaskProperties.AddAsync(tp);
            }
            var project = await context.Projects.Include(p => p.Tasks).FirstAsync(p => p.Id == command.ProjectId);
            newTask.OwnerProject = project;
            await context.Tasks.AddAsync(newTask);
            project.Tasks.Add(newTask);
            context.Update(project);
            await context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<Shared.Models.Task> GetTask(int taskId, CancellationToken cancellationToken = default)
        {
            return await _taskResolver.Get(taskId, cancellationToken);
        }

        public virtual async Task<Shared.Models.TaskProperty> GetTaskProperty(int tpId, CancellationToken cancellationToken = default)
        {
            return await _taskPropertyResolver.Get(tpId, cancellationToken);
        }

        public virtual async Task<IList<int>> GetTasks(int projectId, CancellationToken cancellationToken = default)
        {
            var project = await _projectResolver.Get(projectId, cancellationToken);

            // HACK to work around lack of circular ref handling for now:
            foreach(var task in project.Tasks) task.OwnerProject = null;

            return await Task.FromResult(project.Tasks.Select(t => t.Id).ToList());
        }

        public virtual async Task SetTaskProperty(SetTaskPropertyCommand command, CancellationToken cancellationToken = default)
        {
            if(Computed.IsInvalidating())
            {
                _ = GetTaskProperty(command.TaskPropertyId, cancellationToken);
                return;
            }

            await using var context = await CreateCommandDbContext();
            var tp = await context.TaskProperties.FindAsync(command.TaskPropertyId);
            tp.Value = command.Value;
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
