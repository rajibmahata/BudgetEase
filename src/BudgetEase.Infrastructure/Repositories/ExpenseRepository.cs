using BudgetEase.Core.Entities;
using BudgetEase.Core.Interfaces;
using BudgetEase.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetEase.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ApplicationDbContext _context;

    public ExpenseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Expense?> GetByIdAsync(int id)
    {
        return await _context.Expenses
            .Include(e => e.Vendor)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Expense>> GetAllByEventIdAsync(int eventId)
    {
        return await _context.Expenses
            .Where(e => e.EventId == eventId)
            .Include(e => e.Vendor)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }

    public async Task<Expense> CreateAsync(Expense expense)
    {
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
        return expense;
    }

    public async Task<Expense> UpdateAsync(Expense expense)
    {
        expense.UpdatedAt = DateTime.UtcNow;
        _context.Expenses.Update(expense);
        await _context.SaveChangesAsync();
        return expense;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null) return false;

        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<decimal> GetTotalSpentByEventIdAsync(int eventId)
    {
        return await _context.Expenses
            .Where(e => e.EventId == eventId)
            .SumAsync(e => e.ActualCost);
    }
}
