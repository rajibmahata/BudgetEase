using BudgetEase.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetEase.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Event> Events => Set<Event>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<EventCollaborator> EventCollaborators => Set<EventCollaborator>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Event configuration
        builder.Entity<Event>()
            .HasOne(e => e.Owner)
            .WithMany(u => u.Events)
            .HasForeignKey(e => e.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Expense configuration
        builder.Entity<Expense>()
            .HasOne(e => e.Event)
            .WithMany(ev => ev.Expenses)
            .HasForeignKey(e => e.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Expense>()
            .HasOne(e => e.Vendor)
            .WithMany(v => v.Expenses)
            .HasForeignKey(e => e.VendorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Expense>()
            .Property(e => e.EstimatedCost)
            .HasPrecision(18, 2);

        builder.Entity<Expense>()
            .Property(e => e.ActualCost)
            .HasPrecision(18, 2);

        // Vendor configuration
        builder.Entity<Vendor>()
            .HasOne(v => v.Event)
            .WithMany(e => e.Vendors)
            .HasForeignKey(v => v.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // EventCollaborator configuration
        builder.Entity<EventCollaborator>()
            .HasOne(ec => ec.Event)
            .WithMany(e => e.Collaborators)
            .HasForeignKey(ec => ec.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<EventCollaborator>()
            .HasOne(ec => ec.User)
            .WithMany(u => u.Collaborations)
            .HasForeignKey(ec => ec.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Budget configuration
        builder.Entity<Event>()
            .Property(e => e.BudgetLimit)
            .HasPrecision(18, 2);
    }
}
