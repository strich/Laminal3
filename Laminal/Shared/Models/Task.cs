using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Laminal.Shared.Models
{
    public class TaskProperty
    {
        public int Id { get; set; }
        public string Name { get; set; } // TODO put this somewhere else to save memory and serialization time?
        public string Value { get; set; }

        public T GetValue<T>() => (T)Convert.ChangeType(Value, typeof(T));
        public void SetValue<T>(T value) => Value = (string)Convert.ChangeType(value, typeof(string));
    }

    public class Task
    {
        public int Id { get; set; }
        public List<TaskProperty> Properties { get; set; } = new();
        public string Name { get; set; } = "";
        public TaskStatus Status { get; set; }
        public TaskType TaskType { get; set; }
        //public int WorkHrs { get; set; }
        [NotMapped]
        public int WorkHrs
        {
            get => GetProperty<int>(nameof(WorkHrs), out var val) ? val : default;
            set => SetProperty(nameof(WorkHrs), value);
        }

        bool GetProperty<T>(string name, out T propertyValue)
        {
            for(int i = 0; i < Properties.Count; i++)
            {
                if(string.Equals(Properties[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    propertyValue = Properties[i].GetValue<T>();
                    return true;
                }
            }
            propertyValue = default;
            return false;
        }

        bool SetProperty<T>(string name, T propertyValue)
        {
            for(int i = 0; i < Properties.Count; i++)
            {
                if(string.Equals(Properties[i].Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    Properties[i].SetValue(propertyValue);
                    return true;
                }
            }
            return false;
        }

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
