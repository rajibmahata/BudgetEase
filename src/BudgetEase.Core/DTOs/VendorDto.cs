namespace BudgetEase.Core.DTOs;

public class VendorDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? PaymentTerms { get; set; }
    public DateTime? NextReminderDate { get; set; }
}

public class CreateVendorDto
{
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? PaymentTerms { get; set; }
    public DateTime? NextReminderDate { get; set; }
}

public class UpdateVendorDto
{
    public string? Name { get; set; }
    public string? ServiceType { get; set; }
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? PaymentTerms { get; set; }
    public DateTime? NextReminderDate { get; set; }
}
