namespace BudgetEase.Core.DTOs;

public class ExpenseDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? VendorId { get; set; }
    public string? VendorName { get; set; }
    public decimal EstimatedCost { get; set; }
    public decimal ActualCost { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
    public DateTime? PaymentDueDate { get; set; }
}

public class CreateExpenseDto
{
    public int EventId { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? VendorId { get; set; }
    public decimal EstimatedCost { get; set; }
    public decimal ActualCost { get; set; }
    public string PaymentStatus { get; set; } = "Pending";
    public DateTime? PaymentDueDate { get; set; }
}

public class UpdateExpenseDto
{
    public string? Category { get; set; }
    public string? Description { get; set; }
    public int? VendorId { get; set; }
    public decimal? EstimatedCost { get; set; }
    public decimal? ActualCost { get; set; }
    public string? PaymentStatus { get; set; }
    public DateTime? PaymentDueDate { get; set; }
}
