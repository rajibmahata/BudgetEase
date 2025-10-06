using BudgetEase.Core.Entities;

namespace BudgetEase.Core.Interfaces;

public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(int id);
    Task<IEnumerable<Expense>> GetAllByEventIdAsync(int eventId);
    Task<Expense> CreateAsync(Expense expense);
    Task<Expense> UpdateAsync(Expense expense);
    Task<bool> DeleteAsync(int id);
    Task<decimal> GetTotalSpentByEventIdAsync(int eventId);
}
