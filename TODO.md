# BudgetEase - Implementation TODO

## 🚀 Phase 1: Critical Foundation (HIGH Priority)

### Authentication System
- [ ] Create `AuthController` with endpoints
  - [ ] POST /api/auth/register
  - [ ] POST /api/auth/login
  - [ ] POST /api/auth/logout
  - [ ] POST /api/auth/forgot-password
  - [ ] POST /api/auth/reset-password
  - [ ] POST /api/auth/change-password
- [ ] Create authentication Blazor pages
  - [ ] Login.razor
  - [ ] Register.razor
  - [ ] ForgotPassword.razor
  - [ ] ResetPassword.razor
- [ ] Configure authentication middleware in API Program.cs
- [ ] Add [Authorize] attributes to controllers
- [ ] Implement role seeding (Admin, Collaborator)
- [ ] Add role-based authorization policies
- [ ] (Optional) Configure Google OAuth

### Core UI Pages
- [ ] Create Dashboard.razor
  - [ ] Summary cards (Budget, Spent, Remaining)
  - [ ] Recent expenses list
  - [ ] Upcoming reminders
- [ ] Create Events.razor
  - [ ] Event list view
  - [ ] Create event form
  - [ ] Delete event confirmation
- [ ] Create EventDetails.razor
  - [ ] Event information display
  - [ ] Edit event form
  - [ ] Tabs for Expenses, Vendors
- [ ] Create Expenses.razor (or component in EventDetails)
  - [ ] Expense list for event
  - [ ] Add expense form
  - [ ] Edit expense inline or modal
  - [ ] Delete expense confirmation
  - [ ] Budget progress bar
- [ ] Create Vendors.razor (or component in EventDetails)
  - [ ] Vendor list for event
  - [ ] Add vendor form
  - [ ] Edit vendor inline or modal
  - [ ] Delete vendor confirmation
  - [ ] Reminder status indicators

### Navigation & Layout
- [ ] Update NavMenu.razor
  - [ ] Remove Counter and Weather links
  - [ ] Add Dashboard link
  - [ ] Add Events link
  - [ ] Add authentication status
  - [ ] Add login/logout buttons
  - [ ] Add user profile display
- [ ] Update MainLayout.razor
  - [ ] Add authentication state provider
  - [ ] Style improvements
- [ ] Update Home.razor to redirect to Dashboard
- [ ] Configure authentication state in Program.cs

---

## 🎨 Phase 2: Enhanced Features (MEDIUM Priority)

### Dashboard Enhancements
- [ ] Install charting library (Blazor.Charts or ApexCharts)
- [ ] Add pie chart for category-wise expenses
- [ ] Add bar chart for vendor-wise expenses
- [ ] Add budget vs actual progress chart
- [ ] Add event timeline/calendar view
- [ ] Make dashboard responsive

### Report Generation
- [ ] Install PDF library (QuestPDF or iTextSharp)
- [ ] Install Excel library (EPPlus or ClosedXML)
- [ ] Create `IReportService` interface
- [ ] Implement `ReportService`
  - [ ] GeneratePdfReport method
  - [ ] GenerateExcelReport method
  - [ ] GenerateEmailReport method
- [ ] Create `ReportsController`
  - [ ] GET /api/reports/event/{eventId}/pdf
  - [ ] GET /api/reports/event/{eventId}/excel
  - [ ] POST /api/reports/event/{eventId}/email
- [ ] Create Reports.razor page
  - [ ] Report preview
  - [ ] Export options (PDF/Excel)
  - [ ] Email report form
- [ ] Design report templates

### Notification System
- [ ] Configure SendGrid API key in appsettings
- [ ] Create `INotificationService` interface
- [ ] Implement `EmailNotificationService`
  - [ ] SendBudgetAlert method (80% threshold)
  - [ ] SendPaymentReminder method
  - [ ] SendEventReminder method
  - [ ] SendVendorCallReminder method
- [ ] Create email templates
  - [ ] BudgetAlert.html
  - [ ] PaymentReminder.html
  - [ ] EventReminder.html
  - [ ] VendorReminder.html
- [ ] Install Hangfire for background jobs
- [ ] Create background job services
  - [ ] Budget monitoring job (daily)
  - [ ] Payment reminder job (daily)
  - [ ] Event reminder job (daily)
  - [ ] Vendor reminder job (daily)
- [ ] Add Hangfire dashboard
- [ ] Test notification delivery

---

## 💎 Phase 3: Advanced Features (LOW Priority)

### Collaborator Management
- [ ] Create `IEventCollaboratorRepository` interface
- [ ] Implement `EventCollaboratorRepository`
- [ ] Create collaborator DTOs
- [ ] Add collaborator endpoints to EventsController
  - [ ] GET /api/events/{id}/collaborators
  - [ ] POST /api/events/{id}/collaborators
  - [ ] DELETE /api/events/{id}/collaborators/{collaboratorId}
- [ ] Add collaborator management UI to EventDetails
  - [ ] List collaborators
  - [ ] Add collaborator by email
  - [ ] Remove collaborator
  - [ ] Show permissions

### Enhancements
- [ ] Create category management
  - [ ] Predefined expense categories
  - [ ] Custom category creation
  - [ ] Category icons/colors
- [ ] Create service type management
  - [ ] Predefined vendor service types
  - [ ] Custom service type creation
- [ ] Add data validation
  - [ ] Add validation attributes to DTOs
  - [ ] Add fluent validation
  - [ ] Client-side validation
- [ ] Improve error handling
  - [ ] Global exception handler
  - [ ] User-friendly error messages
  - [ ] Error logging
- [ ] Add loading states
  - [ ] Spinner components
  - [ ] Skeleton loaders
  - [ ] Progress indicators
- [ ] Add search and filtering
  - [ ] Search events
  - [ ] Filter expenses by category
  - [ ] Filter vendors by service type
  - [ ] Date range filtering
- [ ] Add bulk operations
  - [ ] Bulk delete expenses
  - [ ] Bulk delete vendors
  - [ ] Export all events

---

## 🧪 Phase 4: Testing & Quality (HIGH Priority)

### Testing
- [ ] Create test projects
  - [ ] BudgetEase.Core.Tests
  - [ ] BudgetEase.Infrastructure.Tests
  - [ ] BudgetEase.Api.Tests
- [ ] Write unit tests
  - [ ] Repository tests
  - [ ] Service tests
  - [ ] Controller tests
- [ ] Write integration tests
  - [ ] API endpoint tests
  - [ ] Database tests
- [ ] Add code coverage reporting
- [ ] Set coverage targets (>80%)

### Code Quality
- [ ] Add XML documentation comments
- [ ] Configure code analysis rules
- [ ] Run static code analysis
- [ ] Fix code smells
- [ ] Implement logging
  - [ ] Install Serilog
  - [ ] Configure structured logging
  - [ ] Add log levels
  - [ ] Log important events

### Documentation
- [ ] Update README.md
  - [ ] Setup instructions
  - [ ] Running the application
  - [ ] API documentation
- [ ] Create API documentation
  - [ ] Swagger/OpenAPI descriptions
  - [ ] Example requests/responses
- [ ] Create user guide
  - [ ] How to create events
  - [ ] How to track expenses
  - [ ] How to manage vendors
  - [ ] How to view reports

---

## 🚀 Phase 5: Deployment & DevOps

### Deployment Preparation
- [ ] Configure production settings
  - [ ] Production database connection
  - [ ] Production API URLs
  - [ ] SendGrid production keys
- [ ] Setup environment variables
- [ ] Create deployment scripts
- [ ] Configure HTTPS
- [ ] Setup database migrations
  - [ ] Migration scripts
  - [ ] Seed data for production

### CI/CD
- [ ] Setup GitHub Actions
  - [ ] Build workflow
  - [ ] Test workflow
  - [ ] Deploy workflow
- [ ] Configure automated testing
- [ ] Configure automated deployment
- [ ] Setup staging environment

### Monitoring
- [ ] Configure application insights
- [ ] Setup health checks
- [ ] Configure error tracking (e.g., Sentry)
- [ ] Setup performance monitoring
- [ ] Configure backup strategy

---

## 🐛 Technical Debt

### Immediate Fixes
- [x] Add .gitignore for build artifacts
- [ ] Add input validation to all controllers
- [ ] Add error handling middleware
- [ ] Add request/response logging
- [ ] Add rate limiting
- [ ] Add CORS configuration review
- [ ] Add security headers

### Code Improvements
- [ ] Refactor controllers to use MediatR (optional)
- [ ] Add AutoMapper for DTO mapping
- [ ] Implement specification pattern for queries
- [ ] Add caching for frequently accessed data
- [ ] Optimize database queries
- [ ] Add database indexes

### Security
- [ ] Implement JWT token expiration
- [ ] Add refresh token support
- [ ] Implement account lockout
- [ ] Add two-factor authentication (optional)
- [ ] Add CSRF protection
- [ ] Add SQL injection prevention review
- [ ] Add XSS prevention review
- [ ] Implement content security policy

---

## 📦 Required NuGet Packages

### Charting
```bash
# Option 1: Blazor.Charts
dotnet add package Blazor.Charts --version 1.1.0

# Option 2: ApexCharts.Blazor
dotnet add package ApexCharts.Blazor --version 1.0.0
```

### PDF Generation
```bash
# Option 1: QuestPDF (Recommended)
dotnet add package QuestPDF --version 2024.10.0

# Option 2: iTextSharp
dotnet add package itext7 --version 8.0.0
```

### Excel Generation
```bash
# Option 1: EPPlus (Recommended)
dotnet add package EPPlus --version 7.0.0

# Option 2: ClosedXML
dotnet add package ClosedXML --version 0.102.0
```

### Background Jobs
```bash
dotnet add package Hangfire --version 1.8.0
dotnet add package Hangfire.AspNetCore --version 1.8.0
dotnet add package Hangfire.SqlServer --version 1.8.0
# OR for SQLite
dotnet add package Hangfire.Storage.SQLite --version 0.4.0
```

### Logging
```bash
dotnet add package Serilog.AspNetCore --version 8.0.0
dotnet add package Serilog.Sinks.File --version 5.0.0
```

### Testing
```bash
dotnet add package xunit --version 2.6.0
dotnet add package xunit.runner.visualstudio --version 2.5.0
dotnet add package Moq --version 4.20.0
dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 9.0.0
```

### Optional
```bash
# AutoMapper
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.0

# MediatR
dotnet add package MediatR --version 12.0.0

# FluentValidation
dotnet add package FluentValidation.AspNetCore --version 11.3.0
```

---

## 📊 Progress Tracking

**Overall Progress:** 29% Complete

| Module | Backend | Frontend | Overall |
|--------|---------|----------|---------|
| Authentication | 60% | 0% | 30% |
| Event Management | 90% | 0% | 45% |
| Expense Tracker | 100% | 0% | 50% |
| Vendor Management | 100% | 0% | 50% |
| Reports | 0% | 0% | 0% |
| Notifications | 10% | 0% | 5% |
| Dashboard | 50% | 0% | 25% |

---

## 🎯 Success Criteria

### MVP (Minimum Viable Product)
- [x] Users can register and login
- [ ] Users can create events
- [ ] Users can add expenses to events
- [ ] Users can manage vendors
- [ ] Dashboard shows budget overview
- [ ] All endpoints are secured

### Full Product
- [ ] All MVPfeatures
- [ ] Email notifications working
- [ ] Reports can be generated and exported
- [ ] Dashboard has charts
- [ ] Collaborators can be added to events
- [ ] Background jobs running for reminders
- [ ] Application is deployed and accessible

---

**Last Updated:** 2024  
**Status:** In Progress
