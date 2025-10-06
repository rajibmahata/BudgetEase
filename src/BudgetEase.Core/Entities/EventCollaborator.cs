using System.ComponentModel.DataAnnotations;

namespace BudgetEase.Core.Entities;

public class EventCollaborator
{
    [Key]
    public int Id { get; set; }
    
    public int EventId { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Event Event { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}
