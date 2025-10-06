using System.ComponentModel.DataAnnotations;

namespace BudgetEase.Core.Entities;

public class Expense
{
    [Key]
    public int Id { get; set; }
    
    public int EventId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Category { get; set; } = string.Empty; // Food, Decoration, Gift, Photography, etc.
    
    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    public int? VendorId { get; set; }
    
    public decimal EstimatedCost { get; set; }
    
    public decimal ActualCost { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string PaymentStatus { get; set; } = "Pending"; // Paid, Pending, Partial
    
    public DateTime? PaymentDueDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public Event Event { get; set; } = null!;
    public Vendor? Vendor { get; set; }
}
