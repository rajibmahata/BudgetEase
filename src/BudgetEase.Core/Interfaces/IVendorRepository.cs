using BudgetEase.Core.Entities;

namespace BudgetEase.Core.Interfaces;

public interface IVendorRepository
{
    Task<Vendor?> GetByIdAsync(int id);
    Task<IEnumerable<Vendor>> GetAllByEventIdAsync(int eventId);
    Task<Vendor> CreateAsync(Vendor vendor);
    Task<Vendor> UpdateAsync(Vendor vendor);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Vendor>> GetVendorsWithUpcomingRemindersAsync();
}
