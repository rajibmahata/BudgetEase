using System.ComponentModel.DataAnnotations;

namespace BudgetEase.Core.Entities;

public class Event
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty; // Marriage, Birthday, Anniversary, Custom
    
    public DateTime EventDate { get; set; }
    
    [MaxLength(500)]
    public string? Venue { get; set; }
    
    public decimal BudgetLimit { get; set; }
    
    [Required]
    public string OwnerId { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ApplicationUser Owner { get; set; } = null!;
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();
    public ICollection<EventCollaborator> Collaborators { get; set; } = new List<EventCollaborator>();
}
