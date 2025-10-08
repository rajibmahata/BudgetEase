using BudgetEase.Core.Entities;
using BudgetEase.Core.Interfaces;
using BudgetEase.Infrastructure.Data;
using BudgetEase.Infrastructure.Repositories;
using BudgetEase.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static BudgetEase.Infrastructure.Data.DatabaseSeeder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configure SQLite database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Register repositories
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();

// Register database backup service
builder.Services.AddSingleton<IDatabaseBackupService, DatabaseBackupService>();
builder.Services.AddHostedService<DatabaseBackupBackgroundService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins("https://localhost:7001", "http://localhost:5001")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazor");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Restore database from backup if needed and create database on startup
using (var scope = app.Services.CreateScope())
{
    var backupService = scope.ServiceProvider.GetRequiredService<IDatabaseBackupService>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    // Try to restore from latest backup if database doesn't exist
    var restored = await backupService.RestoreLatestBackupAsync();
    if (restored)
    {
        logger.LogInformation("Database restored from backup successfully");
    }
    
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    
    // Seed the database with sample data
    await DatabaseSeeder.SeedAsync(app.Services);
}

app.Run();
