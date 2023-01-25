using System.Diagnostics.CodeAnalysis;

namespace Laminal.Shared.Models
{
    public class Resource
    {
        public int Id { get; set; }
        [DisallowNull]
        public string Name { get; set; }
        [DisallowNull]
        public Project OwnerProject { get; set; }
    }
}
