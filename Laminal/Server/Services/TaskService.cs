using Laminal.Server.Controllers;
using Laminal.Server.Models;
using Laminal.Shared.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using Stl.Fusion.EntityFramework;

namespace Laminal.Server.Services
{
    public class TaskService : ITaskService
    {
        private readonly DbHub<AppDbContext> _dbHub;
        ILogger<BaseAPIController> _logger;
        IConfiguration _configuration;

        public TaskService(DbHub<AppDbContext> dbHub, ILogger<BaseAPIController> logger, IConfiguration config)
        {
            _dbHub = dbHub;
            _logger = logger;
            _configuration = config;
        }

        public virtual async Task<Shared.Models.Task> GetTask(int taskId)
        {
            await using var context = _dbHub.CreateDbContext();
            var task = await context.Tasks.Include(t => t.Properties).FirstOrDefaultAsync(t => t.Id == taskId);
            return task;
        }

        public virtual async Task<Shared.Models.TaskProperty> GetTaskProperty(int taskId, string name)
        {
            await using var context = _dbHub.CreateDbContext();
            var task = await context.Tasks.Include(t => t.Properties).FirstOrDefaultAsync(t => t.Id == taskId);
            return task.Properties.FirstOrDefault(v => v.Name == name);
        }

        public virtual async Task<List<Shared.Models.Task>> GetTasks(int projectId)
        {
            await using var context = _dbHub.CreateDbContext();
            return await context.Tasks.Include(t => t.Properties).ToListAsync();
        }

        public virtual async Task SetTaskProperty(SetTaskPropertyCommand command)
        {
            //await using var context = await _dbHub.CreateCommandDbContext();
            await using var context = _dbHub.CreateDbContext(true);
            var tp = await context.TaskProperties.FirstOrDefaultAsync(t => t.Id == command.TaskPropertyId);
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
