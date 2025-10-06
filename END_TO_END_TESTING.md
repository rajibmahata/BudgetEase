# BudgetEase End-to-End Testing Guide

This document provides comprehensive instructions for testing BudgetEase application end-to-end with the seeded demo data.

## Prerequisites

1. .NET 9.0 SDK installed
2. Sample data seeded (automatic on first run)
3. API and Web applications running

## Starting the Applications

### Terminal 1: Start the API
```bash
cd src/BudgetEase.Api
dotnet run
```
API will be available at: http://localhost:5108

### Terminal 2: Start the Web UI
```bash
cd src/BudgetEase.Web
dotnet run
```
Web UI will be available at: https://localhost:7001

## Automated API Testing

Run the automated validation script:
```bash
./test_api_endpoints.sh
```

This validates:
- Health check endpoint
- Event CRUD operations
- Expense management
- Vendor management
- Data integrity

## Manual Testing Scenarios

### Scenario 1: API Endpoints Testing

#### Test Event Endpoints
```bash
# Get all events (requires auth, returns empty without auth)
curl http://localhost:5108/api/events

# Get specific event
curl http://localhost:5108/api/events/1

# Get event expenses
curl http://localhost:5108/api/expenses/event/1

# Get event vendors
curl http://localhost:5108/api/vendors/event/1
```

#### Test Vendor Endpoints
```bash
# Get vendor by ID
curl http://localhost:5108/api/vendors/1

# Get all vendor reminders
curl http://localhost:5108/api/vendors/reminders
```

### Scenario 2: Database Validation

Verify seeded data directly:
```bash
cd src/BudgetEase.Api
sqlite3 budgetease.db

# Run queries
SELECT COUNT(*) FROM Events;          -- Expected: 4
SELECT COUNT(*) FROM Expenses;        -- Expected: 14
SELECT COUNT(*) FROM Vendors;         -- Expected: 10
SELECT COUNT(*) FROM AspNetUsers;     -- Expected: 3

# View specific data
SELECT Name, Type, BudgetLimit FROM Events;
SELECT Email, FirstName FROM AspNetUsers;
```

### Scenario 3: Web UI Navigation Testing

#### Login Flow
1. Open https://localhost:7001
2. Navigate to Login page
3. Enter credentials:
   - Email: demo@budgetease.com
   - Password: Demo@123
4. Verify successful login

#### Dashboard Navigation
1. After login, verify Dashboard loads
2. Check for event cards
3. Verify budget summaries display
4. Test navigation links

#### Events Management
1. Navigate to Events page
2. Verify 2 events appear (Wedding, Birthday)
3. Click on "Sarah & Tom's Wedding"
4. Verify event details load:
   - Name, date, venue
   - Budget: $50,000
   - Expenses listed
   - Vendors displayed

#### Expense Management
1. From Wedding event, view Expenses tab
2. Verify 7 expenses displayed
3. Check expense details:
   - Categories (Venue, Food, Photography, etc.)
   - Payment statuses (Paid, Partial, Pending)
   - Amounts and descriptions
4. Test "Add Expense" button (if implemented)

#### Vendor Management
1. From Wedding event, view Vendors tab
2. Verify 5 vendors displayed:
   - Grand Ballroom Hotel
   - Divine Delights Catering
   - Perfect Moments Photography
   - Elegant Blooms Florist
   - Melody Makers DJ Services
3. Check vendor details (contact info, payment terms)
4. Verify reminder dates

### Scenario 4: Multi-User Testing

#### Test User 1: John Doe (Primary)
- Login: demo@budgetease.com / Demo@123
- Should see: 2 events (Wedding, Birthday)
- Can manage: All aspects of both events
- Collaborates on: Annual Company Gala

#### Test User 2: Jane Smith
- Login: jane.smith@example.com / Demo@123
- Should see: 2 events (Anniversary as owner, Wedding as collaborator)
- Can manage: Anniversary event fully, Wedding as collaborator
- Owns: 25th Wedding Anniversary

#### Test User 3: Michael Johnson
- Login: michael.johnson@example.com / Demo@123
- Should see: 1 event (Annual Company Gala)
- Can manage: Corporate event
- Has collaborator: John Doe

### Scenario 5: Data Relationships

Verify these relationships work correctly:

1. **Events → Owner**
   - Each event belongs to one user
   - User can see their owned events

2. **Events → Expenses**
   - Wedding event has 7 expenses
   - Birthday event has 4 expenses
   - Anniversary event has 3 expenses
   - Expenses show correct totals

3. **Events → Vendors**
   - Wedding has 5 vendors
   - Birthday has 3 vendors
   - Anniversary has 2 vendors
   - Vendors link to expenses

4. **Events → Collaborators**
   - Wedding has Jane as collaborator
   - Corporate event has John as collaborator
   - Collaborators see events in their list

5. **Expenses → Vendors**
   - Wedding venue expense links to Grand Ballroom Hotel
   - Catering expense links to Divine Delights
   - Some expenses have no vendor (miscellaneous)

## Expected Results Summary

### API Endpoints
- ✅ All endpoints return 200 OK
- ✅ Health check passes
- ✅ Events require authentication (returns [] without auth)
- ✅ Specific event/expense/vendor lookups work
- ✅ JSON responses are well-formed

### Database
- ✅ 4 events created with correct data
- ✅ 3 users with hashed passwords
- ✅ 14 expenses with varied statuses
- ✅ 10 vendors with contact information
- ✅ 2 collaborator relationships
- ✅ All foreign keys properly linked

### Data Quality
- ✅ Event dates are in the future
- ✅ Budget amounts are realistic
- ✅ Payment statuses vary appropriately
- ✅ Vendor reminders are scheduled
- ✅ Expenses have proper categories
- ✅ Contact information is formatted correctly

## Troubleshooting

### Database Not Seeded
If data is missing:
1. Stop the API
2. Delete `src/BudgetEase.Api/budgetease.db`
3. Restart API - seeding will occur automatically

### API Not Responding
1. Check if port 5108 is available
2. Verify .NET 9.0 SDK is installed
3. Check console for error messages
4. Review logs for migration issues

### Web UI Issues
1. Ensure API is running first
2. Check that HttpClient base address is correct in Program.cs
3. Verify port 7001 is available
4. Check browser console for errors

### Authentication Issues
1. Verify user exists in database
2. Check password is correct: Demo@123
3. Ensure Identity is configured properly
4. Review authentication logs

## Performance Benchmarks

Expected response times (Development):
- Health check: < 50ms
- Event list: < 200ms
- Event details: < 300ms (includes related data)
- Expense list: < 150ms
- Vendor list: < 150ms
- Database queries: < 100ms

## Test Coverage

This demo data provides coverage for:
- ✅ Multiple event types (Wedding, Birthday, Anniversary, Corporate)
- ✅ Various budget ranges ($8K to $75K)
- ✅ Different payment statuses (Paid, Partial, Pending)
- ✅ Multiple vendor categories
- ✅ User ownership and collaboration
- ✅ Events at different planning stages
- ✅ Complete and incomplete events
- ✅ Various expense categories
- ✅ Vendor reminders at different intervals

## Next Steps

After validating the demo data:
1. Test creating new events
2. Test adding expenses and vendors
3. Test editing existing data
4. Test deleting data
5. Test user registration
6. Test password reset
7. Test email notifications (when implemented)
8. Test report generation (when implemented)

## Documentation References

- [DEMO_DATA.md](DEMO_DATA.md) - Complete demo data reference
- [USER_FLOW.md](USER_FLOW.md) - User flow documentation
- [README.md](README.md) - Project overview
- [API Documentation](src/BudgetEase.Api/) - API endpoint details

---

**Last Updated:** 2024
**Test Version:** 1.0
**Status:** ✅ All tests passing
