# BudgetEase 💰

**BudgetEase** is a modern, full-featured event expense tracking and management application built with **ASP.NET Core 9.0** and **Blazor Server**. It helps users efficiently manage budgets, track expenses, handle vendor communications, and monitor financial summaries all in one place.

Perfect for managing expenses for weddings, birthdays, anniversaries, or any personal/professional event!

[![.NET](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-blue)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![SQLite](https://img.shields.io/badge/SQLite-Database-green)](https://www.sqlite.org/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## 🎯 Features

### ✅ Implemented Features

- **📅 Event Management**
  - Create and manage multiple events (weddings, birthdays, anniversaries, custom events)
  - Set budget limits for each event
  - Track event details (date, venue, type)
  - Visual budget progress indicators

- **💸 Expense Tracking**
  - Categorize expenses (Food, Decoration, Photography, etc.)
  - Track estimated vs actual costs
  - Payment status monitoring (Paid/Pending/Partial)
  - Associate expenses with vendors
  - Real-time budget calculations

- **👥 Vendor Management**
  - Store vendor contact information
  - Categorize by service type
  - Set payment terms and reminders
  - Track upcoming vendor communications

- **📊 Dashboard**
  - Quick overview of all events
  - Budget vs actual spending visualization
  - Color-coded budget alerts (green/yellow/red)
  - Upcoming events display

- **🔐 User Authentication** (Infrastructure Ready)
  - ASP.NET Core Identity integration
  - Role-based access (Admin/Collaborator)
  - Password recovery support

### 🚧 Planned Features

- **📈 Reports & Export**
  - PDF report generation
  - Excel export functionality
  - Email reports to users

- **📧 Notifications**
  - Email alerts at 80% budget usage
  - Payment due reminders
  - Event day reminders
  - Vendor communication reminders

- **📱 Enhanced UI**
  - Interactive charts (Pie/Bar charts)
  - Advanced analytics dashboard
  - Mobile-responsive design enhancements

- **🤖 AI Features**
  - Budget prediction using ML
  - Spending pattern analysis

## 🏗️ Architecture

BudgetEase follows a **Clean Architecture** pattern with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────┐
│                    Presentation Layer                    │
│  ┌──────────────────────┐   ┌────────────────────────┐ │
│  │   Blazor Server Web  │   │   ASP.NET Core Web API │ │
│  └──────────────────────┘   └────────────────────────┘ │
└─────────────────────────────────────────────────────────┘
                          │
┌─────────────────────────────────────────────────────────┐
│                    Application Layer                     │
│  ┌─────────────────────────────────────────────────┐   │
│  │      DTOs, Interfaces, Use Cases                │   │
│  └─────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────┘
                          │
┌─────────────────────────────────────────────────────────┐
│                   Infrastructure Layer                   │
│  ┌────────────────┐   ┌───────────────┐   ┌─────────┐ │
│  │ EF Core + SQLite│   │  Repositories  │   │ Services│ │
│  └────────────────┘   └───────────────┘   └─────────┘ │
└─────────────────────────────────────────────────────────┘
                          │
┌─────────────────────────────────────────────────────────┐
│                       Domain Layer                       │
│  ┌──────────────────────────────────────────────────┐  │
│  │  Entities: Event, Expense, Vendor, ApplicationUser│  │
│  └──────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
```

### Projects

- **BudgetEase.Core** - Domain entities, interfaces, and DTOs
- **BudgetEase.Infrastructure** - Data access, repositories, and services
- **BudgetEase.Api** - RESTful API controllers
- **BudgetEase.Web** - Blazor Server web application

## 🛠️ Technology Stack

| Component | Technology |
|-----------|-----------|
| **Frontend** | Blazor Server (ASP.NET Core 9.0) |
| **Backend API** | ASP.NET Core Web API |
| **Database** | SQLite (easily upgradable to SQL Server/PostgreSQL) |
| **ORM** | Entity Framework Core 9.0 |
| **Authentication** | ASP.NET Core Identity |
| **UI Framework** | Bootstrap 5 |
| **Icons** | Bootstrap Icons |
| **Email** | SendGrid (configured) |

## 🚀 Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- Git

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/rajibmahata/BudgetEase.git
   cd BudgetEase
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

4. **Apply database migrations**
   ```bash
   cd src/BudgetEase.Api
   dotnet ef database update
   ```

5. **Run the Web Application**
   ```bash
   cd ../BudgetEase.Web
   dotnet run
   ```

6. **Run the API** (in a separate terminal)
   ```bash
   cd src/BudgetEase.Api
   dotnet run
   ```

### Access the Application

- **Blazor Web App**: https://localhost:7001 or http://localhost:5001
- **API**: https://localhost:7002 or http://localhost:5002
- **API Documentation**: https://localhost:7002/openapi/v1.json

## 📖 Usage

### Creating an Event

1. Navigate to the **Events** page
2. Click **Create New Event**
3. Fill in the event details:
   - Event Name (e.g., "John's Wedding")
   - Type (Marriage, Birthday, Anniversary, or Custom)
   - Date and Venue
   - Budget Limit
4. Click **Create Event**

### Adding Expenses

1. Go to an event's details page
2. Click **Add Expense** in the Expenses section
3. Enter expense details:
   - Category (Food, Decoration, Photography, etc.)
   - Description
   - Estimated and Actual costs
   - Payment status
   - Optional: Link to a vendor
4. Save the expense

### Managing Vendors

1. Open an event's details page
2. Click **Add Vendor** in the Vendors section
3. Enter vendor information:
   - Name and Service Type
   - Contact details
   - Payment terms
   - Next reminder date
4. Save the vendor

## 📂 Project Structure

```
BudgetEase/
├── src/
│   ├── BudgetEase.Core/
│   │   ├── Entities/          # Domain models
│   │   ├── Interfaces/        # Repository interfaces
│   │   └── DTOs/              # Data transfer objects
│   │
│   ├── BudgetEase.Infrastructure/
│   │   ├── Data/              # DbContext and migrations
│   │   ├── Repositories/      # Repository implementations
│   │   └── Services/          # Infrastructure services
│   │
│   ├── BudgetEase.Api/
│   │   ├── Controllers/       # API endpoints
│   │   └── Program.cs         # API configuration
│   │
│   └── BudgetEase.Web/
│       ├── Components/
│       │   ├── Pages/         # Blazor pages
│       │   └── Layout/        # Layout components
│       └── Program.cs         # Web app configuration
│
├── ARCHITECTURE.md            # Detailed architecture docs
├── README.md                  # This file
└── BudgetEase.sln            # Solution file
```

## 🗄️ Database Schema

### Core Tables

- **AspNetUsers** - User accounts (ASP.NET Identity)
- **Events** - Event information with budget tracking
- **Expenses** - Expense records with categorization
- **Vendors** - Vendor contact and service information
- **EventCollaborators** - Multi-user event access

### Relationships

```
Users ──< Events ──< Expenses
                 └──< Vendors ──< Expenses
         │
         └──< EventCollaborators >── Users
```

## 🔌 API Endpoints

### Events
- `GET /api/events` - List all events
- `GET /api/events/{id}` - Get event details
- `POST /api/events` - Create new event
- `PUT /api/events/{id}` - Update event
- `DELETE /api/events/{id}` - Delete event

### Expenses
- `GET /api/expenses/event/{eventId}` - List expenses for an event
- `POST /api/expenses` - Create expense
- `PUT /api/expenses/{id}` - Update expense
- `DELETE /api/expenses/{id}` - Delete expense

### Vendors
- `GET /api/vendors/event/{eventId}` - List vendors for an event
- `GET /api/vendors/reminders` - Get vendors with upcoming reminders
- `POST /api/vendors` - Create vendor
- `PUT /api/vendors/{id}` - Update vendor
- `DELETE /api/vendors/{id}` - Delete vendor

## 🔧 Configuration

### Database Connection

Edit `appsettings.json` in both API and Web projects:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=budgetease.db"
  }
}
```

### Email Configuration (SendGrid)

```json
{
  "SendGrid": {
    "ApiKey": "YOUR_SENDGRID_API_KEY_HERE"
  }
}
```

## 🧪 Testing

Run tests using:
```bash
dotnet test
```

## 📝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Built with [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet)
- UI powered by [Bootstrap](https://getbootstrap.com/)
- Icons from [Bootstrap Icons](https://icons.getbootstrap.com/)

## 📞 Support

For issues, questions, or suggestions:
- Open an [Issue](https://github.com/rajibmahata/BudgetEase/issues)
- Contact: rajibmahata@example.com

## 🗺️ Roadmap

### Version 1.0 (Current)
- ✅ Core event management
- ✅ Expense tracking
- ✅ Vendor management
- ✅ Basic dashboard

### Version 1.1 (Next Release)
- 📧 Email notifications
- 📊 Interactive charts
- 📄 PDF/Excel reports
- 🔐 Complete authentication UI

### Version 2.0 (Future)
- 🤖 AI-based budget prediction
- 📱 Progressive Web App (PWA)
- 🌐 Multi-language support
- 📎 File attachments (invoices/receipts)

---

**Made with ❤️ by Rajib Mahata**
