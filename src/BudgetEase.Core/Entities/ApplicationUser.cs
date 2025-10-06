using Microsoft.AspNetCore.Identity;

namespace BudgetEase.Core.Entities;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<EventCollaborator> Collaborations { get; set; } = new List<EventCollaborator>();
}
