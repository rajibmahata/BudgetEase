using BudgetEase.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BudgetEase.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            // Check if data already exists
            if (context.Users.Any())
            {
                logger.LogInformation("Database already contains data. Skipping seed.");
                return;
            }

            logger.LogInformation("Seeding database with sample data...");

            // Create demo users
            var demoUser1 = new ApplicationUser
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "demo@budgetease.com",
                UserName = "demo@budgetease.com",
                EmailConfirmed = true
            };

            var demoUser2 = new ApplicationUser
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                UserName = "jane.smith@example.com",
                EmailConfirmed = true
            };

            var demoUser3 = new ApplicationUser
            {
                FirstName = "Michael",
                LastName = "Johnson",
                Email = "michael.johnson@example.com",
                UserName = "michael.johnson@example.com",
                EmailConfirmed = true
            };

            // Create users with password: Demo@123
            await userManager.CreateAsync(demoUser1, "Demo@123");
            await userManager.CreateAsync(demoUser2, "Demo@123");
            await userManager.CreateAsync(demoUser3, "Demo@123");

            // Retrieve users to get their IDs
            demoUser1 = await userManager.FindByEmailAsync(demoUser1.Email!);
            demoUser2 = await userManager.FindByEmailAsync(demoUser2.Email!);
            demoUser3 = await userManager.FindByEmailAsync(demoUser3.Email!);

            // Create events
            var weddingEvent = new Event
            {
                Name = "Sarah & Tom's Wedding",
                Type = "Wedding",
                EventDate = DateTime.UtcNow.AddDays(60),
                Venue = "Grand Ballroom Hotel, Downtown",
                BudgetLimit = 50000.00m,
                OwnerId = demoUser1!.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-30)
            };

            var birthdayEvent = new Event
            {
                Name = "Emma's 30th Birthday Bash",
                Type = "Birthday",
                EventDate = DateTime.UtcNow.AddDays(20),
                Venue = "Sunset Garden Restaurant",
                BudgetLimit = 8000.00m,
                OwnerId = demoUser1!.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-15)
            };

            var anniversaryEvent = new Event
            {
                Name = "25th Wedding Anniversary",
                Type = "Anniversary",
                EventDate = DateTime.UtcNow.AddDays(90),
                Venue = "Lakeside Country Club",
                BudgetLimit = 15000.00m,
                OwnerId = demoUser2!.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-20)
            };

            var corporateEvent = new Event
            {
                Name = "Annual Company Gala",
                Type = "Corporate",
                EventDate = DateTime.UtcNow.AddDays(45),
                Venue = "Convention Center",
                BudgetLimit = 75000.00m,
                OwnerId = demoUser3!.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-25)
            };

            context.Events.AddRange(weddingEvent, birthdayEvent, anniversaryEvent, corporateEvent);
            await context.SaveChangesAsync();

            // Add vendors for wedding event
            var weddingVendors = new List<Vendor>
            {
                new Vendor
                {
                    EventId = weddingEvent.Id,
                    Name = "Grand Ballroom Hotel",
                    ServiceType = "Venue",
                    ContactNumber = "+1-555-0101",
                    Email = "events@grandballroom.com",
                    PaymentTerms = "50% deposit, 50% on event day",
                    NextReminderDate = DateTime.UtcNow.AddDays(30),
                    CreatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new Vendor
                {
                    EventId = weddingEvent.Id,
                    Name = "Divine Delights Catering",
                    ServiceType = "Catering",
                    ContactNumber = "+1-555-0202",
                    Email = "info@divinedelights.com",
                    PaymentTerms = "30% deposit, balance 1 week before",
                    NextReminderDate = DateTime.UtcNow.AddDays(25),
                    CreatedAt = DateTime.UtcNow.AddDays(-22)
                },
                new Vendor
                {
                    EventId = weddingEvent.Id,
                    Name = "Perfect Moments Photography",
                    ServiceType = "Photography",
                    ContactNumber = "+1-555-0303",
                    Email = "bookings@perfectmoments.com",
                    PaymentTerms = "25% booking fee, 75% on delivery",
                    NextReminderDate = DateTime.UtcNow.AddDays(15),
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new Vendor
                {
                    EventId = weddingEvent.Id,
                    Name = "Elegant Blooms Florist",
                    ServiceType = "Decoration",
                    ContactNumber = "+1-555-0404",
                    Email = "orders@elegantblooms.com",
                    PaymentTerms = "Full payment 3 days before event",
                    NextReminderDate = DateTime.UtcNow.AddDays(40),
                    CreatedAt = DateTime.UtcNow.AddDays(-18)
                },
                new Vendor
                {
                    EventId = weddingEvent.Id,
                    Name = "Melody Makers DJ Services",
                    ServiceType = "Entertainment",
                    ContactNumber = "+1-555-0505",
                    Email = "bookings@melodymakers.com",
                    PaymentTerms = "20% booking fee, 80% on event day",
                    NextReminderDate = DateTime.UtcNow.AddDays(35),
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
                }
            };

            // Add vendors for birthday event
            var birthdayVendors = new List<Vendor>
            {
                new Vendor
                {
                    EventId = birthdayEvent.Id,
                    Name = "Sunset Garden Restaurant",
                    ServiceType = "Venue & Catering",
                    ContactNumber = "+1-555-0606",
                    Email = "events@sunsetgarden.com",
                    PaymentTerms = "50% deposit, 50% on event day",
                    NextReminderDate = DateTime.UtcNow.AddDays(10),
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new Vendor
                {
                    EventId = birthdayEvent.Id,
                    Name = "Party Time Entertainers",
                    ServiceType = "Entertainment",
                    ContactNumber = "+1-555-0707",
                    Email = "info@partytime.com",
                    PaymentTerms = "Full payment 1 week before",
                    NextReminderDate = DateTime.UtcNow.AddDays(8),
                    CreatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new Vendor
                {
                    EventId = birthdayEvent.Id,
                    Name = "Sweet Dreams Bakery",
                    ServiceType = "Cake & Desserts",
                    ContactNumber = "+1-555-0808",
                    Email = "orders@sweetdreams.com",
                    PaymentTerms = "Full payment on pickup",
                    NextReminderDate = DateTime.UtcNow.AddDays(15),
                    CreatedAt = DateTime.UtcNow.AddDays(-7)
                }
            };

            // Add vendors for anniversary event
            var anniversaryVendors = new List<Vendor>
            {
                new Vendor
                {
                    EventId = anniversaryEvent.Id,
                    Name = "Lakeside Country Club",
                    ServiceType = "Venue",
                    ContactNumber = "+1-555-0909",
                    Email = "reservations@lakesidecc.com",
                    PaymentTerms = "Full payment 2 weeks before",
                    NextReminderDate = DateTime.UtcNow.AddDays(60),
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new Vendor
                {
                    EventId = anniversaryEvent.Id,
                    Name = "Gourmet Catering Co",
                    ServiceType = "Catering",
                    ContactNumber = "+1-555-1010",
                    Email = "catering@gourmet.com",
                    PaymentTerms = "50% deposit, balance 3 days before",
                    NextReminderDate = DateTime.UtcNow.AddDays(55),
                    CreatedAt = DateTime.UtcNow.AddDays(-12)
                }
            };

            context.Vendors.AddRange(weddingVendors);
            context.Vendors.AddRange(birthdayVendors);
            context.Vendors.AddRange(anniversaryVendors);
            await context.SaveChangesAsync();

            // Add expenses for wedding event
            var weddingExpenses = new List<Expense>
            {
                new Expense
                {
                    EventId = weddingEvent.Id,
                    VendorId = weddingVendors[0].Id, // Venue
                    Category = "Venue",
                    Description = "Grand Ballroom rental for wedding ceremony and reception",
                    EstimatedCost = 15000.00m,
                    ActualCost = 15000.00m,
                    PaymentStatus = "Paid",
                    PaymentDueDate = DateTime.UtcNow.AddDays(-5),
                    CreatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new Expense
                {
                    EventId = weddingEvent.Id,
                    VendorId = weddingVendors[1].Id, // Catering
                    Category = "Food & Beverage",
                    Description = "Full course dinner for 150 guests",
                    EstimatedCost = 12000.00m,
                    ActualCost = 0.00m,
                    PaymentStatus = "Partial",
                    PaymentDueDate = DateTime.UtcNow.AddDays(50),
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new Expense
                {
                    EventId = weddingEvent.Id,
                    VendorId = weddingVendors[2].Id, // Photography
                    Category = "Photography",
                    Description = "Full day wedding photography and videography",
                    EstimatedCost = 5000.00m,
                    ActualCost = 0.00m,
                    PaymentStatus = "Pending",
                    PaymentDueDate = DateTime.UtcNow.AddDays(60),
                    CreatedAt = DateTime.UtcNow.AddDays(-18)
                },
                new Expense
                {
                    EventId = weddingEvent.Id,
                    VendorId = weddingVendors[3].Id, // Decoration
                    Category = "Decoration",
                    Description = "Floral arrangements and table decorations",
                    EstimatedCost = 4500.00m,
                    ActualCost = 0.00m,
                    PaymentStatus = "Pending",
                    PaymentDueDate = DateTime.UtcNow.AddDays(57),
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new Expense
                {
                    EventId = weddingEvent.Id,
                    VendorId = weddingVendors[4].Id, // Entertainment
                    Category = "Entertainment",
                    Description = "DJ services for 6 hours",
                    EstimatedCost = 2500.00m,
                    ActualCost = 0.00m,
                    PaymentStatus = "Pending",
                    PaymentDueDate = DateTime.UtcNow.AddDays(60),
                    CreatedAt = DateTime.UtcNow.AddDays(-12)
                },
                new Expense
                {
                    EventId = weddingEvent.Id,
                    VendorId = null,
                    Category = "Attire",
                    Description = "Wedding dress and suit alterations",
                    EstimatedCost = 2000.00m,
                    ActualCost = 1850.00m,
                    PaymentStatus = "Paid",
                    PaymentDueDate = DateTime.UtcNow.AddDays(-10),
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new Expense
                {
                    EventId = weddingEvent.Id,
                    VendorId = null,
                    Category = "Invitations",
                    Description = "Custom printed wedding invitations for 150 guests",
                    EstimatedCost = 800.00m,
                    ActualCost = 750.00m,
                    PaymentStatus = "Paid",
                    PaymentDueDate = DateTime.UtcNow.AddDays(-20),
                    CreatedAt = DateTime.UtcNow.AddDays(-22)
                }
            };

            // Add expenses for birthday event
            var birthdayExpenses = new List<Expense>
            {
                new Expense
                {
                    EventId = birthdayEvent.Id,
                    VendorId = birthdayVendors[0].Id,
                    Category = "Venue & Catering",
                    Description = "Restaurant venue rental and dinner buffet for 40 guests",
                    EstimatedCost = 4500.00m,
                    ActualCost = 0.00m,
                    PaymentStatus = "Partial",
                    PaymentDueDate = DateTime.UtcNow.AddDays(18),
                    CreatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new Expense
                {
                    EventId = birthdayEvent.Id,
                    VendorId = birthdayVendors[1].Id,
                    Category = "Entertainment",
                    Description = "Live band and DJ for 4 hours",
                    EstimatedCost = 1800.00m,
                    ActualCost = 0.00m,
                    PaymentStatus = "Pending",
                    PaymentDueDate = DateTime.UtcNow.AddDays(13),
                    CreatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new Expense
                {
                    EventId = birthdayEvent.Id,
                    VendorId = birthdayVendors[2].Id,
                    Category = "Cake & Desserts",
                    Description = "Custom birthday cake and dessert table",
                    EstimatedCost = 600.00m,
                    ActualCost = 0.00m,
                    PaymentStatus = "Pending",
                    PaymentDueDate = DateTime.UtcNow.AddDays(19),
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Expense
                {
                    EventId = birthdayEvent.Id,
                    VendorId = null,
                    Category = "Decoration",
                    Description = "Balloons, banners, and party decorations",
                    EstimatedCost = 400.00m,
                    ActualCost = 380.00m,
                    PaymentStatus = "Paid",
                    PaymentDueDate = DateTime.UtcNow.AddDays(-3),
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                }
            };

            // Add expenses for anniversary event
            var anniversaryExpenses = new List<Expense>
            {
                new Expense
                {
                    EventId = anniversaryEvent.Id,
                    VendorId = anniversaryVendors[0].Id,
                    Category = "Venue",
                    Description = "Country club venue rental for 80 guests",
                    EstimatedCost = 6000.00m,
                    ActualCost = 0.00m,
                    PaymentStatus = "Pending",
                    PaymentDueDate = DateTime.UtcNow.AddDays(76),
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new Expense
                {
                    EventId = anniversaryEvent.Id,
                    VendorId = anniversaryVendors[1].Id,
                    Category = "Food & Beverage",
                    Description = "Gourmet dinner and cocktail hour",
                    EstimatedCost = 7500.00m,
                    ActualCost = 0.00m,
                    PaymentStatus = "Pending",
                    PaymentDueDate = DateTime.UtcNow.AddDays(87),
                    CreatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new Expense
                {
                    EventId = anniversaryEvent.Id,
                    VendorId = null,
                    Category = "Photography",
                    Description = "Professional photographer for 4 hours",
                    EstimatedCost = 1500.00m,
                    ActualCost = 0.00m,
                    PaymentStatus = "Pending",
                    PaymentDueDate = DateTime.UtcNow.AddDays(90),
                    CreatedAt = DateTime.UtcNow.AddDays(-6)
                }
            };

            context.Expenses.AddRange(weddingExpenses);
            context.Expenses.AddRange(birthdayExpenses);
            context.Expenses.AddRange(anniversaryExpenses);
            await context.SaveChangesAsync();

            // Add collaborators
            var collaborators = new List<EventCollaborator>
            {
                new EventCollaborator
                {
                    EventId = weddingEvent.Id,
                    UserId = demoUser2!.Id,
                    AddedAt = DateTime.UtcNow.AddDays(-28)
                },
                new EventCollaborator
                {
                    EventId = corporateEvent.Id,
                    UserId = demoUser1!.Id,
                    AddedAt = DateTime.UtcNow.AddDays(-20)
                }
            };

            context.EventCollaborators.AddRange(collaborators);
            await context.SaveChangesAsync();

            logger.LogInformation("Database seeded successfully with sample data!");
            logger.LogInformation("Demo user credentials: demo@budgetease.com / Demo@123");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
