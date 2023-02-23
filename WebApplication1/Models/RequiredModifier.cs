using System.Diagnostics.CodeAnalysis;

namespace API.Models
{
    public class RequiredModifier
    {
        public required string Name { get; set; }
        public string? MiddleName { get; set; }
        public required string LastName { get; set; }

        public RequiredModifier()
        {
        }

        [SetsRequiredMembers]
        public RequiredModifier(string name, string lastName)
        {
            Name = name;
            LastName = lastName;
        }
    }
}
