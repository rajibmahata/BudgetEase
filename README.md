# BudgetEase

**BudgetEase** is a modern event expense tracking and management application built with ASP.NET Core and Blazor Server. It helps users efficiently manage budgets, track expenses, handle vendor communications, and monitor financial summaries all in one place.

## 📋 Project Status

**Current Version:** 0.7 (In Development)  
**Overall Completion:** ~70%
- Backend: ~90% Complete
- Frontend: ~65% Complete

## 📖 Documentation

- **[User Flow Documentation](USER_FLOW.md)** - Complete user flow diagrams with Mermaid visualizations and API documentation
- **[Requirements Validation](REQUIREMENTS_VALIDATION.md)** - Comprehensive analysis of requirements vs implementation
- **[TODO Checklist](TODO.md)** - Detailed implementation checklist and roadmap
- **[Project Structure](PROJECT_STRUCTURE.md)** - Architecture and code organization
- **[Demo Data Documentation](DEMO_DATA.md)** - Sample data and demo user credentials
- **[Database Backup & Restore](DATABASE_BACKUP.md)** - Automatic backup system documentation

## 🎯 Purpose

A lightweight web application for managing and tracking expenses for personal or professional events such as:
- Weddings
- Birthday parties
- Anniversaries
- Any custom event

## ✨ Features

### Implemented (Backend)
- ✅ **Event Management** - Create, update, delete events with budget tracking
- ✅ **Expense Tracker** - Track expenses by category with estimated and actual costs
- ✅ **Vendor Management** - Manage vendor details, contacts, and reminders
- ✅ **Multi-user Support** - Event owner and collaborator roles
- ✅ **RESTful API** - Complete API endpoints for all features
- ✅ **SQLite Database** - Lightweight database with Entity Framework Core
- ✅ **Database Backup & Restore** - Automatic daily backups with restore on startup

### In Progress / Planned
- ⏳ **User Authentication** - Login, register, password recovery
- ⏳ **Dashboard** - Visual overview with charts and statistics
- ⏳ **Reports** - PDF/Excel export with email delivery
- ⏳ **Notifications** - Email alerts for budget thresholds and reminders
- ⏳ **Frontend UI** - Blazor pages for all features

## 🏗️ Architecture

```
BudgetEase/
├── src/
│   ├── BudgetEase.Api/          # ASP.NET Core Web API
│   ├── BudgetEase.Core/         # Domain entities, DTOs, interfaces
│   ├── BudgetEase.Infrastructure/ # Repositories, data access
│   └── BudgetEase.Web/          # Blazor Server frontend
├── REQUIREMENTS_VALIDATION.md   # Requirements analysis
└── TODO.md                      # Implementation checklist
```

## 🚀 Technology Stack

| Layer | Technology |
|-------|-----------|
| Frontend | Blazor Server |
| Backend API | ASP.NET Core 9.0 |
| Database | SQLite |
| ORM | Entity Framework Core |
| Authentication | ASP.NET Identity |
| Email | SendGrid (planned) |
| Reports | PDF/Excel (planned) |

## 🛠️ Getting Started

### Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 / VS Code / Rider

### Running the API
```bash
cd src/BudgetEase.Api
dotnet run
```
API will be available at: `http://localhost:5108`

### Running the Web UI
```bash
cd src/BudgetEase.Web
dotnet run
```
Web UI will be available at: `https://localhost:7001`

### Building the Solution
```bash
dotnet build
```

### Database & Sample Data
The database is created automatically on first run with sample data for demonstration:
- **Demo User:** demo@budgetease.com / Demo@123
- **Sample Events:** 4 pre-configured events with expenses and vendors
- See [DEMO_DATA.md](DEMO_DATA.md) for complete details

To reset the database, delete `src/BudgetEase.Api/budgetease.db` and restart the API.

## 📊 API Endpoints

### Events
- `GET /api/events` - List all events
- `GET /api/events/{id}` - Get event details
- `POST /api/events` - Create new event
- `PUT /api/events/{id}` - Update event
- `DELETE /api/events/{id}` - Delete event

### Expenses
- `GET /api/expenses/event/{eventId}` - List expenses for event
- `GET /api/expenses/{id}` - Get expense details
- `POST /api/expenses` - Create new expense
- `PUT /api/expenses/{id}` - Update expense
- `DELETE /api/expenses/{id}` - Delete expense

### Vendors
- `GET /api/vendors/event/{eventId}` - List vendors for event
- `GET /api/vendors/{id}` - Get vendor details
- `GET /api/vendors/reminders` - Get upcoming reminders
- `POST /api/vendors` - Create new vendor
- `PUT /api/vendors/{id}` - Update vendor
- `DELETE /api/vendors/{id}` - Delete vendor

## 📈 Roadmap

### Phase 1: Critical Foundation (Current)
- [ ] Authentication system (Login, Register)
- [ ] Core UI pages (Dashboard, Events, Expenses, Vendors)
- [ ] Navigation and layout updates

### Phase 2: Enhanced Features
- [ ] Dashboard with charts
- [ ] Report generation (PDF/Excel)
- [ ] Email notification system

### Phase 3: Advanced Features
- [ ] Collaborator management
- [ ] Category and service type management
- [ ] Enhanced validation and error handling

### Phase 4: Testing & Deployment
- [ ] Unit and integration tests
- [ ] Documentation
- [ ] Production deployment

See [TODO.md](TODO.md) for detailed checklist.

## 🤝 Contributing

This is an active development project. Contributions are welcome!

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a Pull Request

## 📝 License

This project is currently under development.

## 📞 Contact

For questions or feedback, please open an issue in the repository.
