using System.ComponentModel.DataAnnotations;

namespace BudgetEase.Core.Entities;

public class Vendor
{
    [Key]
    public int Id { get; set; }
    
    public int EventId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string ServiceType { get; set; } = string.Empty; // Caterer, Decorator, DJ, Photographer, etc.
    
    [MaxLength(20)]
    public string? ContactNumber { get; set; }
    
    [MaxLength(200)]
    public string? Email { get; set; }
    
    [MaxLength(1000)]
    public string? PaymentTerms { get; set; }
    
    public DateTime? NextReminderDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public Event Event { get; set; } = null!;
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
