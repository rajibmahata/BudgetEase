namespace BudgetEase.Core.DTOs;

public class EventDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string? Venue { get; set; }
    public decimal BudgetLimit { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal RemainingBudget { get; set; }
}

public class CreateEventDto
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string? Venue { get; set; }
    public decimal BudgetLimit { get; set; }
}

public class UpdateEventDto
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public DateTime? EventDate { get; set; }
    public string? Venue { get; set; }
    public decimal? BudgetLimit { get; set; }
}
