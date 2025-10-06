# BudgetEase - Complete User Flow Documentation

> **Authentication-First Architecture**: Every feature, page, and API call in BudgetEase requires user authentication. Nothing works without a valid logged-in session.

## �� Document Overview

This document provides a comprehensive user flow diagram and description for **BudgetEase**, a lightweight event expense tracker and manager built with ASP.NET Core and Blazor Server.

**Version:** 1.0  
**Last Updated:** 2024  
**Status:** Production Ready Documentation

---

## 🎯 Key Authentication Rule

➡️ **CRITICAL:** *Nothing in the application works without user authentication.*

Every feature, page, or API call must require a **logged-in session** through:
- Email/Password authentication
- Google Sign-In (via ASP.NET Identity)
- Valid JWT token for API calls

**Invalid or expired session → automatic redirect to Login page**

---

## 📊 Complete User Flow Overview (Mermaid Diagram)

```mermaid
graph TD
    Start([User Opens App]) --> CheckAuth{Is User<br/>Authenticated?}
    
    CheckAuth -->|No| LoginPage[Login/Register Page]
    CheckAuth -->|Yes| Dashboard[Dashboard]
    
    LoginPage --> LoginOptions{Choose Login Method}
    
    LoginOptions --> EmailLogin[Email + Password Login]
    LoginOptions --> GoogleLogin[Google OAuth Login]
    LoginOptions --> RegisterNew[Register New Account]
    LoginOptions --> ForgotPwd[Forgot Password]
    
    EmailLogin --> ValidateCredentials{Valid<br/>Credentials?}
    GoogleLogin --> GoogleAuth[Google OAuth Flow]
    GoogleAuth --> ValidateGoogle{Google Auth<br/>Success?}
    
    RegisterNew --> RegForm[Fill Registration Form]
    RegForm --> CreateAccount[Create Account]
    CreateAccount --> VerifyEmail{Email<br/>Verification}
    
    ForgotPwd --> SendOTP[Send OTP/Email Link]
    SendOTP --> ResetPwd[Reset Password]
    ResetPwd --> LoginPage
    
    ValidateCredentials -->|Invalid| LoginError[Show Error Message]
    LoginError --> LoginPage
    ValidateCredentials -->|Valid| CreateSession[Create Session/Token]
    ValidateGoogle -->|Failed| LoginError
    ValidateGoogle -->|Success| CreateSession
    VerifyEmail -->|Verified| CreateSession
    
    CreateSession --> Dashboard
    
    Dashboard --> DashboardActions{User Action}
    
    DashboardActions --> ViewEvents[View Events]
    DashboardActions --> ViewExpenses[View Expenses]
    DashboardActions --> ViewVendors[View Vendors]
    DashboardActions --> ViewReports[View Reports]
    DashboardActions --> ViewNotifications[View Notifications]
    DashboardActions --> UserSettings[User Settings]
    DashboardActions --> Logout[Logout]
    
    ViewEvents --> EventsList[Events List Page]
    EventsList --> EventActions{Event Action}
    EventActions --> CreateEvent[Create New Event]
    EventActions --> EditEvent[Edit Event]
    EventActions --> DeleteEvent[Delete Event]
    EventActions --> SelectEvent[Select Active Event]
    
    SelectEvent --> EventContext[Set Active Event Context]
    EventContext --> Dashboard
    
    CreateEvent --> EventForm[Event Form]
    EventForm --> SaveEvent[Save Event]
    SaveEvent --> CheckEventAuth{User is<br/>Event Owner?}
    CheckEventAuth -->|No| AccessDenied[Access Denied]
    CheckEventAuth -->|Yes| EventsList
    
    ViewExpenses --> CheckActiveEvent{Active Event<br/>Selected?}
    CheckActiveEvent -->|No| SelectEventPrompt[Prompt: Select Event First]
    SelectEventPrompt --> EventsList
    CheckActiveEvent -->|Yes| ExpensesList[Expenses List]
    
    ExpensesList --> ExpenseActions{Expense Action}
    ExpenseActions --> AddExpense[Add Expense]
    ExpenseActions --> EditExpense[Edit Expense]
    ExpenseActions --> DeleteExpense[Delete Expense]
    ExpenseActions --> FilterExpenses[Filter/Sort]
    
    AddExpense --> ExpenseForm[Expense Form]
    ExpenseForm --> CheckBudget{Budget Check}
    CheckBudget --> SaveExpense[Save Expense]
    SaveExpense --> UpdateBudget[Update Budget Summary]
    UpdateBudget --> CheckThreshold{Budget > 80%<br/>Used?}
    CheckThreshold -->|Yes| BudgetAlert[Send Budget Alert]
    CheckThreshold -->|No| ExpensesList
    BudgetAlert --> ExpensesList
    
    ViewVendors --> CheckActiveEvent2{Active Event<br/>Selected?}
    CheckActiveEvent2 -->|No| SelectEventPrompt
    CheckActiveEvent2 -->|Yes| VendorsList[Vendors List]
    
    VendorsList --> VendorActions{Vendor Action}
    VendorActions --> AddVendor[Add Vendor]
    VendorActions --> EditVendor[Edit Vendor]
    VendorActions --> DeleteVendor[Delete Vendor]
    VendorActions --> SetReminder[Set Reminder]
    
    SetReminder --> ReminderForm[Reminder Form]
    ReminderForm --> SaveReminder[Save Reminder]
    SaveReminder --> ScheduleJob[Schedule Background Job]
    ScheduleJob --> VendorsList
    
    ViewReports --> ReportOptions{Report Type}
    ReportOptions --> BudgetReport[Budget vs Actual Report]
    ReportOptions --> CategoryReport[Category-wise Report]
    ReportOptions --> VendorReport[Vendor-wise Report]
    
    BudgetReport --> ReportFormat{Export Format}
    CategoryReport --> ReportFormat
    VendorReport --> ReportFormat
    
    ReportFormat --> ExportPDF[Export as PDF]
    ReportFormat --> ExportExcel[Export as Excel]
    ReportFormat --> EmailReport[Email Report]
    
    ExportPDF --> DownloadFile[Download File]
    ExportExcel --> DownloadFile
    EmailReport --> SendEmail[Send via Email]
    SendEmail --> ReportSuccess[Success Message]
    
    ViewNotifications --> NotificationsList[Notifications List]
    NotificationsList --> NotifActions{Notification Action}
    NotifActions --> MarkRead[Mark as Read]
    NotifActions --> ViewDetails[View Details]
    
    Logout --> ClearSession[Clear Session/Token]
    ClearSession --> LoginPage
    
    AccessDenied --> Dashboard
    DownloadFile --> Dashboard
    ReportSuccess --> Dashboard
    
    %% Session Timeout Path
    Dashboard -.->|Session Timeout| SessionExpired[Session Expired]
    EventsList -.->|Session Timeout| SessionExpired
    ExpensesList -.->|Session Timeout| SessionExpired
    VendorsList -.->|Session Timeout| SessionExpired
    SessionExpired --> LoginPage
    
    style CheckAuth fill:#ff6b6b,stroke:#c92a2a,stroke-width:3px,color:#fff
    style CreateSession fill:#51cf66,stroke:#2f9e44,stroke-width:3px,color:#fff
    style CheckActiveEvent fill:#ffd43b,stroke:#fab005,stroke-width:2px
    style CheckActiveEvent2 fill:#ffd43b,stroke:#fab005,stroke-width:2px
    style SessionExpired fill:#ff6b6b,stroke:#c92a2a,stroke-width:3px,color:#fff
    style Dashboard fill:#339af0,stroke:#1971c2,stroke-width:2px,color:#fff
```

---

## 1️⃣ Authentication Flow

### Overview
The authentication system is the gateway to all application features. Users must authenticate before accessing any functionality.

### Flow Diagram

```mermaid
sequenceDiagram
    actor User
    participant Browser
    participant LoginPage
    participant AuthAPI
    participant Database
    participant GoogleOAuth
    participant Dashboard

    User->>Browser: Open Application
    Browser->>LoginPage: Load Landing Page
    LoginPage->>LoginPage: Check Session
    
    alt No Valid Session
        LoginPage-->>User: Display Login/Register Options
        
        alt Email Login
            User->>LoginPage: Enter Email + Password
            LoginPage->>AuthAPI: POST /api/auth/login
            AuthAPI->>Database: Validate Credentials
            Database-->>AuthAPI: User Data
            AuthAPI->>AuthAPI: Generate JWT Token
            AuthAPI-->>LoginPage: Return Token + User Info
            LoginPage->>Browser: Store Token (HttpOnly Cookie)
            Browser->>Dashboard: Redirect to Dashboard
        end
        
        alt Google OAuth
            User->>LoginPage: Click "Sign in with Google"
            LoginPage->>GoogleOAuth: Redirect to Google
            GoogleOAuth-->>User: Google Login Page
            User->>GoogleOAuth: Enter Google Credentials
            GoogleOAuth->>AuthAPI: Return Auth Code
            AuthAPI->>GoogleOAuth: Exchange Code for Token
            GoogleOAuth-->>AuthAPI: Google User Profile
            AuthAPI->>Database: Create/Update User
            AuthAPI->>AuthAPI: Generate JWT Token
            AuthAPI-->>LoginPage: Return Token + User Info
            LoginPage->>Browser: Store Token
            Browser->>Dashboard: Redirect to Dashboard
        end
        
        alt Register New Account
            User->>LoginPage: Click "Sign Up"
            Browser->>LoginPage: Load Register Form
            User->>LoginPage: Enter Details (Name, Email, Password)
            LoginPage->>AuthAPI: POST /api/auth/register
            AuthAPI->>Database: Check Email Exists
            alt Email Already Exists
                Database-->>AuthAPI: Email Taken
                AuthAPI-->>LoginPage: Error: Email Already Registered
                LoginPage-->>User: Show Error Message
            else New Email
                AuthAPI->>Database: Create User Account
                AuthAPI->>AuthAPI: Send Verification Email (Optional)
                AuthAPI->>AuthAPI: Generate JWT Token
                AuthAPI-->>LoginPage: Return Token + User Info
                LoginPage->>Browser: Store Token
                Browser->>Dashboard: Redirect to Dashboard
            end
        end
        
        alt Forgot Password
            User->>LoginPage: Click "Forgot Password"
            LoginPage-->>User: Show Email Input
            User->>LoginPage: Enter Email
            LoginPage->>AuthAPI: POST /api/auth/forgot-password
            AuthAPI->>Database: Check Email Exists
            alt Email Exists
                AuthAPI->>AuthAPI: Generate OTP/Reset Token
                AuthAPI->>AuthAPI: Send Reset Email
                AuthAPI-->>LoginPage: Success Message
                LoginPage-->>User: "Check your email"
                User->>User: Open Email
                User->>Browser: Click Reset Link
                Browser->>LoginPage: Load Reset Password Page
                User->>LoginPage: Enter New Password
                LoginPage->>AuthAPI: POST /api/auth/reset-password
                AuthAPI->>Database: Update Password
                AuthAPI-->>LoginPage: Success
                LoginPage-->>User: "Password Reset Complete"
                User->>LoginPage: Return to Login
            end
        end
    else Valid Session Exists
        LoginPage->>Dashboard: Auto-redirect to Dashboard
    end
```

### API Endpoints

#### POST /api/auth/register
**Request:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "password": "SecurePass123!",
  "confirmPassword": "SecurePass123!"
}
```

**Response (201 Created):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "userId": "user-uuid-123",
  "email": "john.doe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "expiresIn": 3600
}
```

#### POST /api/auth/login
**Request:**
```json
{
  "email": "john.doe@example.com",
  "password": "SecurePass123!"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "userId": "user-uuid-123",
  "email": "john.doe@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "expiresIn": 3600
}
```

**Response (401 Unauthorized):**
```json
{
  "error": "Invalid email or password"
}
```

#### POST /api/auth/forgot-password
**Request:**
```json
{
  "email": "john.doe@example.com"
}
```

**Response (200 OK):**
```json
{
  "message": "If the email exists, a password reset link has been sent."
}
```

#### POST /api/auth/reset-password
**Request:**
```json
{
  "email": "john.doe@example.com",
  "token": "reset-token-123",
  "newPassword": "NewSecurePass123!",
  "confirmPassword": "NewSecurePass123!"
}
```

**Response (200 OK):**
```json
{
  "message": "Password has been reset successfully."
}
```

#### POST /api/auth/logout
**Request Headers:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

**Response (200 OK):**
```json
{
  "message": "Logged out successfully"
}
```

### Authentication Checkpoints

Every page and API endpoint must validate authentication:

1. **Frontend (Blazor Pages)**
   - Check for valid token in browser storage
   - Redirect to `/login` if not authenticated
   - Use `[Authorize]` attribute on protected pages

2. **Backend (API Controllers)**
   - Validate JWT token from Authorization header
   - Return 401 Unauthorized if invalid
   - Use `[Authorize]` attribute on all controllers except AuthController

3. **Session Management**
   - Token expiration: 1 hour (configurable)
   - Refresh token: Optional implementation
   - Session timeout: Auto-logout after inactivity

### Role-Based Access Control

| Role | Permissions |
|------|-------------|
| **Event Owner** | Full access to their events (CRUD) |
| **Event Owner** | Can add/remove collaborators |
| **Event Owner** | Can delete events |
| **Collaborator** | Read-only access to event details |
| **Collaborator** | Can add/edit expenses (if granted) |
| **Collaborator** | Cannot delete events |
| **Collaborator** | Cannot manage other collaborators |

---

## 2️⃣ Dashboard Flow

### Overview
The Dashboard is the central hub of BudgetEase, accessible only after successful authentication. It provides a comprehensive overview of user's events, expenses, and budget status.

### Page Components

1. **Summary Cards**
   - Total Events
   - Total Budget Allocated
   - Total Spent
   - Remaining Budget
   - Budget Utilization %

2. **Charts & Visualizations**
   - Category-wise expense breakdown (Pie Chart)
   - Monthly spending trend (Line Chart)
   - Event-wise budget comparison (Bar Chart)

3. **Quick Actions**
   - Create New Event
   - Add Expense
   - View Reports
   - Manage Vendors

4. **Recent Activity Feed**
   - Latest expenses added
   - Upcoming reminders
   - Budget alerts

5. **Upcoming Events**
   - Events within next 30 days
   - Event countdown

### Flow Diagram

```mermaid
flowchart TD
    Auth[User Authenticated] --> LoadDash[Load Dashboard]
    LoadDash --> AuthCheck{Verify Token}
    
    AuthCheck -->|Invalid/Expired| Redirect[Redirect to Login]
    AuthCheck -->|Valid| FetchData[Fetch Dashboard Data]
    
    FetchData --> API1[GET /api/events]
    FetchData --> API2[GET /api/expenses/summary]
    FetchData --> API3[GET /api/vendors/reminders]
    
    API1 --> RenderDash[Render Dashboard]
    API2 --> RenderDash
    API3 --> RenderDash
    
    RenderDash --> UserAction{User Clicks}
    
    UserAction --> CreateEvent[Create Event Button]
    UserAction --> ViewEvent[View Event Card]
    UserAction --> AddExpense[Quick Add Expense]
    UserAction --> ViewReports[View Reports]
    UserAction --> ViewVendors[Manage Vendors]
    UserAction --> Notifications[View Notifications]
    UserAction --> Profile[User Profile]
    
    CreateEvent --> EventForm[Event Creation Form]
    ViewEvent --> EventDetails[Event Details Page]
    AddExpense --> ExpenseForm[Expense Form]
    ViewReports --> ReportsPage[Reports Page]
    ViewVendors --> VendorsPage[Vendors Page]
    Notifications --> NotifPage[Notifications Page]
    Profile --> SettingsPage[Settings Page]
    
    EventForm --> RenderDash
    EventDetails --> RenderDash
    ExpenseForm --> RenderDash
    ReportsPage --> RenderDash
    VendorsPage --> RenderDash
    NotifPage --> RenderDash
    SettingsPage --> RenderDash
    
    style AuthCheck fill:#ff6b6b,stroke:#c92a2a,stroke-width:2px
    style RenderDash fill:#51cf66,stroke:#2f9e44,stroke-width:2px
```

### API Endpoints Used

#### GET /api/dashboard/summary
**Headers:**
```
Authorization: Bearer <token>
```

**Response (200 OK):**
```json
{
  "totalEvents": 5,
  "totalBudget": 50000.00,
  "totalSpent": 35000.00,
  "remainingBudget": 15000.00,
  "budgetUtilization": 70.0,
  "activeEvents": 3,
  "upcomingEvents": [
    {
      "id": 1,
      "name": "Wedding Ceremony",
      "eventDate": "2024-12-15T00:00:00Z",
      "daysUntil": 45,
      "budgetLimit": 30000.00,
      "totalSpent": 22000.00
    }
  ],
  "recentExpenses": [
    {
      "id": 101,
      "eventName": "Wedding Ceremony",
      "category": "Venue",
      "description": "Venue Booking Deposit",
      "actualCost": 5000.00,
      "createdAt": "2024-10-05T10:30:00Z"
    }
  ],
  "upcomingReminders": [
    {
      "vendorName": "Catering Services Inc",
      "reminderDate": "2024-10-10T00:00:00Z",
      "reminderType": "Payment Due"
    }
  ]
}
```

#### GET /api/dashboard/charts/category-spending
**Headers:**
```
Authorization: Bearer <token>
```

**Query Parameters:**
```
?eventId=1  // Optional: filter by specific event
```

**Response (200 OK):**
```json
{
  "categories": [
    {
      "category": "Venue",
      "totalSpent": 8000.00,
      "percentage": 32.0
    },
    {
      "category": "Catering",
      "totalSpent": 10000.00,
      "percentage": 40.0
    },
    {
      "category": "Decoration",
      "totalSpent": 4000.00,
      "percentage": 16.0
    },
    {
      "category": "Entertainment",
      "totalSpent": 3000.00,
      "percentage": 12.0
    }
  ],
  "totalSpent": 25000.00
}
```

### Navigation Links

| Link | Route | Description | Auth Required |
|------|-------|-------------|---------------|
| Dashboard | `/` or `/dashboard` | Main dashboard page | ✅ Yes |
| Events | `/events` | Events list | ✅ Yes |
| New Event | `/events/create` | Create event form | ✅ Yes |
| Event Details | `/events/{id}` | Single event view | ✅ Yes (+ Owner/Collab) |
| Expenses | `/expenses` | Expenses list | ✅ Yes |
| Vendors | `/vendors` | Vendors list | ✅ Yes |
| Reports | `/reports` | Reports page | ✅ Yes |
| Notifications | `/notifications` | Notifications center | ✅ Yes |
| Settings | `/settings` | User settings | ✅ Yes |
| Profile | `/profile` | User profile | ✅ Yes |
| Logout | `/logout` | Logout action | ✅ Yes |

### Authentication Check on Dashboard

Every time the dashboard loads or user performs an action:

```csharp
// Pseudo-code for authentication check
public async Task OnInitializedAsync()
{
    // Check if user is authenticated
    var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
    var user = authState.User;
    
    if (!user.Identity?.IsAuthenticated ?? false)
    {
        // Redirect to login
        NavigationManager.NavigateTo("/login");
        return;
    }
    
    // Fetch dashboard data
    await LoadDashboardData();
}

// Every API call includes authentication token
httpClient.DefaultRequestHeaders.Authorization = 
    new AuthenticationHeaderValue("Bearer", token);
```

---

## 3️⃣ Event Management Flow

### Overview
Event Management allows users to create, view, edit, and delete events. Each event serves as a container for expenses and vendors. Users can also manage collaborators for shared event access.

### Prerequisites
- ✅ User must be authenticated
- ✅ Valid session token required

### Flow Diagram

```mermaid
stateDiagram-v2
    [*] --> AuthCheck: User Accesses Events
    AuthCheck --> Login: Not Authenticated
    AuthCheck --> EventsList: Authenticated
    
    EventsList --> CreateEvent: Click "New Event"
    EventsList --> ViewEvent: Click Event Card
    EventsList --> FilterEvents: Apply Filters
    
    CreateEvent --> EventForm: Show Form
    EventForm --> ValidateForm: Submit Form
    ValidateForm --> CreateAPI: Valid
    ValidateForm --> EventForm: Invalid (Show Errors)
    CreateAPI --> EventsList: Success
    CreateAPI --> EventForm: API Error
    
    ViewEvent --> EventDetails: Load Event
    EventDetails --> EditEvent: Click Edit
    EventDetails --> DeleteEvent: Click Delete
    EventDetails --> ManageCollabs: Manage Collaborators
    EventDetails --> ViewExpenses: View Expenses Tab
    EventDetails --> ViewVendors: View Vendors Tab
    
    EditEvent --> EventForm: Load Form with Data
    
    DeleteEvent --> ConfirmDelete: Show Confirmation
    ConfirmDelete --> DeleteAPI: Confirm
    ConfirmDelete --> EventDetails: Cancel
    DeleteAPI --> EventsList: Success
    
    ManageCollabs --> AddCollab: Add Collaborator
    ManageCollabs --> RemoveCollab: Remove Collaborator
    AddCollab --> EventDetails: Success
    RemoveCollab --> EventDetails: Success
    
    FilterEvents --> EventsList: Apply Filters
    
    EventsList --> [*]: Logout/Navigate Away
    
    Login --> [*]
```

### User Actions

#### 1. View Events List

**Route:** `/events`  
**Auth Required:** ✅ Yes

**API Call:**
```http
GET /api/events HTTP/1.1
Authorization: Bearer <token>
```

**Response:**
```json
{
  "events": [
    {
      "id": 1,
      "name": "Wedding Ceremony",
      "type": "Wedding",
      "eventDate": "2024-12-15T00:00:00Z",
      "venue": "Grand Ballroom Hotel",
      "budgetLimit": 30000.00,
      "totalSpent": 22000.00,
      "remainingBudget": 8000.00,
      "budgetUtilization": 73.33,
      "isOwner": true,
      "createdAt": "2024-09-01T00:00:00Z"
    },
    {
      "id": 2,
      "name": "Birthday Party",
      "type": "Birthday",
      "eventDate": "2024-11-20T00:00:00Z",
      "venue": "Home",
      "budgetLimit": 5000.00,
      "totalSpent": 2500.00,
      "remainingBudget": 2500.00,
      "budgetUtilization": 50.0,
      "isOwner": true,
      "createdAt": "2024-10-01T00:00:00Z"
    }
  ],
  "totalCount": 2
}
```

#### 2. Create New Event

**Route:** `/events/create`  
**Auth Required:** ✅ Yes

**API Call:**
```http
POST /api/events HTTP/1.1
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Anniversary Celebration",
  "type": "Anniversary",
  "eventDate": "2025-02-14T18:00:00Z",
  "venue": "Beachside Resort",
  "budgetLimit": 15000.00
}
```

**Response (201 Created):**
```json
{
  "id": 3,
  "name": "Anniversary Celebration",
  "type": "Anniversary",
  "eventDate": "2025-02-14T18:00:00Z",
  "venue": "Beachside Resort",
  "budgetLimit": 15000.00,
  "totalSpent": 0.00,
  "remainingBudget": 15000.00,
  "budgetUtilization": 0.0,
  "ownerId": "user-uuid-123",
  "createdAt": "2024-10-06T14:30:00Z"
}
```

#### 3. View Event Details

**Route:** `/events/{id}`  
**Auth Required:** ✅ Yes + Event Access (Owner or Collaborator)

**API Call:**
```http
GET /api/events/1 HTTP/1.1
Authorization: Bearer <token>
```

**Response (200 OK):**
```json
{
  "id": 1,
  "name": "Wedding Ceremony",
  "type": "Wedding",
  "eventDate": "2024-12-15T00:00:00Z",
  "venue": "Grand Ballroom Hotel",
  "budgetLimit": 30000.00,
  "totalSpent": 22000.00,
  "remainingBudget": 8000.00,
  "budgetUtilization": 73.33,
  "ownerId": "user-uuid-123",
  "ownerName": "John Doe",
  "isOwner": true,
  "collaborators": [
    {
      "userId": "user-uuid-456",
      "userName": "Jane Smith",
      "email": "jane.smith@example.com",
      "addedAt": "2024-09-05T00:00:00Z"
    }
  ],
  "expenseCount": 8,
  "vendorCount": 5,
  "createdAt": "2024-09-01T00:00:00Z",
  "updatedAt": "2024-10-05T10:30:00Z"
}
```

**Response (403 Forbidden):**
```json
{
  "error": "You do not have permission to access this event."
}
```

#### 4. Edit Event

**Route:** `/events/{id}/edit`  
**Auth Required:** ✅ Yes + Owner Only

**API Call:**
```http
PUT /api/events/1 HTTP/1.1
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Wedding Ceremony - Updated",
  "type": "Wedding",
  "eventDate": "2024-12-20T00:00:00Z",
  "venue": "Grand Ballroom Hotel - Main Hall",
  "budgetLimit": 35000.00
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "name": "Wedding Ceremony - Updated",
  "type": "Wedding",
  "eventDate": "2024-12-20T00:00:00Z",
  "venue": "Grand Ballroom Hotel - Main Hall",
  "budgetLimit": 35000.00,
  "totalSpent": 22000.00,
  "remainingBudget": 13000.00,
  "budgetUtilization": 62.86,
  "updatedAt": "2024-10-06T15:00:00Z"
}
```

#### 5. Delete Event

**Route:** `/events/{id}`  
**Auth Required:** ✅ Yes + Owner Only

**API Call:**
```http
DELETE /api/events/1 HTTP/1.1
Authorization: Bearer <token>
```

**Response (204 No Content)**

**Response (403 Forbidden):**
```json
{
  "error": "Only the event owner can delete this event."
}
```

#### 6. Manage Collaborators

##### Add Collaborator

**API Call:**
```http
POST /api/events/1/collaborators HTTP/1.1
Authorization: Bearer <token>
Content-Type: application/json

{
  "email": "collaborator@example.com"
}
```

**Response (201 Created):**
```json
{
  "userId": "user-uuid-789",
  "userName": "Collaborator Name",
  "email": "collaborator@example.com",
  "addedAt": "2024-10-06T15:30:00Z"
}
```

##### Remove Collaborator

**API Call:**
```http
DELETE /api/events/1/collaborators/user-uuid-789 HTTP/1.1
Authorization: Bearer <token>
```

**Response (204 No Content)**

#### 7. Select Active Event

When a user selects an event as "active", it sets the context for expenses and vendors:

**Local Storage:**
```javascript
localStorage.setItem('activeEventId', '1');
localStorage.setItem('activeEventName', 'Wedding Ceremony');
```

This active event is used for:
- Filtering expenses
- Filtering vendors
- Adding new expenses/vendors

### Access Control Matrix

| Action | Owner | Collaborator | Other Users |
|--------|-------|--------------|-------------|
| View Event List | ✅ Yes | ✅ Yes (only shared events) | ✅ Yes (only own events) |
| Create Event | ✅ Yes | ✅ Yes | ✅ Yes |
| View Event Details | ✅ Yes | ✅ Yes | ❌ No (403) |
| Edit Event | ✅ Yes | ❌ No (403) | ❌ No (403) |
| Delete Event | ✅ Yes | ❌ No (403) | ❌ No (403) |
| Add Collaborator | ✅ Yes | ❌ No (403) | ❌ No (403) |
| Remove Collaborator | ✅ Yes | ❌ No (403) | ❌ No (403) |
| View Expenses | ✅ Yes | ✅ Yes | ❌ No (403) |
| Add Expense | ✅ Yes | ⚠️ Conditional | ❌ No (403) |

---

## 4️⃣ Expense Tracker Flow

### Overview
The Expense Tracker allows users to record and manage expenses for their events. It tracks estimated vs. actual costs, payment status, and automatically updates budget summaries.

### Prerequisites
- ✅ User must be authenticated
- ✅ An active event must be selected
- ✅ User must have access to the event (Owner or Collaborator)

### Flow Diagram

```mermaid
sequenceDiagram
    actor User
    participant ExpensePage
    participant EventContext
    participant ExpenseAPI
    participant Database
    participant NotificationService

    User->>ExpensePage: Navigate to Expenses
    ExpensePage->>ExpensePage: Check Authentication
    
    alt Not Authenticated
        ExpensePage->>User: Redirect to Login
    else Authenticated
        ExpensePage->>EventContext: Check Active Event
        
        alt No Active Event Selected
            EventContext-->>User: Prompt "Please Select Event First"
            User->>ExpensePage: Select Event from Dropdown
            ExpensePage->>EventContext: Set Active Event
        end
        
        EventContext-->>ExpensePage: Active Event: "Wedding"
        ExpensePage->>ExpenseAPI: GET /api/expenses/event/1
        ExpenseAPI->>ExpenseAPI: Validate Token
        ExpenseAPI->>Database: Fetch Expenses for Event 1
        Database-->>ExpenseAPI: Expense List
        ExpenseAPI-->>ExpensePage: Return Expenses
        ExpensePage-->>User: Display Expenses List
        
        User->>ExpensePage: Click "Add Expense"
        ExpensePage-->>User: Show Expense Form
        
        User->>ExpensePage: Fill Form & Submit
        ExpensePage->>ExpenseAPI: POST /api/expenses
        ExpenseAPI->>Database: Create Expense
        Database-->>ExpenseAPI: Expense Created
        
        ExpenseAPI->>Database: Calculate Total Spent
        Database-->>ExpenseAPI: Total Spent = $23,000
        
        ExpenseAPI->>ExpenseAPI: Calculate Budget Usage
        Note over ExpenseAPI: Budget Usage = ($23,000 / $30,000) * 100 = 76.67%
        
        alt Budget Usage > 80%
            ExpenseAPI->>NotificationService: Trigger Budget Alert
            NotificationService->>NotificationService: Send Email Alert
            NotificationService-->>User: "Warning: 80% Budget Used"
        end
        
        ExpenseAPI-->>ExpensePage: Expense Created Successfully
        ExpensePage->>ExpenseAPI: GET /api/expenses/event/1
        ExpenseAPI-->>ExpensePage: Updated Expense List
        ExpensePage-->>User: Display Updated List
    end
```

### User Actions

#### 1. View Expenses for Event

**Route:** `/expenses?eventId=1`  
**Auth Required:** ✅ Yes + Event Access

**API Call:**
```http
GET /api/expenses/event/1 HTTP/1.1
Authorization: Bearer <token>
```

**Response (200 OK):**
```json
{
  "eventId": 1,
  "eventName": "Wedding Ceremony",
  "budgetLimit": 30000.00,
  "totalSpent": 22000.00,
  "remainingBudget": 8000.00,
  "budgetUtilization": 73.33,
  "expenses": [
    {
      "id": 101,
      "category": "Venue",
      "description": "Venue Booking Deposit",
      "vendorId": 5,
      "vendorName": "Grand Ballroom Hotel",
      "estimatedCost": 10000.00,
      "actualCost": 10000.00,
      "paymentStatus": "Paid",
      "paymentDueDate": "2024-09-15T00:00:00Z",
      "createdAt": "2024-09-10T00:00:00Z"
    },
    {
      "id": 102,
      "category": "Catering",
      "description": "Catering Services - Per Plate",
      "vendorId": 3,
      "vendorName": "Delicious Catering Co",
      "estimatedCost": 15000.00,
      "actualCost": 12000.00,
      "paymentStatus": "Pending",
      "paymentDueDate": "2024-12-10T00:00:00Z",
      "createdAt": "2024-09-12T00:00:00Z"
    }
  ],
  "categoryBreakdown": [
    {
      "category": "Venue",
      "totalSpent": 10000.00,
      "percentage": 45.45
    },
    {
      "category": "Catering",
      "totalSpent": 12000.00,
      "percentage": 54.55
    }
  ]
}
```

**Response (403 Forbidden):**
```json
{
  "error": "You do not have access to this event's expenses."
}
```

#### 2. Add New Expense

**Route:** `/expenses/create?eventId=1`  
**Auth Required:** ✅ Yes + Event Access

**API Call:**
```http
POST /api/expenses HTTP/1.1
Authorization: Bearer <token>
Content-Type: application/json

{
  "eventId": 1,
  "category": "Decoration",
  "description": "Floral Arrangements",
  "vendorId": 7,
  "estimatedCost": 3000.00,
  "actualCost": 2800.00,
  "paymentStatus": "Paid",
  "paymentDueDate": "2024-11-01T00:00:00Z"
}
```

**Response (201 Created):**
```json
{
  "id": 103,
  "eventId": 1,
  "category": "Decoration",
  "description": "Floral Arrangements",
  "vendorId": 7,
  "vendorName": "Flower Paradise",
  "estimatedCost": 3000.00,
  "actualCost": 2800.00,
  "paymentStatus": "Paid",
  "paymentDueDate": "2024-11-01T00:00:00Z",
  "createdAt": "2024-10-06T16:00:00Z",
  "budgetAlert": false,
  "newBudgetUtilization": 75.93
}
```

**Response with Budget Alert (201 Created):**
```json
{
  "id": 104,
  "eventId": 1,
  "category": "Entertainment",
  "description": "DJ Services",
  "estimatedCost": 5000.00,
  "actualCost": 5000.00,
  "paymentStatus": "Pending",
  "createdAt": "2024-10-06T16:30:00Z",
  "budgetAlert": true,
  "alertMessage": "Warning: You have used 85% of your event budget!",
  "newBudgetUtilization": 85.0,
  "remainingBudget": 4500.00
}
```

#### 3. Edit Expense

**Route:** `/expenses/{id}/edit`  
**Auth Required:** ✅ Yes + Event Access

**API Call:**
```http
PUT /api/expenses/103 HTTP/1.1
Authorization: Bearer <token>
Content-Type: application/json

{
  "category": "Decoration",
  "description": "Floral Arrangements - Premium Package",
  "vendorId": 7,
  "estimatedCost": 3500.00,
  "actualCost": 3200.00,
  "paymentStatus": "Paid",
  "paymentDueDate": "2024-11-01T00:00:00Z"
}
```

**Response (200 OK):**
```json
{
  "id": 103,
  "category": "Decoration",
  "description": "Floral Arrangements - Premium Package",
  "estimatedCost": 3500.00,
  "actualCost": 3200.00,
  "paymentStatus": "Paid",
  "updatedAt": "2024-10-06T17:00:00Z"
}
```

#### 4. Delete Expense

**Route:** `/expenses/{id}`  
**Auth Required:** ✅ Yes + Event Owner

**API Call:**
```http
DELETE /api/expenses/103 HTTP/1.1
Authorization: Bearer <token>
```

**Response (204 No Content)**

#### 5. Filter & Sort Expenses

**API Call with Filters:**
```http
GET /api/expenses/event/1?category=Catering&paymentStatus=Pending&sortBy=actualCost&sortOrder=desc HTTP/1.1
Authorization: Bearer <token>
```

**Query Parameters:**
- `category` - Filter by expense category
- `paymentStatus` - Filter by payment status (Paid, Pending, Partial)
- `vendorId` - Filter by specific vendor
- `sortBy` - Sort field (actualCost, createdAt, category)
- `sortOrder` - Sort direction (asc, desc)

### Budget Alert Thresholds

| Budget Usage | Alert Level | Action |
|--------------|-------------|--------|
| 0-50% | ✅ Safe | No alert |
| 51-75% | ⚠️ Warning | Optional notification |
| 76-90% | 🟠 High | Email notification |
| 91-100% | 🔴 Critical | Immediate email + In-app alert |
| >100% | 🚨 Over Budget | Block new expenses (optional) |

### Expense Categories

Predefined categories for consistency:

- Venue
- Catering
- Decoration
- Entertainment
- Photography
- Transportation
- Accommodation
- Gifts & Favors
- Stationery
- Miscellaneous

### Payment Status Options

- **Paid** - Full payment completed
- **Pending** - Payment not yet made
- **Partial** - Partial payment made

### Conditional Logic

```javascript
// Frontend validation before adding expense
function validateExpense(expense, event) {
  // Check if expense would exceed budget
  const newTotalSpent = event.totalSpent + expense.actualCost;
  const newUtilization = (newTotalSpent / event.budgetLimit) * 100;
  
  if (newUtilization > 100) {
    return {
      valid: false,
      warning: `This expense will exceed your budget by $${newTotalSpent - event.budgetLimit}`
    };
  }
  
  if (newUtilization > 90) {
    return {
      valid: true,
      warning: `Warning: This will use ${newUtilization.toFixed(2)}% of your budget!`
    };
  }
  
  return { valid: true };
}
```

### Access Control

| Action | Event Owner | Collaborator (with permissions) | Collaborator (read-only) | Other Users |
|--------|-------------|----------------------------------|--------------------------|-------------|
| View Expenses | ✅ Yes | ✅ Yes | ✅ Yes | ❌ No (403) |
| Add Expense | ✅ Yes | ✅ Yes | ❌ No (403) | ❌ No (403) |
| Edit Expense | ✅ Yes | ✅ Yes | ❌ No (403) | ❌ No (403) |
| Delete Expense | ✅ Yes | ❌ No (403) | ❌ No (403) | ❌ No (403) |
| Export Expenses | ✅ Yes | ✅ Yes | ✅ Yes | ❌ No (403) |

---

## 5️⃣ Vendor Management Flow

### Overview
Vendor Management enables users to track vendors associated with events, store contact information, manage payment terms, and schedule call/payment reminders.

### Prerequisites
- ✅ User must be authenticated
- ✅ An active event must be selected
- ✅ User must have access to the event

### Flow Diagram

```mermaid
flowchart TB
    Start([User Navigates to Vendors]) --> AuthCheck{Authenticated?}
    AuthCheck -->|No| Login[Redirect to Login]
    AuthCheck -->|Yes| EventCheck{Active Event<br/>Selected?}
    
    EventCheck -->|No| SelectEvent[Prompt: Select Event]
    SelectEvent --> EventList[Show Event Dropdown]
    EventList --> SetActive[Set Active Event]
    SetActive --> EventCheck
    
    EventCheck -->|Yes| LoadVendors[GET /api/vendors/event/{eventId}]
    LoadVendors --> AuthAPI{Validate Token<br/>& Access}
    AuthAPI -->|Invalid| Login
    AuthAPI -->|No Access| AccessDenied[403 Forbidden]
    AuthAPI -->|Valid| DisplayList[Display Vendors List]
    
    DisplayList --> UserAction{User Action}
    
    UserAction --> AddVendor[Add New Vendor]
    UserAction --> EditVendor[Edit Vendor]
    UserAction --> DeleteVendor[Delete Vendor]
    UserAction --> SetReminder[Set Reminder]
    UserAction --> ContactVendor[Contact Vendor]
    UserAction --> ViewReminders[View All Reminders]
    
    AddVendor --> VendorForm[Vendor Form]
    VendorForm --> SubmitVendor[POST /api/vendors]
    SubmitVendor --> ValidateData{Valid Data?}
    ValidateData -->|No| ShowErrors[Show Validation Errors]
    ShowErrors --> VendorForm
    ValidateData -->|Yes| SaveVendor[Save to Database]
    SaveVendor --> DisplayList
    
    EditVendor --> VendorForm
    VendorForm --> UpdateVendor[PUT /api/vendors/{id}]
    UpdateVendor --> SaveVendor
    
    DeleteVendor --> ConfirmDelete{Confirm Delete?}
    ConfirmDelete -->|No| DisplayList
    ConfirmDelete -->|Yes| CheckLinked{Has Linked<br/>Expenses?}
    CheckLinked -->|Yes| ShowWarning[Show Warning:<br/>Expenses will be unlinked]
    ShowWarning --> FinalConfirm{Proceed?}
    FinalConfirm -->|No| DisplayList
    FinalConfirm -->|Yes| DeleteAPI[DELETE /api/vendors/{id}]
    CheckLinked -->|No| DeleteAPI
    DeleteAPI --> DisplayList
    
    SetReminder --> ReminderForm[Reminder Form]
    ReminderForm --> SaveReminder[Update Vendor with Reminder Date]
    SaveReminder --> ScheduleJob[Schedule Background Job]
    ScheduleJob --> DisplayList
    
    ContactVendor --> ContactOptions{Contact Method}
    ContactOptions --> CallVendor[Click to Call]
    ContactOptions --> EmailVendor[Click to Email]
    CallVendor --> DisplayList
    EmailVendor --> DisplayList
    
    ViewReminders --> RemindersPage[GET /api/vendors/reminders]
    RemindersPage --> DisplayReminders[Show Upcoming Reminders]
    DisplayReminders --> DisplayList
    
    style AuthCheck fill:#ff6b6b,stroke:#c92a2a,stroke-width:2px
    style EventCheck fill:#ffd43b,stroke:#fab005,stroke-width:2px
    style AuthAPI fill:#ff6b6b,stroke:#c92a2a,stroke-width:2px
    style DisplayList fill:#51cf66,stroke:#2f9e44,stroke-width:2px
```

### User Actions

#### 1. View Vendors for Event

**Route:** `/vendors?eventId=1`  
**Auth Required:** ✅ Yes + Event Access

**API Call:**
```http
GET /api/vendors/event/1 HTTP/1.1
Authorization: Bearer <token>
```

**Response (200 OK):**
```json
{
  "eventId": 1,
  "eventName": "Wedding Ceremony",
  "vendors": [
    {
      "id": 5,
      "name": "Grand Ballroom Hotel",
      "serviceType": "Venue",
      "contactNumber": "+1-555-0101",
      "email": "info@grandballroom.com",
      "paymentTerms": "50% advance, 50% on event day",
      "nextReminderDate": "2024-12-10T00:00:00Z",
      "linkedExpensesCount": 2,
      "totalExpenseAmount": 10000.00,
      "createdAt": "2024-09-01T00:00:00Z"
    },
    {
      "id": 3,
      "name": "Delicious Catering Co",
      "serviceType": "Catering",
      "contactNumber": "+1-555-0202",
      "email": "orders@deliciouscatering.com",
      "paymentTerms": "30% advance, 70% after service",
      "nextReminderDate": "2024-11-15T00:00:00Z",
      "linkedExpensesCount": 1,
      "totalExpenseAmount": 12000.00,
      "createdAt": "2024-09-05T00:00:00Z"
    },
    {
      "id": 7,
      "name": "Flower Paradise",
      "serviceType": "Decoration",
      "contactNumber": "+1-555-0303",
      "email": "contact@flowerparadise.com",
      "paymentTerms": "Full payment in advance",
      "nextReminderDate": null,
      "linkedExpensesCount": 1,
      "totalExpenseAmount": 2800.00,
      "createdAt": "2024-09-08T00:00:00Z"
    }
  ],
  "totalVendors": 3
}
```

#### 2. Add New Vendor

**Route:** `/vendors/create?eventId=1`  
**Auth Required:** ✅ Yes + Event Access

**API Call:**
```http
POST /api/vendors HTTP/1.1
Authorization: Bearer <token>
Content-Type: application/json

{
  "eventId": 1,
  "name": "Melody Makers DJ Services",
  "serviceType": "Entertainment",
  "contactNumber": "+1-555-0404",
  "email": "bookings@melodymakers.com",
  "paymentTerms": "20% booking fee, 80% on event day",
  "nextReminderDate": "2024-12-01T00:00:00Z"
}
```

**Response (201 Created):**
```json
{
  "id": 8,
  "eventId": 1,
  "name": "Melody Makers DJ Services",
  "serviceType": "Entertainment",
  "contactNumber": "+1-555-0404",
  "email": "bookings@melodymakers.com",
  "paymentTerms": "20% booking fee, 80% on event day",
  "nextReminderDate": "2024-12-01T00:00:00Z",
  "createdAt": "2024-10-06T18:00:00Z"
}
```

#### 3. Edit Vendor

**Route:** `/vendors/{id}/edit`  
**Auth Required:** ✅ Yes + Event Access

**API Call:**
```http
PUT /api/vendors/8 HTTP/1.1
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Melody Makers DJ & Entertainment",
  "serviceType": "Entertainment",
  "contactNumber": "+1-555-0404",
  "email": "bookings@melodymakers.com",
  "paymentTerms": "25% booking fee, 75% on event day",
  "nextReminderDate": "2024-11-25T00:00:00Z"
}
```

**Response (200 OK):**
```json
{
  "id": 8,
  "name": "Melody Makers DJ & Entertainment",
  "serviceType": "Entertainment",
  "contactNumber": "+1-555-0404",
  "email": "bookings@melodymakers.com",
  "paymentTerms": "25% booking fee, 75% on event day",
  "nextReminderDate": "2024-11-25T00:00:00Z",
  "updatedAt": "2024-10-06T18:30:00Z"
}
```

#### 4. Delete Vendor

**Route:** `/vendors/{id}`  
**Auth Required:** ✅ Yes + Event Owner Only

**API Call:**
```http
DELETE /api/vendors/8 HTTP/1.1
Authorization: Bearer <token>
```

**Response (204 No Content)**

**Response (409 Conflict - if vendor has linked expenses):**
```json
{
  "error": "Cannot delete vendor with linked expenses. Please remove expense associations first or select a different vendor for those expenses.",
  "linkedExpensesCount": 2
}
```

#### 5. View Upcoming Reminders

**Route:** `/vendors/reminders`  
**Auth Required:** ✅ Yes

**API Call:**
```http
GET /api/vendors/reminders HTTP/1.1
Authorization: Bearer <token>
```

**Response (200 OK):**
```json
{
  "reminders": [
    {
      "vendorId": 3,
      "vendorName": "Delicious Catering Co",
      "serviceType": "Catering",
      "eventId": 1,
      "eventName": "Wedding Ceremony",
      "contactNumber": "+1-555-0202",
      "email": "orders@deliciouscatering.com",
      "reminderDate": "2024-10-10T00:00:00Z",
      "daysUntilReminder": 4,
      "reminderType": "Payment Due"
    },
    {
      "vendorId": 8,
      "vendorName": "Melody Makers DJ & Entertainment",
      "serviceType": "Entertainment",
      "eventId": 1,
      "eventName": "Wedding Ceremony",
      "contactNumber": "+1-555-0404",
      "email": "bookings@melodymakers.com",
      "reminderDate": "2024-11-25T00:00:00Z",
      "daysUntilReminder": 50,
      "reminderType": "Follow-up Call"
    }
  ],
  "totalReminders": 2
}
```

### Service Type Options

Predefined service types for consistency:

- Venue
- Catering
- Decoration
- Photography
- Videography
- Entertainment
- Transportation
- Accommodation
- Printing & Stationery
- Makeup & Styling
- Gifts & Favors
- Security
- Miscellaneous

### Reminder Types

- **Payment Due** - Upcoming payment deadline
- **Follow-up Call** - Scheduled vendor follow-up
- **Service Confirmation** - Confirm service details
- **Final Payment** - Final payment reminder
- **Custom** - User-defined reminder

### Background Job for Reminders

The system runs a background job that checks for upcoming reminders daily:

```csharp
// Background service pseudo-code
public class VendorReminderService : IHostedService
{
    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Run daily at 8 AM
            await CheckUpcomingReminders();
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
    
    private async Task CheckUpcomingReminders()
    {
        // Get reminders for next 3 days
        var reminders = await _vendorRepository
            .GetVendorsWithUpcomingRemindersAsync(daysAhead: 3);
        
        foreach (var vendor in reminders)
        {
            // Send notification
            await _notificationService.SendReminderEmail(
                to: vendor.OwnerId,
                subject: $"Reminder: Contact {vendor.Name}",
                body: $"You have a scheduled reminder for {vendor.Name} on {vendor.NextReminderDate}"
            );
        }
    }
}
```

### Contact Vendor Features

When viewing vendor details, users can:

1. **Click to Call**
   ```html
   <a href="tel:+15550404">+1-555-0404</a>
   ```

2. **Click to Email**
   ```html
   <a href="mailto:bookings@melodymakers.com">bookings@melodymakers.com</a>
   ```

3. **View on Map** (if address is stored)
   ```html
   <a href="https://maps.google.com/?q=vendor+address">View Location</a>
   ```

### Access Control

| Action | Event Owner | Collaborator | Other Users |
|--------|-------------|--------------|-------------|
| View Vendors | ✅ Yes | ✅ Yes | ❌ No (403) |
| Add Vendor | ✅ Yes | ✅ Yes | ❌ No (403) |
| Edit Vendor | ✅ Yes | ✅ Yes | ❌ No (403) |
| Delete Vendor | ✅ Yes | ❌ No (403) | ❌ No (403) |
| Set Reminders | ✅ Yes | ✅ Yes | ❌ No (403) |
| View Reminders | ✅ Yes | ✅ Yes (own events) | ✅ Yes (own events) |

---

## 6️⃣ Report / Balance Sheet Flow

### Overview
The Report module allows users to generate comprehensive financial reports showing budget vs. actual spending, with options to export as PDF or Excel, and email to themselves or collaborators.

### Prerequisites
- ✅ User must be authenticated
- ✅ User must have at least one event
- ✅ User must have access to the event being reported

### Flow Diagram

```mermaid
flowchart TD
    Start([User Clicks Reports]) --> AuthCheck{Authenticated?}
    AuthCheck -->|No| Login[Redirect to Login]
    AuthCheck -->|Yes| ReportsPage[Load Reports Page]
    
    ReportsPage --> SelectEvent[Select Event for Report]
    SelectEvent --> EventSelected{Event<br/>Selected?}
    EventSelected -->|No| ReportsPage
    EventSelected -->|Yes| ReportType{Choose Report Type}
    
    ReportType --> BudgetReport[Budget vs Actual Report]
    ReportType --> CategoryReport[Category-wise Report]
    ReportType --> VendorReport[Vendor-wise Report]
    ReportType --> DetailedReport[Detailed Expense Report]
    
    BudgetReport --> GenerateData[Fetch Report Data]
    CategoryReport --> GenerateData
    VendorReport --> GenerateData
    DetailedReport --> GenerateData
    
    GenerateData --> APICall[GET /api/reports/{eventId}]
    APICall --> ValidateAccess{User Has<br/>Event Access?}
    ValidateAccess -->|No| AccessDenied[403 Forbidden]
    ValidateAccess -->|Yes| ProcessData[Process & Format Data]
    
    ProcessData --> DisplayReport[Display Report Preview]
    DisplayReport --> UserAction{User Action}
    
    UserAction --> ExportPDF[Export as PDF]
    UserAction --> ExportExcel[Export as Excel]
    UserAction --> EmailReport[Email Report]
    UserAction --> Print[Print Report]
    UserAction --> ModifyFilters[Change Filters]
    
    ExportPDF --> GeneratePDF[POST /api/reports/pdf]
    GeneratePDF --> DownloadPDF[Download PDF File]
    DownloadPDF --> ReportsPage
    
    ExportExcel --> GenerateExcel[POST /api/reports/excel]
    GenerateExcel --> DownloadExcel[Download Excel File]
    DownloadExcel --> ReportsPage
    
    EmailReport --> EmailForm[Show Email Options]
    EmailForm --> EnterRecipients[Enter Email Addresses]
    EnterRecipients --> SendEmail[POST /api/reports/email]
    SendEmail --> EmailSent[Show Success Message]
    EmailSent --> ReportsPage
    
    Print --> BrowserPrint[Open Print Dialog]
    BrowserPrint --> ReportsPage
    
    ModifyFilters --> SelectEvent
    
    style AuthCheck fill:#ff6b6b,stroke:#c92a2a,stroke-width:2px
    style ValidateAccess fill:#ff6b6b,stroke:#c92a2a,stroke-width:2px
    style DisplayReport fill:#51cf66,stroke:#2f9e44,stroke-width:2px
```

### Report Types

#### 1. Budget vs Actual Report

Shows overall budget performance for an event.

**API Call:**
```http
GET /api/reports/budget-vs-actual/1 HTTP/1.1
Authorization: Bearer <token>
```

**Response (200 OK):**
```json
{
  "eventId": 1,
  "eventName": "Wedding Ceremony",
  "eventDate": "2024-12-15T00:00:00Z",
  "reportDate": "2024-10-06T19:00:00Z",
  "budgetSummary": {
    "totalBudget": 30000.00,
    "totalEstimated": 35000.00,
    "totalActual": 22000.00,
    "totalRemaining": 8000.00,
    "budgetUtilization": 73.33,
    "estimatedVsActual": -13000.00,
    "estimatedVsActualPercentage": -37.14
  },
  "categoryBreakdown": [
    {
      "category": "Venue",
      "estimated": 10000.00,
      "actual": 10000.00,
      "variance": 0.00,
      "variancePercentage": 0.0
    },
    {
      "category": "Catering",
      "estimated": 15000.00,
      "actual": 12000.00,
      "variance": -3000.00,
      "variancePercentage": -20.0
    }
  ],
  "paymentStatus": {
    "paid": 10000.00,
    "pending": 12000.00,
    "partial": 0.00
  }
}
```

#### 2. Category-wise Report

Detailed breakdown by expense category.

**API Call:**
```http
GET /api/reports/category-wise/1 HTTP/1.1
Authorization: Bearer <token>
```

**Response (200 OK):**
```json
{
  "eventId": 1,
  "eventName": "Wedding Ceremony",
  "reportDate": "2024-10-06T19:00:00Z",
  "categories": [
    {
      "category": "Venue",
      "expenseCount": 2,
      "totalEstimated": 10000.00,
      "totalActual": 10000.00,
      "percentageOfBudget": 33.33,
      "expenses": [
        {
          "description": "Venue Booking Deposit",
          "vendorName": "Grand Ballroom Hotel",
          "estimated": 10000.00,
          "actual": 10000.00,
          "paymentStatus": "Paid"
        }
      ]
    },
    {
      "category": "Catering",
      "expenseCount": 1,
      "totalEstimated": 15000.00,
      "totalActual": 12000.00,
      "percentageOfBudget": 40.0,
      "expenses": [
        {
          "description": "Catering Services - Per Plate",
          "vendorName": "Delicious Catering Co",
          "estimated": 15000.00,
          "actual": 12000.00,
          "paymentStatus": "Pending"
        }
      ]
    }
  ]
}
```

#### 3. Vendor-wise Report

Breakdown by vendor with all associated expenses.

**API Call:**
```http
GET /api/reports/vendor-wise/1 HTTP/1.1
Authorization: Bearer <token>
```

**Response (200 OK):**
```json
{
  "eventId": 1,
  "eventName": "Wedding Ceremony",
  "reportDate": "2024-10-06T19:00:00Z",
  "vendors": [
    {
      "vendorId": 5,
      "vendorName": "Grand Ballroom Hotel",
      "serviceType": "Venue",
      "contactNumber": "+1-555-0101",
      "expenseCount": 2,
      "totalEstimated": 10000.00,
      "totalActual": 10000.00,
      "totalPaid": 10000.00,
      "totalPending": 0.00,
      "expenses": [
        {
          "description": "Venue Booking Deposit",
          "category": "Venue",
          "estimated": 10000.00,
          "actual": 10000.00,
          "paymentStatus": "Paid",
          "paymentDate": "2024-09-15T00:00:00Z"
        }
      ]
    }
  ]
}
```

#### 4. Detailed Expense Report

Complete list of all expenses with full details.

**API Call:**
```http
GET /api/reports/detailed/1 HTTP/1.1
Authorization: Bearer <token>
```

**Response:** Returns all expenses with complete details including timestamps, payment status, vendor info, etc.

### Export Formats

#### PDF Export

**API Call:**
```http
POST /api/reports/export/pdf HTTP/1.1
Authorization: Bearer <token>
Content-Type: application/json

{
  "eventId": 1,
  "reportType": "budget-vs-actual",
  "includeCharts": true,
  "includeVendorDetails": true
}
```

**Response:**
```
Content-Type: application/pdf
Content-Disposition: attachment; filename="Wedding-Ceremony-Report-2024-10-06.pdf"

[Binary PDF Data]
```

#### Excel Export

**API Call:**
```http
POST /api/reports/export/excel HTTP/1.1
Authorization: Bearer <token>
Content-Type: application/json

{
  "eventId": 1,
  "reportType": "detailed",
  "includeFormulas": true
}
```

**Response:**
```
Content-Type: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
Content-Disposition: attachment; filename="Wedding-Ceremony-Report-2024-10-06.xlsx"

[Binary Excel Data]
```

### Email Report

**API Call:**
```http
POST /api/reports/email HTTP/1.1
Authorization: Bearer <token>
Content-Type: application/json

{
  "eventId": 1,
  "reportType": "budget-vs-actual",
  "recipients": [
    "user@example.com",
    "collaborator@example.com"
  ],
  "format": "pdf",
  "includeCharts": true,
  "message": "Please find attached the latest budget report for our wedding."
}
```

**Response (200 OK):**
```json
{
  "message": "Report has been sent successfully to 2 recipients.",
  "recipients": [
    "user@example.com",
    "collaborator@example.com"
  ],
  "sentAt": "2024-10-06T19:30:00Z"
}
```

### Report Filters & Options

Users can customize reports with:

- **Date Range**: Filter expenses by date range
- **Categories**: Include/exclude specific categories
- **Payment Status**: Filter by Paid, Pending, or Partial
- **Vendors**: Filter by specific vendors
- **Currency**: Choose currency format (if multi-currency support exists)
- **Include Charts**: Toggle chart inclusion in exports
- **Detail Level**: Summary vs Detailed

### Authentication & Access Control

| Action | Event Owner | Collaborator | Other Users |
|--------|-------------|--------------|-------------|
| View Reports | ✅ Yes | ✅ Yes | ❌ No (403) |
| Export PDF | ✅ Yes | ✅ Yes | ❌ No (403) |
| Export Excel | ✅ Yes | ✅ Yes | ❌ No (403) |
| Email Report | ✅ Yes | ✅ Yes (only to self) | ❌ No (403) |
| Email to Others | ✅ Yes | ❌ No (403) | ❌ No (403) |

### Report Generation Process

```mermaid
sequenceDiagram
    actor User
    participant Frontend
    participant ReportAPI
    participant Database
    participant PDFService
    participant EmailService

    User->>Frontend: Request Report
    Frontend->>ReportAPI: GET /api/reports/{eventId}
    ReportAPI->>ReportAPI: Validate Token & Access
    ReportAPI->>Database: Fetch Event Data
    ReportAPI->>Database: Fetch All Expenses
    ReportAPI->>Database: Fetch All Vendors
    Database-->>ReportAPI: Return Data
    ReportAPI->>ReportAPI: Calculate Summaries
    ReportAPI->>ReportAPI: Format Response
    ReportAPI-->>Frontend: Return Report Data
    Frontend-->>User: Display Report
    
    User->>Frontend: Click "Export as PDF"
    Frontend->>ReportAPI: POST /api/reports/export/pdf
    ReportAPI->>PDFService: Generate PDF
    PDFService->>PDFService: Create PDF Document
    PDFService->>PDFService: Add Tables & Charts
    PDFService-->>ReportAPI: Return PDF Binary
    ReportAPI-->>Frontend: Return PDF File
    Frontend-->>User: Download PDF
    
    User->>Frontend: Click "Email Report"
    Frontend->>Frontend: Show Email Form
    User->>Frontend: Enter Recipients & Click Send
    Frontend->>ReportAPI: POST /api/reports/email
    ReportAPI->>PDFService: Generate PDF
    PDFService-->>ReportAPI: Return PDF
    ReportAPI->>EmailService: Send Email with Attachment
    EmailService->>EmailService: Send via SendGrid
    EmailService-->>ReportAPI: Email Sent
    ReportAPI-->>Frontend: Success Response
    Frontend-->>User: Show Success Message
```

---

## 7️⃣ Notification & Reminder Flow

### Overview
The Notification system automatically sends alerts and reminders to users via email for budget thresholds, payment due dates, and event reminders. It runs as a background service.

### Prerequisites
- ✅ SendGrid (or SMTP) configured in backend
- ✅ Background job service running
- ✅ User must be authenticated to view notifications in-app

### Flow Diagram

```mermaid
flowchart TD
    Start([Background Job Starts]) --> CheckTime{Scheduled<br/>Time?}
    CheckTime -->|Not Yet| Sleep[Sleep Until Next Check]
    Sleep --> CheckTime
    CheckTime -->|Yes| RunChecks[Run Notification Checks]
    
    RunChecks --> BudgetCheck[Check Budget Thresholds]
    RunChecks --> PaymentCheck[Check Payment Due Dates]
    RunChecks --> EventCheck[Check Event Reminders]
    RunChecks --> VendorCheck[Check Vendor Reminders]
    
    BudgetCheck --> QueryBudgets[Query Events with High Budget Usage]
    QueryBudgets --> BudgetResults{Events > 80%<br/>Budget Used?}
    BudgetResults -->|Yes| CreateBudgetAlert[Create Budget Alert Notification]
    BudgetResults -->|No| PaymentCheck
    CreateBudgetAlert --> SendBudgetEmail[Send Email via SendGrid]
    SendBudgetEmail --> SaveNotification[Save to Notifications Table]
    SaveNotification --> PaymentCheck
    
    PaymentCheck --> QueryPayments[Query Expenses with Upcoming Due Dates]
    QueryPayments --> PaymentResults{Payments Due<br/>Within 3 Days?}
    PaymentResults -->|Yes| CreatePaymentAlert[Create Payment Reminder]
    PaymentResults -->|No| EventCheck
    CreatePaymentAlert --> SendPaymentEmail[Send Email Reminder]
    SendPaymentEmail --> SaveNotification
    
    EventCheck --> QueryEvents[Query Events Within Next 7 Days]
    QueryEvents --> EventResults{Upcoming<br/>Events?}
    EventResults -->|Yes| CreateEventReminder[Create Event Reminder]
    EventResults -->|No| VendorCheck
    CreateEventReminder --> SendEventEmail[Send Event Reminder Email]
    SendEventEmail --> SaveNotification
    
    VendorCheck --> QueryVendors[Query Vendors with Upcoming Reminders]
    QueryVendors --> VendorResults{Vendor Reminders<br/>Due?}
    VendorResults -->|Yes| CreateVendorReminder[Create Vendor Reminder]
    VendorResults -->|No| Complete
    CreateVendorReminder --> SendVendorEmail[Send Vendor Reminder Email]
    SendVendorEmail --> SaveNotification
    
    SaveNotification --> Complete[Mark as Sent]
    Complete --> Sleep
    
    subgraph User Actions
        UserLogin[User Logs In] --> CheckNotifs{New<br/>Notifications?}
        CheckNotifs -->|Yes| ShowBadge[Show Notification Badge]
        CheckNotifs -->|No| Dashboard
        ShowBadge --> Dashboard[Dashboard]
        Dashboard --> ClickNotif[Click Notifications Icon]
        ClickNotif --> LoadNotifs[GET /api/notifications]
        LoadNotifs --> DisplayList[Display Notifications List]
        DisplayList --> MarkRead[Mark as Read]
        DisplayList --> ViewDetail[View Details]
        DisplayList --> DeleteNotif[Delete Notification]
        MarkRead --> DisplayList
        ViewDetail --> RelatedPage[Navigate to Related Page]
        DeleteNotif --> DisplayList
    end
    
    style BudgetResults fill:#ffd43b,stroke:#fab005,stroke-width:2px
    style PaymentResults fill:#ffd43b,stroke:#fab005,stroke-width:2px
    style EventResults fill:#ffd43b,stroke:#fab005,stroke-width:2px
    style VendorResults fill:#ffd43b,stroke:#fab005,stroke-width:2px
```

### Notification Types

#### 1. Budget Alert Notification

**Trigger:** When event budget usage exceeds threshold (80%, 90%, 100%)

**Email Template:**
```
Subject: ⚠️ Budget Alert: [Event Name] - 85% Budget Used

Hi [User Name],

Your event "[Event Name]" has reached 85% of its allocated budget.

Budget Details:
- Total Budget: $30,000.00
- Total Spent: $25,500.00
- Remaining: $4,500.00
- Budget Utilization: 85%

Please review your expenses and adjust your budget if necessary.

View Event Details: [Link to Event]

Best regards,
BudgetEase Team
```

**API Endpoint (View Notifications):**
```http
GET /api/notifications HTTP/1.1
Authorization: Bearer <token>
```

**Response:**
```json
{
  "notifications": [
    {
      "id": 501,
      "type": "BudgetAlert",
      "title": "Budget Alert: Wedding Ceremony",
      "message": "Your event has reached 85% of its allocated budget.",
      "eventId": 1,
      "eventName": "Wedding Ceremony",
      "severity": "High",
      "isRead": false,
      "createdAt": "2024-10-06T20:00:00Z",
      "data": {
        "budgetLimit": 30000.00,
        "totalSpent": 25500.00,
        "budgetUtilization": 85.0
      }
    }
  ]
}
```

#### 2. Payment Due Reminder

**Trigger:** 3 days before payment due date, 1 day before, and on due date

**Email Template:**
```
Subject: 💳 Payment Reminder: [Vendor Name] - Due in 3 Days

Hi [User Name],

This is a reminder that you have an upcoming payment due for your event "[Event Name]".

Payment Details:
- Vendor: [Vendor Name]
- Category: [Category]
- Amount Due: $[Amount]
- Due Date: [Date]
- Days Until Due: 3 days

View Expense Details: [Link to Expense]

Best regards,
BudgetEase Team
```

**Notification Data:**
```json
{
  "id": 502,
  "type": "PaymentDue",
  "title": "Payment Due: Delicious Catering Co",
  "message": "Payment of $12,000.00 is due in 3 days.",
  "eventId": 1,
  "eventName": "Wedding Ceremony",
  "expenseId": 102,
  "vendorName": "Delicious Catering Co",
  "severity": "Medium",
  "isRead": false,
  "createdAt": "2024-10-06T08:00:00Z",
  "data": {
    "amount": 12000.00,
    "dueDate": "2024-10-09T00:00:00Z",
    "daysUntilDue": 3
  }
}
```

#### 3. Event Reminder

**Trigger:** 7 days before event, 3 days before, and 1 day before

**Email Template:**
```
Subject: 📅 Event Reminder: [Event Name] - 7 Days Away

Hi [User Name],

Your event "[Event Name]" is coming up in 7 days!

Event Details:
- Date: [Event Date]
- Venue: [Venue]
- Days Until Event: 7 days

Budget Status:
- Budget: $30,000.00
- Spent: $25,500.00
- Remaining: $4,500.00

Pending Payments: 2 payments totaling $12,000.00

View Event Dashboard: [Link to Event]

Best regards,
BudgetEase Team
```

**Notification Data:**
```json
{
  "id": 503,
  "type": "EventReminder",
  "title": "Event Reminder: Wedding Ceremony",
  "message": "Your event is coming up in 7 days!",
  "eventId": 1,
  "eventName": "Wedding Ceremony",
  "severity": "Info",
  "isRead": false,
  "createdAt": "2024-12-08T08:00:00Z",
  "data": {
    "eventDate": "2024-12-15T00:00:00Z",
    "daysUntilEvent": 7,
    "venue": "Grand Ballroom Hotel"
  }
}
```

#### 4. Vendor Reminder

**Trigger:** When vendor's `nextReminderDate` is reached

**Email Template:**
```
Subject: 📞 Vendor Reminder: Contact [Vendor Name]

Hi [User Name],

You have a scheduled reminder to contact your vendor for "[Event Name]".

Vendor Details:
- Name: [Vendor Name]
- Service Type: [Service Type]
- Contact: [Phone] | [Email]
- Payment Terms: [Terms]

Click to Call: [Phone Link]
Send Email: [Email Link]

View Vendor Details: [Link to Vendor]

Best regards,
BudgetEase Team
```

**Notification Data:**
```json
{
  "id": 504,
  "type": "VendorReminder",
  "title": "Vendor Reminder: Melody Makers DJ",
  "message": "Scheduled reminder to contact your vendor.",
  "eventId": 1,
  "eventName": "Wedding Ceremony",
  "vendorId": 8,
  "vendorName": "Melody Makers DJ & Entertainment",
  "severity": "Info",
  "isRead": false,
  "createdAt": "2024-11-25T08:00:00Z",
  "data": {
    "contactNumber": "+1-555-0404",
    "email": "bookings@melodymakers.com",
    "serviceType": "Entertainment"
  }
}
```

### Background Job Implementation

```csharp
// Background service that runs notifications
public class NotificationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificationBackgroundService> _logger;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Notification Background Service started");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var notificationService = scope.ServiceProvider
                        .GetRequiredService<INotificationService>();
                    
                    // Check and send all types of notifications
                    await notificationService.CheckAndSendBudgetAlerts();
                    await notificationService.CheckAndSendPaymentReminders();
                    await notificationService.CheckAndSendEventReminders();
                    await notificationService.CheckAndSendVendorReminders();
                }
                
                // Run every hour
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Notification Background Service");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
```

### SendGrid Configuration

**appsettings.json:**
```json
{
  "SendGrid": {
    "ApiKey": "YOUR_SENDGRID_API_KEY",
    "FromEmail": "noreply@budgetease.com",
    "FromName": "BudgetEase"
  },
  "NotificationSettings": {
    "BudgetAlertThresholds": [80, 90, 100],
    "PaymentReminderDays": [3, 1, 0],
    "EventReminderDays": [7, 3, 1],
    "VendorReminderDaysAhead": 3
  }
}
```

### User Notification Management

#### View All Notifications

**API Call:**
```http
GET /api/notifications HTTP/1.1
Authorization: Bearer <token>
```

**Query Parameters:**
- `unreadOnly=true` - Show only unread notifications
- `type=BudgetAlert` - Filter by notification type
- `page=1&pageSize=20` - Pagination

#### Mark Notification as Read

**API Call:**
```http
PATCH /api/notifications/501/read HTTP/1.1
Authorization: Bearer <token>
```

**Response (200 OK):**
```json
{
  "id": 501,
  "isRead": true,
  "readAt": "2024-10-06T21:00:00Z"
}
```

#### Mark All as Read

**API Call:**
```http
PATCH /api/notifications/mark-all-read HTTP/1.1
Authorization: Bearer <token>
```

**Response (200 OK):**
```json
{
  "message": "All notifications marked as read",
  "count": 15
}
```

#### Delete Notification

**API Call:**
```http
DELETE /api/notifications/501 HTTP/1.1
Authorization: Bearer <token>
```

**Response (204 No Content)**

### Notification Badge

In the UI, show a badge with unread notification count:

```html
<a href="/notifications">
  <i class="bell-icon"></i>
  <span class="badge">5</span> <!-- Unread count -->
</a>
```

### Optional: SMS/WhatsApp Integration

For future enhancement, integrate Twilio for SMS/WhatsApp notifications:

**Configuration:**
```json
{
  "Twilio": {
    "AccountSid": "YOUR_TWILIO_ACCOUNT_SID",
    "AuthToken": "YOUR_TWILIO_AUTH_TOKEN",
    "FromNumber": "+1234567890",
    "WhatsAppFromNumber": "whatsapp:+1234567890"
  }
}
```

### Authentication Requirements

All notification endpoints require authentication:

- ✅ Valid JWT token in Authorization header
- ✅ User can only view their own notifications
- ✅ Notifications are user-specific (not shared between users)
- ✅ Background job uses system credentials to send emails

---

## 8️⃣ Logout / Session Management Flow

### Overview
Session management ensures secure access to the application with automatic timeout and logout capabilities. Invalid or expired sessions redirect users to login.

### Prerequisites
- ✅ User must have an active session to logout
- ✅ JWT tokens have expiration time
- ✅ Refresh tokens (optional) for extending sessions

### Flow Diagram

```mermaid
stateDiagram-v2
    [*] --> LoggedIn: Successful Login
    
    LoggedIn --> ActiveSession: Valid Token
    ActiveSession --> UserActivity: User Interacts
    
    UserActivity --> APICall: Makes API Request
    APICall --> ValidateToken: Backend Validates Token
    
    ValidateToken --> TokenValid: Token Valid & Not Expired
    ValidateToken --> TokenExpired: Token Expired
    ValidateToken --> TokenInvalid: Token Invalid/Tampered
    
    TokenValid --> ActiveSession: Continue Session
    
    TokenExpired --> RefreshCheck: Check Refresh Token
    RefreshCheck --> HasRefreshToken: Has Valid Refresh Token
    RefreshCheck --> NoRefreshToken: No Refresh Token
    
    HasRefreshToken --> RenewToken: Issue New Access Token
    RenewToken --> ActiveSession
    
    NoRefreshToken --> SessionExpired: Session Expired
    TokenInvalid --> SessionExpired
    
    SessionExpired --> ClearClientData: Clear Token from Storage
    ClearClientData --> RedirectLogin: Redirect to Login
    RedirectLogin --> [*]
    
    UserActivity --> ManualLogout: User Clicks Logout
    ManualLogout --> LogoutAPI: POST /api/auth/logout
    LogoutAPI --> InvalidateToken: Invalidate Token
    InvalidateToken --> ClearClientData
    
    ActiveSession --> IdleTimeout: No Activity for 30 Minutes
    IdleTimeout --> SessionExpired
    
    ActiveSession --> ConcurrentLogin: Login from Another Device
    ConcurrentLogin --> InvalidatePrevious: Invalidate Previous Session
    InvalidatePrevious --> SessionExpired
```

### User Actions

#### 1. Manual Logout

When user clicks the logout button:

**Route:** `/logout` or Logout button in navbar  
**Auth Required:** ✅ Yes

**Frontend Flow:**
```javascript
async function logout() {
  try {
    // Call logout API
    await fetch('/api/auth/logout', {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${getToken()}`,
        'Content-Type': 'application/json'
      }
    });
  } catch (error) {
    console.error('Logout error:', error);
  } finally {
    // Always clear local data even if API fails
    clearAuthData();
    redirectToLogin();
  }
}

function clearAuthData() {
  // Clear token from storage
  localStorage.removeItem('authToken');
  localStorage.removeItem('userId');
  localStorage.removeItem('userEmail');
  localStorage.removeItem('activeEventId');
  
  // Clear cookies
  document.cookie = 'authToken=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
  
  // Clear any cached data
  sessionStorage.clear();
}

function redirectToLogin() {
  window.location.href = '/login';
}
```

**API Call:**
```http
POST /api/auth/logout HTTP/1.1
Authorization: Bearer <token>
Content-Type: application/json
```

**Response (200 OK):**
```json
{
  "message": "Logged out successfully",
  "loggedOutAt": "2024-10-06T22:00:00Z"
}
```

**Backend Action:**
- Invalidate the JWT token (add to blacklist if using blacklist approach)
- Clear any server-side session data
- Log the logout event for audit purposes

#### 2. Automatic Session Timeout

**Trigger:** No user activity for configurable duration (default: 30 minutes)

**Frontend Implementation:**
```javascript
let inactivityTimer;
const INACTIVITY_TIMEOUT = 30 * 60 * 1000; // 30 minutes

function resetInactivityTimer() {
  clearTimeout(inactivityTimer);
  
  inactivityTimer = setTimeout(() => {
    showSessionExpiredMessage();
    logout();
  }, INACTIVITY_TIMEOUT);
}

// Reset timer on user activity
['mousedown', 'mousemove', 'keypress', 'scroll', 'touchstart', 'click'].forEach(event => {
  document.addEventListener(event, resetInactivityTimer, true);
});

// Start timer on page load
resetInactivityTimer();
```

**User Experience:**
1. Show warning dialog 2 minutes before timeout
2. Offer "Stay Logged In" button to extend session
3. If no action, auto-logout and redirect to login
4. Show message: "Your session has expired due to inactivity. Please log in again."

#### 3. Token Expiration Handling

**JWT Token Expiration:** 1 hour (configurable)

**API Response when token expired:**
```http
HTTP/1.1 401 Unauthorized
Content-Type: application/json

{
  "error": "Token has expired",
  "errorCode": "TOKEN_EXPIRED",
  "expiredAt": "2024-10-06T22:00:00Z"
}
```

**Frontend Handler:**
```javascript
async function handleAPICall(url, options) {
  const response = await fetch(url, options);
  
  if (response.status === 401) {
    const error = await response.json();
    
    if (error.errorCode === 'TOKEN_EXPIRED') {
      // Try to refresh token
      const refreshed = await tryRefreshToken();
      
      if (refreshed) {
        // Retry the original request
        return fetch(url, {
          ...options,
          headers: {
            ...options.headers,
            'Authorization': `Bearer ${getNewToken()}`
          }
        });
      } else {
        // Refresh failed, logout
        showSessionExpiredMessage();
        logout();
      }
    }
  }
  
  return response;
}
```

#### 4. Refresh Token Flow (Optional)

**Request New Access Token:**
```http
POST /api/auth/refresh HTTP/1.1
Content-Type: application/json

{
  "refreshToken": "refresh_token_string_here"
}
```

**Response (200 OK):**
```json
{
  "accessToken": "new_jwt_token_here",
  "expiresIn": 3600,
  "tokenType": "Bearer"
}
```

**Response (401 Unauthorized):**
```json
{
  "error": "Invalid or expired refresh token",
  "errorCode": "REFRESH_TOKEN_INVALID"
}
```

#### 5. Concurrent Login Handling

**Strategy 1: Allow Multiple Sessions**
- User can be logged in on multiple devices
- Each device has its own JWT token
- All sessions remain valid until individual logout or expiration

**Strategy 2: Single Session Only**
- When user logs in on new device, invalidate previous sessions
- Previous devices get 401 Unauthorized on next API call
- Show message: "You have been logged out because you signed in on another device."

**Implementation (Single Session):**
```csharp
public async Task<LoginResponse> Login(LoginRequest request)
{
    // Validate credentials
    var user = await ValidateUser(request.Email, request.Password);
    
    // Generate new session ID
    var sessionId = Guid.NewGuid().ToString();
    
    // Store session ID in database
    user.CurrentSessionId = sessionId;
    await _context.SaveChangesAsync();
    
    // Include session ID in JWT token
    var token = GenerateJwtToken(user, sessionId);
    
    return new LoginResponse { Token = token };
}

// Validate token middleware
public async Task<bool> ValidateToken(string token, string sessionId)
{
    var user = await GetUserFromToken(token);
    
    // Check if session ID matches current session
    if (user.CurrentSessionId != sessionId)
    {
        return false; // Session invalidated
    }
    
    return true;
}
```

### Session Configuration

**appsettings.json:**
```json
{
  "JwtSettings": {
    "SecretKey": "your-secret-key-here-make-it-long-and-random",
    "Issuer": "BudgetEase",
    "Audience": "BudgetEase-Users",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7,
    "RequireHttpsMetadata": true
  },
  "SessionSettings": {
    "InactivityTimeoutMinutes": 30,
    "ShowTimeoutWarningMinutes": 2,
    "AllowConcurrentLogins": false,
    "MaxConcurrentSessions": 1
  }
}
```

### Security Best Practices

#### 1. Token Storage

**Recommended: HTTP-Only Cookies**
```csharp
Response.Cookies.Append("authToken", token, new CookieOptions
{
    HttpOnly = true,
    Secure = true, // HTTPS only
    SameSite = SameSiteMode.Strict,
    Expires = DateTimeOffset.UtcNow.AddHours(1)
});
```

**Alternative: Local Storage (with CSRF protection)**
- Store in localStorage for SPAs
- Include CSRF token in separate cookie
- Validate CSRF token on state-changing requests

#### 2. Token Blacklisting

For invalidated tokens before expiration:

```csharp
// Token blacklist service
public class TokenBlacklistService
{
    private readonly IDistributedCache _cache;
    
    public async Task BlacklistToken(string token, DateTime expiration)
    {
        var expiresIn = expiration - DateTime.UtcNow;
        await _cache.SetStringAsync(
            $"blacklist:{token}",
            "1",
            new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expiration
            }
        );
    }
    
    public async Task<bool> IsTokenBlacklisted(string token)
    {
        var value = await _cache.GetStringAsync($"blacklist:{token}");
        return value != null;
    }
}
```

#### 3. Audit Logging

Log all authentication events:

```csharp
public class AuthAuditLog
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Action { get; set; } // Login, Logout, TokenRefresh
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public DateTime Timestamp { get; set; }
    public bool Success { get; set; }
    public string FailureReason { get; set; }
}
```

### Error Handling

| Error Code | HTTP Status | Message | Action |
|------------|-------------|---------|--------|
| TOKEN_EXPIRED | 401 | Token has expired | Refresh token or re-login |
| TOKEN_INVALID | 401 | Invalid token | Clear storage, redirect to login |
| SESSION_EXPIRED | 401 | Session has expired | Show message, redirect to login |
| UNAUTHORIZED | 403 | Insufficient permissions | Show access denied page |
| CONCURRENT_LOGIN | 401 | Logged in from another device | Show message, redirect to login |

### User Experience Flow

```mermaid
sequenceDiagram
    actor User
    participant Browser
    participant App
    participant API
    participant Auth

    Note over User,Auth: Normal Activity
    User->>Browser: Interact with App
    Browser->>App: User Action
    App->>API: API Request + Token
    API->>Auth: Validate Token
    Auth-->>API: Token Valid
    API-->>App: Response
    App-->>User: Update UI
    
    Note over User,Auth: Session About to Expire
    App->>App: Detect Inactivity (28 min)
    App-->>User: Show Warning Modal
    Note over User: "Your session will expire in 2 minutes.<br/>Click to stay logged in."
    
    alt User Clicks "Stay Logged In"
        User->>App: Click Button
        App->>API: Refresh Token
        API->>Auth: Issue New Token
        Auth-->>API: New Token
        API-->>App: Token Updated
        App->>App: Reset Inactivity Timer
    else User Doesn't Respond
        App->>App: Timeout (30 min)
        App->>App: Clear Auth Data
        App-->>User: Show "Session Expired" Message
        App->>Browser: Redirect to Login
    end
    
    Note over User,Auth: Manual Logout
    User->>App: Click Logout
    App->>API: POST /api/auth/logout
    API->>Auth: Invalidate Token
    Auth-->>API: Success
    API-->>App: Logged Out
    App->>App: Clear Auth Data
    App->>Browser: Redirect to Login
    Browser-->>User: Show Login Page
```

---

## 🗺️ Complete Navigation Structure

### Application Sitemap

```
BudgetEase Application
│
├── 🔐 Public Routes (No Authentication)
│   ├── /login - Login Page
│   ├── /register - Registration Page
│   ├── /forgot-password - Forgot Password
│   └── /reset-password - Reset Password Page
│
└── 🔒 Protected Routes (Authentication Required)
    │
    ├── / or /dashboard - Dashboard (Home)
    │   ├── Summary Cards
    │   ├── Charts & Visualizations
    │   ├── Recent Activity
    │   └── Quick Actions
    │
    ├── /events - Events Management
    │   ├── /events - Events List
    │   ├── /events/create - Create New Event
    │   ├── /events/{id} - Event Details
    │   ├── /events/{id}/edit - Edit Event
    │   └── /events/{id}/collaborators - Manage Collaborators
    │
    ├── /expenses - Expense Tracking
    │   ├── /expenses - Expenses List (requires active event)
    │   ├── /expenses/create - Add New Expense
    │   └── /expenses/{id}/edit - Edit Expense
    │
    ├── /vendors - Vendor Management
    │   ├── /vendors - Vendors List (requires active event)
    │   ├── /vendors/create - Add New Vendor
    │   ├── /vendors/{id}/edit - Edit Vendor
    │   └── /vendors/reminders - View All Reminders
    │
    ├── /reports - Reports & Exports
    │   ├── /reports - Reports Dashboard
    │   ├── /reports/budget-vs-actual - Budget Report
    │   ├── /reports/category-wise - Category Report
    │   └── /reports/vendor-wise - Vendor Report
    │
    ├── /notifications - Notifications Center
    │   └── /notifications - View All Notifications
    │
    ├── /settings - User Settings
    │   ├── /settings/profile - Profile Settings
    │   ├── /settings/security - Security Settings
    │   └── /settings/notifications - Notification Preferences
    │
    └── /logout - Logout Action
```

### Authentication Checkpoint Matrix

| Route | Authentication | Additional Requirements | Redirect on Fail |
|-------|----------------|------------------------|------------------|
| `/login` | ❌ No | - | - |
| `/register` | ❌ No | - | - |
| `/forgot-password` | ❌ No | - | - |
| `/` or `/dashboard` | ✅ Yes | - | `/login` |
| `/events` | ✅ Yes | - | `/login` |
| `/events/create` | ✅ Yes | - | `/login` |
| `/events/{id}` | ✅ Yes | Event access (Owner/Collaborator) | `/login` or `/events` |
| `/events/{id}/edit` | ✅ Yes | Event Owner only | `/login` or `/events` |
| `/expenses` | ✅ Yes | Active event selected | `/login` or `/events` |
| `/expenses/create` | ✅ Yes | Active event + Event access | `/login` or `/events` |
| `/vendors` | ✅ Yes | Active event selected | `/login` or `/events` |
| `/vendors/create` | ✅ Yes | Active event + Event access | `/login` or `/events` |
| `/reports` | ✅ Yes | - | `/login` |
| `/notifications` | ✅ Yes | - | `/login` |
| `/settings` | ✅ Yes | - | `/login` |

---

## 📋 Structured Navigation JSON

```json
{
  "application": "BudgetEase",
  "version": "1.0",
  "authenticationRequired": true,
  "navigation": {
    "public": [
      {
        "path": "/login",
        "name": "Login",
        "component": "Login.razor",
        "requiresAuth": false,
        "description": "User login page with email/password and Google OAuth"
      },
      {
        "path": "/register",
        "name": "Register",
        "component": "Register.razor",
        "requiresAuth": false,
        "description": "New user registration"
      },
      {
        "path": "/forgot-password",
        "name": "Forgot Password",
        "component": "ForgotPassword.razor",
        "requiresAuth": false,
        "description": "Password recovery via email"
      }
    ],
    "protected": [
      {
        "path": "/dashboard",
        "name": "Dashboard",
        "component": "Dashboard.razor",
        "requiresAuth": true,
        "icon": "dashboard",
        "showInMenu": true,
        "order": 1,
        "description": "Main dashboard with summary, charts, and quick actions",
        "apiEndpoints": [
          "GET /api/dashboard/summary",
          "GET /api/dashboard/charts/category-spending"
        ]
      },
      {
        "path": "/events",
        "name": "Events",
        "component": "Events.razor",
        "requiresAuth": true,
        "icon": "event",
        "showInMenu": true,
        "order": 2,
        "description": "Event management - list, create, edit, delete",
        "apiEndpoints": [
          "GET /api/events",
          "POST /api/events",
          "GET /api/events/{id}",
          "PUT /api/events/{id}",
          "DELETE /api/events/{id}"
        ],
        "children": [
          {
            "path": "/events/create",
            "name": "Create Event",
            "component": "CreateEvent.razor",
            "requiresAuth": true,
            "showInMenu": false
          },
          {
            "path": "/events/{id}",
            "name": "Event Details",
            "component": "EventDetails.razor",
            "requiresAuth": true,
            "additionalCheck": "eventAccess",
            "showInMenu": false
          },
          {
            "path": "/events/{id}/edit",
            "name": "Edit Event",
            "component": "EditEvent.razor",
            "requiresAuth": true,
            "additionalCheck": "eventOwner",
            "showInMenu": false
          }
        ]
      },
      {
        "path": "/expenses",
        "name": "Expenses",
        "component": "Expenses.razor",
        "requiresAuth": true,
        "icon": "receipt",
        "showInMenu": true,
        "order": 3,
        "description": "Expense tracking and management",
        "prerequisite": "activeEventSelected",
        "apiEndpoints": [
          "GET /api/expenses/event/{eventId}",
          "POST /api/expenses",
          "PUT /api/expenses/{id}",
          "DELETE /api/expenses/{id}"
        ],
        "children": [
          {
            "path": "/expenses/create",
            "name": "Add Expense",
            "component": "CreateExpense.razor",
            "requiresAuth": true,
            "showInMenu": false
          }
        ]
      },
      {
        "path": "/vendors",
        "name": "Vendors",
        "component": "Vendors.razor",
        "requiresAuth": true,
        "icon": "store",
        "showInMenu": true,
        "order": 4,
        "description": "Vendor management and reminders",
        "prerequisite": "activeEventSelected",
        "apiEndpoints": [
          "GET /api/vendors/event/{eventId}",
          "POST /api/vendors",
          "PUT /api/vendors/{id}",
          "DELETE /api/vendors/{id}",
          "GET /api/vendors/reminders"
        ],
        "children": [
          {
            "path": "/vendors/create",
            "name": "Add Vendor",
            "component": "CreateVendor.razor",
            "requiresAuth": true,
            "showInMenu": false
          },
          {
            "path": "/vendors/reminders",
            "name": "Reminders",
            "component": "VendorReminders.razor",
            "requiresAuth": true,
            "showInMenu": false
          }
        ]
      },
      {
        "path": "/reports",
        "name": "Reports",
        "component": "Reports.razor",
        "requiresAuth": true,
        "icon": "assessment",
        "showInMenu": true,
        "order": 5,
        "description": "Budget reports and exports (PDF/Excel)",
        "apiEndpoints": [
          "GET /api/reports/budget-vs-actual/{eventId}",
          "GET /api/reports/category-wise/{eventId}",
          "GET /api/reports/vendor-wise/{eventId}",
          "POST /api/reports/export/pdf",
          "POST /api/reports/export/excel",
          "POST /api/reports/email"
        ]
      },
      {
        "path": "/notifications",
        "name": "Notifications",
        "component": "Notifications.razor",
        "requiresAuth": true,
        "icon": "notifications",
        "showInMenu": true,
        "showBadge": true,
        "order": 6,
        "description": "View all notifications and alerts",
        "apiEndpoints": [
          "GET /api/notifications",
          "PATCH /api/notifications/{id}/read",
          "DELETE /api/notifications/{id}"
        ]
      },
      {
        "path": "/settings",
        "name": "Settings",
        "component": "Settings.razor",
        "requiresAuth": true,
        "icon": "settings",
        "showInMenu": true,
        "order": 7,
        "description": "User settings and preferences",
        "children": [
          {
            "path": "/settings/profile",
            "name": "Profile",
            "component": "ProfileSettings.razor",
            "requiresAuth": true,
            "showInMenu": false
          },
          {
            "path": "/settings/security",
            "name": "Security",
            "component": "SecuritySettings.razor",
            "requiresAuth": true,
            "showInMenu": false
          }
        ]
      }
    ]
  },
  "actions": {
    "authentication": [
      {
        "action": "login",
        "method": "POST",
        "endpoint": "/api/auth/login",
        "requiresAuth": false,
        "redirectOnSuccess": "/dashboard"
      },
      {
        "action": "register",
        "method": "POST",
        "endpoint": "/api/auth/register",
        "requiresAuth": false,
        "redirectOnSuccess": "/dashboard"
      },
      {
        "action": "logout",
        "method": "POST",
        "endpoint": "/api/auth/logout",
        "requiresAuth": true,
        "redirectOnSuccess": "/login"
      },
      {
        "action": "forgotPassword",
        "method": "POST",
        "endpoint": "/api/auth/forgot-password",
        "requiresAuth": false,
        "redirectOnSuccess": "/login"
      }
    ],
    "events": [
      {
        "action": "createEvent",
        "method": "POST",
        "endpoint": "/api/events",
        "requiresAuth": true,
        "requiredRole": "authenticated"
      },
      {
        "action": "updateEvent",
        "method": "PUT",
        "endpoint": "/api/events/{id}",
        "requiresAuth": true,
        "requiredRole": "eventOwner"
      },
      {
        "action": "deleteEvent",
        "method": "DELETE",
        "endpoint": "/api/events/{id}",
        "requiresAuth": true,
        "requiredRole": "eventOwner"
      }
    ],
    "expenses": [
      {
        "action": "createExpense",
        "method": "POST",
        "endpoint": "/api/expenses",
        "requiresAuth": true,
        "prerequisite": "activeEvent",
        "requiredRole": "eventAccess"
      },
      {
        "action": "updateExpense",
        "method": "PUT",
        "endpoint": "/api/expenses/{id}",
        "requiresAuth": true,
        "requiredRole": "eventAccess"
      },
      {
        "action": "deleteExpense",
        "method": "DELETE",
        "endpoint": "/api/expenses/{id}",
        "requiresAuth": true,
        "requiredRole": "eventOwner"
      }
    ],
    "vendors": [
      {
        "action": "createVendor",
        "method": "POST",
        "endpoint": "/api/vendors",
        "requiresAuth": true,
        "prerequisite": "activeEvent",
        "requiredRole": "eventAccess"
      },
      {
        "action": "setReminder",
        "method": "PUT",
        "endpoint": "/api/vendors/{id}",
        "requiresAuth": true,
        "requiredRole": "eventAccess"
      }
    ],
    "reports": [
      {
        "action": "generatePDF",
        "method": "POST",
        "endpoint": "/api/reports/export/pdf",
        "requiresAuth": true,
        "requiredRole": "eventAccess"
      },
      {
        "action": "emailReport",
        "method": "POST",
        "endpoint": "/api/reports/email",
        "requiresAuth": true,
        "requiredRole": "eventAccess"
      }
    ]
  },
  "backgroundJobs": [
    {
      "name": "BudgetAlertMonitor",
      "schedule": "Every 1 hour",
      "description": "Check events with budget usage > 80% and send alerts"
    },
    {
      "name": "PaymentReminderJob",
      "schedule": "Daily at 8:00 AM",
      "description": "Send payment due reminders (3 days, 1 day, on due date)"
    },
    {
      "name": "EventReminderJob",
      "schedule": "Daily at 8:00 AM",
      "description": "Send event reminders (7 days, 3 days, 1 day before)"
    },
    {
      "name": "VendorReminderJob",
      "schedule": "Daily at 8:00 AM",
      "description": "Send vendor contact reminders based on nextReminderDate"
    }
  ]
}
```

---

## 🔐 Authentication Flow Summary

### Authentication Gates

Every feature in BudgetEase has authentication checkpoints:

#### Level 1: User Authentication
- **Check:** Is user logged in?
- **How:** Valid JWT token exists
- **Fail:** Redirect to `/login`

#### Level 2: Event Access
- **Check:** Does user have access to the event?
- **How:** User is owner OR collaborator
- **Fail:** 403 Forbidden or redirect to `/events`

#### Level 3: Active Event Context
- **Check:** Has user selected an active event?
- **How:** Active event ID stored in session
- **Fail:** Prompt to select event

#### Level 4: Role-Based Permission
- **Check:** Does user have required role for action?
- **How:** Owner vs Collaborator permissions
- **Fail:** 403 Forbidden with message

### Authentication Flow Summary Diagram

```mermaid
graph LR
    A[User Opens App] --> B{Authenticated?}
    B -->|No| C[Login Page]
    B -->|Yes| D[Dashboard]
    
    C --> E{Login Success?}
    E -->|No| C
    E -->|Yes| D
    
    D --> F{User Action}
    F --> G[Validate Token]
    
    G -->|Valid| H{Check Permissions}
    G -->|Invalid| C
    
    H -->|Authorized| I[Execute Action]
    H -->|Unauthorized| J[403 Forbidden]
    
    I --> K{Session Active?}
    K -->|Yes| F
    K -->|Timeout| L[Session Expired]
    L --> C
    
    style B fill:#ff6b6b,stroke:#c92a2a,stroke-width:2px
    style G fill:#ff6b6b,stroke:#c92a2a,stroke-width:2px
    style H fill:#ffd43b,stroke:#fab005,stroke-width:2px
    style D fill:#51cf66,stroke:#2f9e44,stroke-width:2px
```

---

## 📊 Feature-to-API Mapping

### Complete API Endpoint Reference

| Feature | HTTP Method | Endpoint | Auth Required | Additional Check |
|---------|-------------|----------|---------------|------------------|
| **Authentication** |
| Register | POST | `/api/auth/register` | ❌ No | - |
| Login | POST | `/api/auth/login` | ❌ No | - |
| Logout | POST | `/api/auth/logout` | ✅ Yes | - |
| Forgot Password | POST | `/api/auth/forgot-password` | ❌ No | - |
| Reset Password | POST | `/api/auth/reset-password` | ❌ No | Valid reset token |
| Refresh Token | POST | `/api/auth/refresh` | ❌ No | Valid refresh token |
| **Events** |
| List Events | GET | `/api/events` | ✅ Yes | - |
| Get Event | GET | `/api/events/{id}` | ✅ Yes | Event Access |
| Create Event | POST | `/api/events` | ✅ Yes | - |
| Update Event | PUT | `/api/events/{id}` | ✅ Yes | Event Owner |
| Delete Event | DELETE | `/api/events/{id}` | ✅ Yes | Event Owner |
| Add Collaborator | POST | `/api/events/{id}/collaborators` | ✅ Yes | Event Owner |
| Remove Collaborator | DELETE | `/api/events/{id}/collaborators/{userId}` | ✅ Yes | Event Owner |
| **Expenses** |
| List Expenses | GET | `/api/expenses/event/{eventId}` | ✅ Yes | Event Access |
| Get Expense | GET | `/api/expenses/{id}` | ✅ Yes | Event Access |
| Create Expense | POST | `/api/expenses` | ✅ Yes | Event Access |
| Update Expense | PUT | `/api/expenses/{id}` | ✅ Yes | Event Access |
| Delete Expense | DELETE | `/api/expenses/{id}` | ✅ Yes | Event Owner |
| **Vendors** |
| List Vendors | GET | `/api/vendors/event/{eventId}` | ✅ Yes | Event Access |
| Get Vendor | GET | `/api/vendors/{id}` | ✅ Yes | Event Access |
| Create Vendor | POST | `/api/vendors` | ✅ Yes | Event Access |
| Update Vendor | PUT | `/api/vendors/{id}` | ✅ Yes | Event Access |
| Delete Vendor | DELETE | `/api/vendors/{id}` | ✅ Yes | Event Owner |
| Get Reminders | GET | `/api/vendors/reminders` | ✅ Yes | - |
| **Reports** |
| Budget Report | GET | `/api/reports/budget-vs-actual/{eventId}` | ✅ Yes | Event Access |
| Category Report | GET | `/api/reports/category-wise/{eventId}` | ✅ Yes | Event Access |
| Vendor Report | GET | `/api/reports/vendor-wise/{eventId}` | ✅ Yes | Event Access |
| Detailed Report | GET | `/api/reports/detailed/{eventId}` | ✅ Yes | Event Access |
| Export PDF | POST | `/api/reports/export/pdf` | ✅ Yes | Event Access |
| Export Excel | POST | `/api/reports/export/excel` | ✅ Yes | Event Access |
| Email Report | POST | `/api/reports/email` | ✅ Yes | Event Access |
| **Notifications** |
| List Notifications | GET | `/api/notifications` | ✅ Yes | - |
| Mark as Read | PATCH | `/api/notifications/{id}/read` | ✅ Yes | Own notification |
| Mark All Read | PATCH | `/api/notifications/mark-all-read` | ✅ Yes | - |
| Delete Notification | DELETE | `/api/notifications/{id}` | ✅ Yes | Own notification |
| **Dashboard** |
| Dashboard Summary | GET | `/api/dashboard/summary` | ✅ Yes | - |
| Category Chart | GET | `/api/dashboard/charts/category-spending` | ✅ Yes | - |

---

## 📝 Implementation Checklist

### Backend (API)

- [ ] **Authentication Controller**
  - [ ] POST /api/auth/register
  - [ ] POST /api/auth/login
  - [ ] POST /api/auth/logout
  - [ ] POST /api/auth/forgot-password
  - [ ] POST /api/auth/reset-password
  - [ ] POST /api/auth/refresh

- [ ] **Authorization Middleware**
  - [ ] JWT token validation
  - [ ] Role-based authorization
  - [ ] Event access validation
  - [ ] Token blacklisting (optional)

- [ ] **Event Collaborator Endpoints**
  - [ ] POST /api/events/{id}/collaborators
  - [ ] DELETE /api/events/{id}/collaborators/{userId}

- [ ] **Reports Controller**
  - [ ] GET /api/reports/budget-vs-actual/{eventId}
  - [ ] GET /api/reports/category-wise/{eventId}
  - [ ] GET /api/reports/vendor-wise/{eventId}
  - [ ] POST /api/reports/export/pdf
  - [ ] POST /api/reports/export/excel
  - [ ] POST /api/reports/email

- [ ] **Notifications Controller**
  - [ ] GET /api/notifications
  - [ ] PATCH /api/notifications/{id}/read
  - [ ] DELETE /api/notifications/{id}

- [ ] **Dashboard Controller**
  - [ ] GET /api/dashboard/summary
  - [ ] GET /api/dashboard/charts/category-spending

- [ ] **Background Services**
  - [ ] Budget alert monitoring
  - [ ] Payment reminders
  - [ ] Event reminders
  - [ ] Vendor reminders

### Frontend (Blazor)

- [ ] **Authentication Pages**
  - [x] Login.razor
  - [x] Register.razor
  - [ ] ForgotPassword.razor
  - [ ] ResetPassword.razor

- [ ] **Core Pages**
  - [ ] Dashboard.razor
  - [ ] Events.razor
  - [ ] EventDetails.razor
  - [ ] Expenses.razor
  - [ ] Vendors.razor
  - [ ] Reports.razor
  - [ ] Notifications.razor
  - [ ] Settings.razor

- [ ] **Authentication Service**
  - [ ] Login method
  - [ ] Logout method
  - [ ] Token storage
  - [ ] Token refresh
  - [ ] Session timeout handling

- [ ] **Navigation**
  - [ ] Update NavMenu.razor
  - [ ] Add authentication state display
  - [ ] Add notification badge
  - [ ] Role-based menu visibility

### Integration

- [ ] **SendGrid Email Service**
  - [ ] Configure API key
  - [ ] Email templates
  - [ ] Send notification emails

- [ ] **PDF/Excel Generation**
  - [ ] Install libraries (QuestPDF, EPPlus)
  - [ ] Create report templates
  - [ ] File download handling

- [ ] **Google OAuth (Optional)**
  - [ ] Configure Google credentials
  - [ ] OAuth callback handling

---

## 🎯 Summary

### Key Principles

1. **Authentication First**: Every feature requires authentication
2. **Role-Based Access**: Owner vs Collaborator permissions
3. **Event Context**: Expenses and vendors require active event
4. **Automatic Alerts**: Background jobs for notifications
5. **Session Security**: Token expiration and refresh
6. **Audit Trail**: Log all authentication events

### User Journey

```
1. User lands on app → Redirected to Login
2. User logs in → Session created with JWT token
3. User sees Dashboard → Summary of all events
4. User creates Event → New event container created
5. User selects Event → Sets active event context
6. User adds Expenses → Tracked against event budget
7. User adds Vendors → Linked to event with reminders
8. System monitors → Sends automatic alerts
9. User views Reports → Generate PDF/Excel exports
10. User logs out → Session invalidated, redirect to Login
```

### Conditional Flows

- **No Active Event**: Prompt user to select event before accessing expenses/vendors
- **Budget > 80%**: Automatic email alert to user
- **Payment Due**: Reminder emails at 3 days, 1 day, on due date
- **Event Upcoming**: Reminder emails at 7 days, 3 days, 1 day
- **Session Timeout**: Auto-logout after inactivity, redirect to login
- **Token Expired**: Attempt refresh, otherwise logout

---

## 📚 Related Documentation

- [README.md](README.md) - Project overview
- [PROJECT_STRUCTURE.md](PROJECT_STRUCTURE.md) - Code structure
- [REQUIREMENTS_VALIDATION.md](REQUIREMENTS_VALIDATION.md) - Requirements analysis
- [TODO.md](TODO.md) - Implementation checklist

---

**Document Version:** 1.0  
**Created:** 2024  
**Last Updated:** 2024  
**Status:** Production Ready

**Author:** BudgetEase Development Team  
**Purpose:** Complete user flow documentation for AI-based design and development

---

*This document covers all user flows with authentication-first architecture for the BudgetEase application. Every feature, page, and API call requires valid user authentication.*

