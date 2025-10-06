# BudgetEase Architecture Documentation

## Overview
BudgetEase is a modern event expense tracking and management application built with ASP.NET Core and Blazor Server. The application follows a clean architecture pattern with clear separation of concerns.

## Technology Stack

### Frontend
- **Blazor Server** - Interactive web UI with server-side rendering
- **Bootstrap 5** - Responsive UI framework
- **Bootstrap Icons** - Icon library

### Backend
- **ASP.NET Core 9.0 Web API** - RESTful API services
- **ASP.NET Core Identity** - Authentication and authorization
- **Entity Framework Core 9.0** - ORM for data access

### Database
- **SQLite** - Lightweight, file-based database (suitable for development and small deployments)
- Can be easily switched to SQL Server or PostgreSQL for production

### Additional Libraries
- **SendGrid** - Email notifications (configured, ready to use)
- Future: PDF/Excel generation libraries for reports

## Solution Structure

```
BudgetEase/
├── src/
│   ├── BudgetEase.Core/           # Domain layer
│   │   ├── Entities/              # Domain entities
│   │   ├── Interfaces/            # Repository interfaces
│   │   └── DTOs/                  # Data transfer objects
│   │
│   ├── BudgetEase.Infrastructure/ # Data access layer
│   │   ├── Data/                  # DbContext and migrations
│   │   ├── Repositories/          # Repository implementations
│   │   └── Services/              # Infrastructure services
│   │
│   ├── BudgetEase.Api/            # API layer
│   │   └── Controllers/           # API controllers
│   │
│   └── BudgetEase.Web/            # Presentation layer
│       ├── Components/
│       │   ├── Pages/             # Blazor pages
│       │   └── Layout/            # Layout components
│       └── wwwroot/               # Static files
│
├── BudgetEase.sln                 # Solution file
└── README.md                      # Project documentation
```

## Database Schema

### Tables and Relationships

#### AspNetUsers (Identity)
- Extends with custom fields: FirstName, LastName, CreatedAt
- Primary authentication table

#### Events
- **Primary Key:** Id (int)
- **Foreign Key:** OwnerId → AspNetUsers.Id
- Fields: Name, Type, EventDate, Venue, BudgetLimit, CreatedAt, UpdatedAt
- Relationships:
  - One-to-Many with Expenses
  - One-to-Many with Vendors
  - One-to-Many with EventCollaborators

#### Expenses
- **Primary Key:** Id (int)
- **Foreign Keys:**
  - EventId → Events.Id
  - VendorId → Vendors.Id (nullable)
- Fields: Category, Description, EstimatedCost, ActualCost, PaymentStatus, PaymentDueDate
- Relationships:
  - Many-to-One with Event
  - Many-to-One with Vendor (optional)

#### Vendors
- **Primary Key:** Id (int)
- **Foreign Key:** EventId → Events.Id
- Fields: Name, ServiceType, ContactNumber, Email, PaymentTerms, NextReminderDate
- Relationships:
  - Many-to-One with Event
  - One-to-Many with Expenses

#### EventCollaborators
- **Primary Key:** Id (int)
- **Foreign Keys:**
  - EventId → Events.Id
  - UserId → AspNetUsers.Id
- Fields: AddedAt
- Purpose: Allow multiple users to collaborate on an event

## Key Features Implementation

### 1. User Authentication
- **Framework:** ASP.NET Core Identity
- **Features:**
  - Email & Password authentication
  - Role-based access (Admin/Collaborator)
  - Password recovery (token-based)
- **Location:** Configured in both API and Web projects

### 2. Event Management
- **CRUD Operations:** Full Create, Read, Update, Delete functionality
- **API Endpoints:** `/api/events`
- **Blazor Pages:**
  - `/events` - List all events
  - `/events/create` - Create new event
  - `/events/details/{id}` - View event details

### 3. Expense Tracker
- **Features:**
  - Category-based expense tracking
  - Vendor association
  - Payment status tracking (Paid/Pending/Partial)
  - Budget vs actual cost comparison
- **API Endpoints:** `/api/expenses`
- **Real-time Budget Calculation:** Automatically calculates remaining budget

### 4. Vendor Management
- **Features:**
  - Complete vendor contact information
  - Service type classification
  - Payment terms tracking
  - Reminder scheduling
- **API Endpoints:** `/api/vendors`
- **Special Endpoint:** `/api/vendors/reminders` - Get vendors with upcoming reminders

### 5. Dashboard
- **Features:**
  - Quick budget summary cards
  - Upcoming events display
  - Visual progress bars for budget usage
  - Color-coded budget alerts (green/yellow/red)

### 6. Balance Sheet & Reports
- **Current Status:** Infrastructure ready
- **Planned Features:**
  - PDF report generation
  - Excel export
  - Email reports to users
- **Libraries to Add:**
  - iTextSharp or QuestPDF for PDF
  - EPPlus or ClosedXML for Excel

### 7. Communication & Notifications
- **SendGrid Integration:** Configured and ready
- **Planned Notifications:**
  - 80% budget usage alerts
  - Payment due reminders
  - Event day reminders
  - Vendor communication reminders

## API Design

### RESTful Principles
All API endpoints follow REST conventions:
- GET - Retrieve resources
- POST - Create new resources
- PUT - Update existing resources
- DELETE - Remove resources

### Example Endpoints

#### Events Controller
```
GET    /api/events              - Get all user events
GET    /api/events/{id}         - Get specific event
POST   /api/events              - Create new event
PUT    /api/events/{id}         - Update event
DELETE /api/events/{id}         - Delete event
```

#### Expenses Controller
```
GET    /api/expenses/event/{eventId}  - Get expenses by event
GET    /api/expenses/{id}             - Get specific expense
POST   /api/expenses                  - Create new expense
PUT    /api/expenses/{id}             - Update expense
DELETE /api/expenses/{id}             - Delete expense
```

#### Vendors Controller
```
GET    /api/vendors/event/{eventId}   - Get vendors by event
GET    /api/vendors/{id}              - Get specific vendor
POST   /api/vendors                   - Create new vendor
PUT    /api/vendors/{id}              - Update vendor
DELETE /api/vendors/{id}              - Delete vendor
GET    /api/vendors/reminders         - Get vendors with upcoming reminders
```

## Data Flow

1. **User Request** → Blazor Component (Interactive Server)
2. **Component** → Repository Interface
3. **Repository** → Entity Framework Core
4. **EF Core** → SQLite Database
5. **Database** → EF Core (Data)
6. **EF Core** → Repository (Entities)
7. **Repository** → Component (DTOs/Entities)
8. **Component** → UI Rendering

## Security Considerations

### Authentication
- ASP.NET Core Identity handles user authentication
- Passwords are hashed using PBKDF2
- JWT tokens can be implemented for API authentication

### Authorization
- Role-based access control (Admin, Collaborator)
- Event ownership validation
- User isolation (users only see their own events)

### Data Protection
- Connection strings stored in appsettings.json
- Sensitive data (API keys) should be moved to User Secrets or Azure Key Vault
- HTTPS enforced in production

## Scalability & Future Enhancements

### Immediate Enhancements
1. **Authentication UI** - Login, register, password reset pages
2. **Complete Expense/Vendor Pages** - Full CRUD operations
3. **Reports Generation** - PDF and Excel export
4. **Email Notifications** - Budget alerts, reminders
5. **Charts Integration** - Chart.js or Blazor Charts for visualizations

### Future Features
1. **AI-Based Budget Prediction** - ML model to predict expenses
2. **Multi-Device Access** - Progressive Web App (PWA)
3. **Offline Support** - Service workers for offline capability
4. **Mobile Apps** - Xamarin or .NET MAUI
5. **SMS/WhatsApp Integration** - Vendor communication
6. **File Attachments** - Invoice and receipt uploads
7. **Calendar Integration** - Sync with Google Calendar, Outlook
8. **Multi-Currency Support** - International events
9. **Collaborative Features** - Real-time updates, comments
10. **Analytics Dashboard** - Advanced reporting and insights

### Performance Optimization
- Implement caching (Redis or In-Memory)
- Add pagination to large lists
- Optimize database queries with proper indexing
- Implement lazy loading for related data
- Use async/await throughout

### Testing Strategy
- Unit Tests - Repository and service logic
- Integration Tests - API endpoints
- E2E Tests - Blazor components
- Load Testing - Performance under stress

## Deployment Options

### Development
- SQLite database (included)
- IIS Express or Kestrel
- Visual Studio or VS Code

### Production Options
1. **Azure App Service** - For both API and Web
2. **Docker Containers** - Containerized deployment
3. **Kubernetes** - For high-scale deployments
4. **AWS/GCP** - Alternative cloud platforms

### Database Options for Production
- Azure SQL Database
- AWS RDS
- PostgreSQL
- SQL Server

## Configuration

### Connection Strings
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=budgetease.db"
}
```

### SendGrid (Email)
```json
"SendGrid": {
  "ApiKey": "YOUR_SENDGRID_API_KEY_HERE"
}
```

### Identity Options
Configured in Program.cs with customizable:
- Password requirements
- Lockout settings
- Token lifespan
- Cookie settings

## Development Guidelines

### Code Style
- Follow Microsoft C# coding conventions
- Use async/await for all I/O operations
- Implement proper error handling
- Add XML documentation to public APIs

### Git Workflow
- Main branch for stable code
- Feature branches for new development
- Pull requests for code review
- Semantic versioning for releases

### Testing
- Write tests for critical business logic
- Aim for >80% code coverage
- Use test-driven development (TDD) where appropriate

## Monitoring & Logging

### Logging
- Built-in ASP.NET Core logging
- Log levels: Debug, Information, Warning, Error, Critical
- Structured logging with Serilog (recommended)

### Application Insights
- Track exceptions
- Monitor performance
- User analytics
- Custom telemetry

## License
This project is open-source and available for modification and distribution.

## Support
For issues and feature requests, please use the GitHub issue tracker.
