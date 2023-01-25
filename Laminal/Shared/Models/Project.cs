namespace Laminal.Shared.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public IList<Task> Tasks { get; set; } = new List<Task>();
        public IList<Resource> Resources { get; set; } = new List<Resource>();
    }
}
