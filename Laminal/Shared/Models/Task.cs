using System.Diagnostics.CodeAnalysis;

namespace Laminal.Shared.Models
{
    public class TaskProperty
    {
        public int Id { get; set; }
        public string Name { get; set; } // TODO put this somewhere else to save memory and serialization time
        public string Value { get; set; }
    }

    public class Task
    {
        public int Id { get; set; }
        public List<TaskProperty> Properties { get; set; } = new();
        public string Name { get; set; } = "";
        public TaskStatus Status { get; set; }
        public TaskType TaskType { get; set; }
        //public int WorkHrs { get; set; }
        //public int WorkHrs
        //{
        //    get => (int)Properties.FirstOrDefault(v => v.Name == nameof(WorkHrs)).Value;
        //    set => Properties[nameof(WorkHrs)] = new TaskProperty { Name = nameof(WorkHrs), Value = value };
        //}

        //public int CommittedWorkHrs { get; set; }

        /// <summary>
        /// The iteration this task is on. It increments each time this task is restarted.
        /// </summary>
        //public int Iteration { get; set; }

        /// <summary>
        /// Tasks that this task depends on to start.
        /// </summary>
        public IList<Task> PredecessorTasks { get; set; } = new List<Task>();
        /// <summary>
        /// Tasks that depend on this task to finish.
        /// </summary>
        public IList<Task> SuccessorTasks { get; set; } = new List<Task>();

        [DisallowNull]
        public Project OwnerProject { get; set; }
        public Resource? Assignee { get; set; }

        /// <summary>
        /// The parent group task if existing.
        /// </summary>
        //[AllowNull]
        //public Task GroupParent { get; set; }
        /// <summary>
        /// If this task is a group task, it will have children.
        /// </summary>
        //public IList<Task> GroupChildren { get; set; } = new List<Task>();

        /// <summary>
        /// This is the template this task was based from.
        /// </summary>
        //public Task BaseTemplateTask { get; set; }
    }

    public enum TaskStatus
    {
        Todo,
        InProgress,
        Finished
    }

    public enum TaskType
    {
        Task,
        GroupTask,
        Template
    }
}
