# BudgetEase - Project Structure

## 📁 Directory Structure

```
BudgetEase/
├── 📄 BudgetEase.sln              # Solution file
├── 📄 .gitignore                  # Git ignore configuration
├── 📘 README.md                   # Project overview and documentation
├── 📘 REQUIREMENTS_VALIDATION.md  # Requirements analysis (16KB)
├── 📘 TODO.md                     # Implementation checklist (10KB)
├── 📘 TASK_SUMMARY.md            # Task completion summary
├── 📘 PROJECT_STRUCTURE.md       # This file
│
└── 📂 src/                        # Source code directory
    │
    ├── 📂 BudgetEase.Api/         # 🌐 Web API Project
    │   ├── 📂 Controllers/
    │   │   ├── EventsController.cs      # Event CRUD operations
    │   │   ├── ExpensesController.cs    # Expense tracking
    │   │   └── VendorsController.cs     # Vendor management
    │   ├── 📂 Properties/
    │   │   └── launchSettings.json
    │   ├── 📄 BudgetEase.Api.csproj
    │   ├── 📄 BudgetEase.Api.http       # HTTP request samples
    │   ├── 📄 Program.cs                # API startup & configuration
    │   ├── 📄 appsettings.json
    │   └── 📄 appsettings.Development.json
    │
    ├── 📂 BudgetEase.Core/         # 🎯 Domain Layer
    │   ├── 📂 Entities/
    │   │   ├── ApplicationUser.cs       # User entity (extends IdentityUser)
    │   │   ├── Event.cs                 # Event entity
    │   │   ├── EventCollaborator.cs     # Collaborator relationship
    │   │   ├── Expense.cs               # Expense entity
    │   │   └── Vendor.cs                # Vendor entity
    │   ├── 📂 DTOs/
    │   │   ├── EventDto.cs              # Event data transfer objects
    │   │   ├── ExpenseDto.cs            # Expense DTOs
    │   │   └── VendorDto.cs             # Vendor DTOs
    │   ├── 📂 Interfaces/
    │   │   ├── IEventRepository.cs      # Event repository contract
    │   │   ├── IExpenseRepository.cs    # Expense repository contract
    │   │   └── IVendorRepository.cs     # Vendor repository contract
    │   └── 📄 BudgetEase.Core.csproj
    │
    ├── 📂 BudgetEase.Infrastructure/  # 🗄️ Data Access Layer
    │   ├── 📂 Data/
    │   │   ├── ApplicationDbContext.cs  # EF Core DbContext
    │   │   └── Migrations/              # EF Core migrations (auto-generated)
    │   ├── 📂 Repositories/
    │   │   ├── EventRepository.cs       # Event data access
    │   │   ├── ExpenseRepository.cs     # Expense data access
    │   │   └── VendorRepository.cs      # Vendor data access
    │   └── 📄 BudgetEase.Infrastructure.csproj
    │
    └── 📂 BudgetEase.Web/          # 🎨 Blazor Frontend
        ├── 📂 Components/
        │   ├── 📂 Layout/
        │   │   ├── MainLayout.razor     # Main page layout
        │   │   ├── MainLayout.razor.css
        │   │   ├── NavMenu.razor        # Navigation menu
        │   │   └── NavMenu.razor.css
        │   ├── 📂 Pages/
        │   │   ├── Home.razor           # Home page (template)
        │   │   ├── Counter.razor        # Template page (to be replaced)
        │   │   ├── Weather.razor        # Template page (to be replaced)
        │   │   └── Error.razor          # Error page
        │   ├── App.razor                # Root app component
        │   ├── Routes.razor             # Route configuration
        │   └── _Imports.razor           # Global imports
        ├── 📂 Properties/
        │   └── launchSettings.json
        ├── 📂 wwwroot/
        │   ├── app.css                  # Application styles
        │   ├── bootstrap/               # Bootstrap framework
        │   └── favicon.png
        ├── 📄 BudgetEase.Web.csproj
        ├── 📄 Program.cs                # Web app startup
        ├── 📄 appsettings.json
        └── 📄 appsettings.Development.json
```

## 🏗️ Architecture Layers

```
┌─────────────────────────────────────────────────────────────┐
│                   🎨 Presentation Layer                      │
│              BudgetEase.Web (Blazor Server)                 │
│  ┌────────────────────────────────────────────────────┐    │
│  │  Components  │  Pages  │  Layout  │  wwwroot       │    │
│  └────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────┘
                            ↕ HTTP
┌─────────────────────────────────────────────────────────────┐
│                    🌐 API Layer                              │
│            BudgetEase.Api (Web API)                         │
│  ┌────────────────────────────────────────────────────┐    │
│  │  EventsController  │  ExpensesController  │  ...   │    │
│  └────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────┘
                            ↕ Services
┌─────────────────────────────────────────────────────────────┐
│                  🎯 Domain Layer                             │
│                BudgetEase.Core                              │
│  ┌────────────────────────────────────────────────────┐    │
│  │  Entities  │  DTOs  │  Interfaces  │  Services     │    │
│  └────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────┘
                            ↕ Data Access
┌─────────────────────────────────────────────────────────────┐
│              🗄️ Data Access Layer                           │
│          BudgetEase.Infrastructure                          │
│  ┌────────────────────────────────────────────────────┐    │
│  │  Repositories  │  DbContext  │  Migrations         │    │
│  └────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────┘
                            ↕ EF Core
┌─────────────────────────────────────────────────────────────┐
│                  💾 Database                                 │
│               SQLite Database                               │
│  ┌────────────────────────────────────────────────────┐    │
│  │  Events  │  Expenses  │  Vendors  │  Users         │    │
│  └────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────┘
```

## 🔑 Key Components

### BudgetEase.Api (Web API)
**Purpose:** RESTful API endpoints  
**Status:** ✅ ~80% Complete  
**Port:** https://localhost:7000

**Controllers:**
- `EventsController` - CRUD for events with budget calculations
- `ExpensesController` - Expense tracking with vendor linking
- `VendorsController` - Vendor management with reminders

**Missing:**
- ❌ AuthController (Login, Register, Logout)
- ❌ ReportsController (PDF/Excel generation)
- ❌ NotificationsController (Email notifications)

### BudgetEase.Core (Domain)
**Purpose:** Business entities, DTOs, and interfaces  
**Status:** ✅ ~90% Complete

**Entities:**
- `ApplicationUser` - User with Identity integration
- `Event` - Event with budget tracking
- `EventCollaborator` - Multi-user event access
- `Expense` - Expense tracking (estimated vs actual)
- `Vendor` - Vendor contact and reminder management

**DTOs:**
- Full CRUD DTOs for Event, Expense, Vendor
- Separate Create/Update/Read DTOs

**Interfaces:**
- Repository interfaces for all entities
- (Missing) Service interfaces for notifications, reports

### BudgetEase.Infrastructure (Data Access)
**Purpose:** Database access and repository implementations  
**Status:** ✅ ~85% Complete

**DbContext:**
- `ApplicationDbContext` - EF Core context with Identity
- Full entity configurations
- Proper relationships and cascading deletes
- Decimal precision for currency fields

**Repositories:**
- `EventRepository` - Event data access with collaborator support
- `ExpenseRepository` - Expense CRUD with total calculations
- `VendorRepository` - Vendor CRUD with reminder queries

**Missing:**
- ❌ NotificationRepository (if needed)
- ❌ ReportRepository (if needed)

### BudgetEase.Web (Blazor Frontend)
**Purpose:** User interface  
**Status:** ⚠️ ~10% Complete  
**Port:** https://localhost:7001

**Existing:**
- ✅ Blazor Server configuration
- ✅ Basic layout and navigation
- ✅ Template pages (Home, Counter, Weather)

**Missing:**
- ❌ Authentication pages (Login, Register)
- ❌ Dashboard with charts
- ❌ Events pages (List, Details, Create, Edit)
- ❌ Expenses pages
- ❌ Vendors pages
- ❌ Reports pages
- ❌ Settings pages

## 📦 Dependencies

### Shared Dependencies
- `Microsoft.EntityFrameworkCore.Sqlite` - Database
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` - Authentication
- `SendGrid` - Email (not yet integrated)

### API Specific
- `Microsoft.AspNetCore.OpenApi` - API documentation

### Web Specific
- `Microsoft.AspNetCore.Components.Web` - Blazor components

## 🔄 Data Flow

### Example: Creating an Expense

```
User (Browser)
    ↓
Blazor Page (Expenses.razor)
    ↓ HTTP POST
API Controller (ExpensesController.CreateExpense)
    ↓ Map DTO to Entity
Repository (ExpenseRepository.CreateAsync)
    ↓ EF Core
DbContext (ApplicationDbContext)
    ↓ SQL
SQLite Database
    ↓ Saved
DbContext
    ↓ Entity
Repository
    ↓ Map Entity to DTO
API Controller
    ↓ HTTP 201 Created
Blazor Page
    ↓ Update UI
User (Browser) - Success!
```

## 📊 Statistics

| Metric | Count | Status |
|--------|-------|--------|
| Projects | 4 | ✅ |
| Controllers | 3 | ⚠️ (Need +3) |
| Entities | 5 | ✅ |
| Repositories | 3 | ✅ |
| Blazor Pages | 4 | ⚠️ (Need +10) |
| API Endpoints | 15 | ⚠️ (Need +8) |
| DTOs | 12 | ✅ |
| Documentation Files | 5 | ✅ |

## 🎯 Next Development Areas

### 1. Authentication (Priority: HIGH)
- Add `AuthController.cs` in BudgetEase.Api/Controllers
- Add `Login.razor` in BudgetEase.Web/Components/Pages
- Add `Register.razor` in BudgetEase.Web/Components/Pages
- Configure authentication middleware

### 2. Dashboard (Priority: HIGH)
- Replace `Home.razor` with dashboard
- Add charting library
- Create summary cards component
- Add recent activity feed

### 3. Business Pages (Priority: HIGH)
- Create `Events.razor` - Event list and creation
- Create `EventDetails.razor` - Event details with tabs
- Create `Expenses.razor` - Expense tracking
- Create `Vendors.razor` - Vendor management

### 4. Reports (Priority: MEDIUM)
- Add `ReportsController.cs`
- Add PDF generation library
- Add Excel generation library
- Create `Reports.razor`

### 5. Notifications (Priority: MEDIUM)
- Add `NotificationService.cs`
- Configure SendGrid
- Add background jobs
- Email template creation

## 📚 References

- [README.md](README.md) - Project overview
- [REQUIREMENTS_VALIDATION.md](REQUIREMENTS_VALIDATION.md) - Detailed analysis
- [TODO.md](TODO.md) - Implementation checklist
- [TASK_SUMMARY.md](TASK_SUMMARY.md) - Task completion report

---

**Last Updated:** 2024  
**Version:** 1.0  
**Status:** Active Development (29% Complete)
