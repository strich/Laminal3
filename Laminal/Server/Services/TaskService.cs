using Laminal.Server.Controllers;
using Laminal.Server.Models;
using Laminal.Shared.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Laminal.Server.Services
{
    public class TaskService : ITaskService
    {
        AppDbContext _context;
        ILogger<BaseAPIController> _logger;
        IConfiguration _configuration;

        public TaskService(AppDbContext context, ILogger<BaseAPIController> logger, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _configuration = config;
        }

        public virtual async Task<Shared.Models.Task> GetTask(int taskId)
        {
            var task = await _context.Tasks.Include(t => t.Properties).FirstOrDefaultAsync(t => t.Id == taskId);
            return task;
        }

        public virtual async Task<Shared.Models.TaskProperty> GetTaskProperty(int taskId, string name)
        {
            var task = await _context.Tasks.Include(t => t.Properties).FirstOrDefaultAsync(t => t.Id == taskId);
            return task.Properties.FirstOrDefault(v => v.Name == name);
        }

        public virtual async Task<List<Shared.Models.Task>> GetTasks(int projectId)
        {
            return await _context.Tasks.Include(t => t.Properties).ToListAsync();
        }

        public virtual async ValueTask SetTaskProperty(int taskId, Shared.Models.TaskProperty property)
        {
            var task = await _context.Tasks.Include(t => t.Properties).FirstOrDefaultAsync(t => t.Id == taskId);
            task.Properties.FirstOrDefault(v => v.Name == property.Name).Value = property;
            await _context.SaveChangesAsync();
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
