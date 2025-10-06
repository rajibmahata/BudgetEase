using BudgetEase.Core.Entities;
using BudgetEase.Core.Interfaces;
using BudgetEase.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetEase.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;

    public EventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Event?> GetByIdAsync(int id)
    {
        return await _context.Events
            .Include(e => e.Expenses)
            .Include(e => e.Vendors)
            .Include(e => e.Collaborators)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Event>> GetAllByUserIdAsync(string userId)
    {
        return await _context.Events
            .Where(e => e.OwnerId == userId || e.Collaborators.Any(c => c.UserId == userId))
            .Include(e => e.Expenses)
            .Include(e => e.Vendors)
            .OrderByDescending(e => e.EventDate)
            .ToListAsync();
    }

    public async Task<Event> CreateAsync(Event @event)
    {
        _context.Events.Add(@event);
        await _context.SaveChangesAsync();
        return @event;
    }

    public async Task<Event> UpdateAsync(Event @event)
    {
        @event.UpdatedAt = DateTime.UtcNow;
        _context.Events.Update(@event);
        await _context.SaveChangesAsync();
        return @event;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var @event = await _context.Events.FindAsync(id);
        if (@event == null) return false;

        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Events.AnyAsync(e => e.Id == id);
    }
}
