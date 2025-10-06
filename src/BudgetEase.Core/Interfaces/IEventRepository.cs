using BudgetEase.Core.Entities;

namespace BudgetEase.Core.Interfaces;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(int id);
    Task<IEnumerable<Event>> GetAllByUserIdAsync(string userId);
    Task<Event> CreateAsync(Event @event);
    Task<Event> UpdateAsync(Event @event);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
