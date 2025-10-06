using BudgetEase.Core.Entities;
using BudgetEase.Core.Interfaces;
using BudgetEase.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetEase.Infrastructure.Repositories;

public class VendorRepository : IVendorRepository
{
    private readonly ApplicationDbContext _context;

    public VendorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Vendor?> GetByIdAsync(int id)
    {
        return await _context.Vendors
            .Include(v => v.Expenses)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<IEnumerable<Vendor>> GetAllByEventIdAsync(int eventId)
    {
        return await _context.Vendors
            .Where(v => v.EventId == eventId)
            .OrderBy(v => v.Name)
            .ToListAsync();
    }

    public async Task<Vendor> CreateAsync(Vendor vendor)
    {
        _context.Vendors.Add(vendor);
        await _context.SaveChangesAsync();
        return vendor;
    }

    public async Task<Vendor> UpdateAsync(Vendor vendor)
    {
        vendor.UpdatedAt = DateTime.UtcNow;
        _context.Vendors.Update(vendor);
        await _context.SaveChangesAsync();
        return vendor;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var vendor = await _context.Vendors.FindAsync(id);
        if (vendor == null) return false;

        _context.Vendors.Remove(vendor);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Vendor>> GetVendorsWithUpcomingRemindersAsync()
    {
        var today = DateTime.UtcNow.Date;
        var threeDaysFromNow = today.AddDays(3);

        return await _context.Vendors
            .Where(v => v.NextReminderDate.HasValue 
                && v.NextReminderDate.Value.Date >= today 
                && v.NextReminderDate.Value.Date <= threeDaysFromNow)
            .Include(v => v.Event)
            .OrderBy(v => v.NextReminderDate)
            .ToListAsync();
    }
}
