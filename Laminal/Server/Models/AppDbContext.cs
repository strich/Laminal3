using Laminal.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Laminal.Server.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Shared.Models.Task> Tasks { get; set; }
        public DbSet<Shared.Models.TaskProperty> TaskProperties { get; set; }
        public DbSet<TemplateFeature> TemplateTasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Resource> Resources { get; set; }
        //public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shared.Models.Task>().HasMany<TaskProperty>();

            var project = new Project { Id = 1, Name = "Project 1" };

            var resources = new List<Resource>();
            for(int i = 0; i < 2; i++)
            {
                resources.Add(new Resource { Id = i, Name = $"Resource {i}" });
            }

            var tasks = new List<Shared.Models.Task>();
            for(int i = 0; i < 10; i++)
            {
                tasks.Add(new Shared.Models.Task
                {
                    Id = i,
                    Name = $"Task {i}",
                    Assignee = resources[i % 2]
                });
            }
            project.Resources = resources;
            project.Tasks = tasks;

            modelBuilder.Entity<Project>().HasData(project);
            modelBuilder.Entity<Resource>().HasData(resources);
            modelBuilder.Entity<Shared.Models.Task>().HasData(tasks);
        }
    }
}
