# BudgetEase Setup Guide

This guide will help you set up and run the BudgetEase application on your local machine.

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Installation Steps](#installation-steps)
3. [Running the Application](#running-the-application)
4. [Database Management](#database-management)
5. [Configuration](#configuration)
6. [Troubleshooting](#troubleshooting)

## Prerequisites

### Required Software

1. **.NET 9.0 SDK or later**
   - Download from: https://dotnet.microsoft.com/download/dotnet/9.0
   - Verify installation: `dotnet --version`

2. **IDE or Code Editor** (Choose one)
   - Visual Studio 2022 (Community/Professional/Enterprise)
     - Download from: https://visualstudio.microsoft.com/
     - Workload: "ASP.NET and web development"
   - Visual Studio Code
     - Download from: https://code.visualstudio.com/
     - Extensions: C# Dev Kit, C# Extensions

3. **Git**
   - Download from: https://git-scm.com/
   - Verify installation: `git --version`

### Optional Tools

- **SQL Browser** (for viewing SQLite database)
  - DB Browser for SQLite: https://sqlitebrowser.org/
  - SQLite Studio: https://sqlitestudio.pl/

- **API Testing Tools**
  - Postman: https://www.postman.com/
  - REST Client VS Code Extension

## Installation Steps

### Step 1: Clone the Repository

```bash
# Clone the repository
git clone https://github.com/rajibmahata/BudgetEase.git

# Navigate to the project directory
cd BudgetEase
```

### Step 2: Restore Dependencies

```bash
# Restore all NuGet packages for the solution
dotnet restore
```

Expected output:
```
Restore complete (X.Xs)
```

### Step 3: Build the Solution

```bash
# Build all projects
dotnet build
```

Expected output:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

### Step 4: Set Up the Database

#### Option A: Automatic Setup (Recommended)
The application will automatically create and migrate the database on first run.

#### Option B: Manual Setup
```bash
# Navigate to the API project
cd src/BudgetEase.Api

# Install EF Core tools (if not already installed)
dotnet tool install --global dotnet-ef

# Apply migrations
dotnet ef database update

# Return to root directory
cd ../..
```

## Running the Application

### Option 1: Using Visual Studio

1. Open `BudgetEase.sln` in Visual Studio
2. Set multiple startup projects:
   - Right-click solution → Properties
   - Select "Multiple startup projects"
   - Set both `BudgetEase.Api` and `BudgetEase.Web` to "Start"
3. Press F5 or click "Start"

### Option 2: Using Command Line

#### Terminal 1 - Run the Web Application
```bash
cd src/BudgetEase.Web
dotnet run
```

#### Terminal 2 - Run the API (Optional)
```bash
cd src/BudgetEase.Api
dotnet run
```

### Option 3: Using Visual Studio Code

1. Open the project folder in VS Code
2. Open integrated terminal (Ctrl+`)
3. Run the commands from Option 2

## Accessing the Application

After starting the application:

### Web Application (Blazor)
- HTTPS: https://localhost:7001
- HTTP: http://localhost:5001

### API (Optional)
- HTTPS: https://localhost:7002
- HTTP: http://localhost:5002
- OpenAPI: https://localhost:7002/openapi/v1.json

### Default Ports

If ports are in use, .NET will automatically assign different ports. Check the console output for actual URLs.

## Database Management

### Database Location
The SQLite database file `budgetease.db` will be created in:
- API Project: `src/BudgetEase.Api/budgetease.db`
- Web Project: `src/BudgetEase.Web/budgetease.db`

### Database Migrations

#### Create a New Migration
```bash
cd src/BudgetEase.Api
dotnet ef migrations add MigrationName --project ../BudgetEase.Infrastructure/BudgetEase.Infrastructure.csproj --startup-project . --context ApplicationDbContext --output-dir Data/Migrations
```

#### Update Database
```bash
dotnet ef database update
```

#### Rollback Migration
```bash
dotnet ef database update PreviousMigrationName
```

#### Remove Last Migration
```bash
dotnet ef migrations remove
```

### Viewing Database

Use DB Browser for SQLite:
1. Open DB Browser
2. File → Open Database
3. Navigate to `budgetease.db`
4. Browse tables: Events, Expenses, Vendors, AspNetUsers, etc.

## Configuration

### Connection String

Edit `appsettings.json` in both API and Web projects:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=budgetease.db"
  }
}
```

**For Production (SQL Server):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=BudgetEase;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Email Configuration (SendGrid)

1. Sign up at https://sendgrid.com/
2. Create an API key
3. Update `appsettings.json`:

```json
{
  "SendGrid": {
    "ApiKey": "YOUR_ACTUAL_API_KEY_HERE"
  }
}
```

**For Development (User Secrets):**
```bash
cd src/BudgetEase.Api
dotnet user-secrets init
dotnet user-secrets set "SendGrid:ApiKey" "YOUR_ACTUAL_API_KEY_HERE"
```

### CORS Configuration

API is pre-configured to allow requests from Blazor app. To modify:

Edit `Program.cs` in BudgetEase.Api:
```csharp
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
```

## Troubleshooting

### Issue: Port Already in Use

**Solution:**
```bash
# Kill process on port 5001 (Linux/Mac)
lsof -ti:5001 | xargs kill -9

# Kill process on port 5001 (Windows)
netstat -ano | findstr :5001
taskkill /PID <PID> /F
```

Or change ports in `Properties/launchSettings.json`:
```json
"applicationUrl": "https://localhost:7101;http://localhost:5101"
```

### Issue: Database Migration Errors

**Solution:**
1. Delete `budgetease.db` file
2. Delete `Migrations` folder
3. Recreate migrations:
```bash
cd src/BudgetEase.Api
dotnet ef migrations add InitialCreate --project ../BudgetEase.Infrastructure/BudgetEase.Infrastructure.csproj
dotnet ef database update
```

### Issue: NuGet Package Restore Fails

**Solution:**
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore --force
```

### Issue: Build Errors

**Solution:**
1. Clean the solution:
```bash
dotnet clean
```

2. Delete `bin` and `obj` folders:
```bash
# Linux/Mac
find . -name "bin" -o -name "obj" | xargs rm -rf

# Windows PowerShell
Get-ChildItem -Include bin,obj -Recurse | Remove-Item -Force -Recurse
```

3. Rebuild:
```bash
dotnet build
```

### Issue: Database Connection Errors

**Symptoms:** "Unable to open the database file"

**Solution:**
1. Check file permissions on `budgetease.db`
2. Ensure the directory is writable
3. Try absolute path in connection string:
```json
"DefaultConnection": "Data Source=C:/Path/To/budgetease.db"
```

### Issue: HTTPS Certificate Errors

**Solution:**
```bash
# Trust the development certificate
dotnet dev-certs https --trust
```

If still having issues:
```bash
# Clean and recreate certificate
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

## Development Tips

### Hot Reload

When running with `dotnet run` or `dotnet watch`:
- Blazor: Changes to `.razor` files auto-refresh
- API: Changes to `.cs` files trigger rebuild

### Debugging

**Visual Studio:**
- Set breakpoints (F9)
- Start debugging (F5)

**VS Code:**
1. Install C# Dev Kit extension
2. Open Run and Debug (Ctrl+Shift+D)
3. Select "Launch .NET Core Web App"
4. Press F5

### Logging

Check console output for detailed logs:
- Information: General application flow
- Warning: Unexpected but handled issues
- Error: Exceptions and failures

To increase log verbosity, edit `appsettings.Development.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  }
}
```

## Testing the Application

### Manual Testing

1. **Create an Event**
   - Go to Events → Create New Event
   - Fill in details and submit
   
2. **Add Expenses**
   - Open event details
   - Add multiple expenses in different categories
   
3. **Add Vendors**
   - Open event details
   - Add vendors with contact information
   
4. **Check Dashboard**
   - Return to home page
   - Verify budget calculations
   - Check progress bars

### API Testing

Use the provided `.http` file with REST Client:

```bash
# Open in VS Code with REST Client extension
code src/BudgetEase.Api/BudgetEase.Api.http
```

Or use Postman:
1. Import OpenAPI spec from: https://localhost:7002/openapi/v1.json
2. Test endpoints

## Production Deployment

### Publish the Application

```bash
# Publish API
cd src/BudgetEase.Api
dotnet publish -c Release -o ../../publish/api

# Publish Web
cd ../BudgetEase.Web
dotnet publish -c Release -o ../../publish/web
```

### Update Connection String

For production, update to use a proper database server:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=BudgetEase;User Id=user;Password=pass;"
  }
}
```

### Security Considerations

1. **Never commit secrets** to source control
2. Use **Azure Key Vault** or environment variables for sensitive data
3. Enable **HTTPS** in production
4. Set **strong passwords** for database users
5. Implement **rate limiting** on API endpoints

## Getting Help

If you encounter issues not covered here:

1. Check the [ARCHITECTURE.md](ARCHITECTURE.md) for technical details
2. Review the [README.md](README.md) for feature overview
3. Open an issue on GitHub: https://github.com/rajibmahata/BudgetEase/issues
4. Contact: rajibmahata@example.com

## Next Steps

After setup:
1. Review [ARCHITECTURE.md](ARCHITECTURE.md) to understand the codebase
2. Explore the code in `src/` directory
3. Try modifying a Blazor page
4. Add a new feature!

---

Happy coding! 🚀
