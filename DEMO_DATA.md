# BudgetEase Demo Data Documentation

This document describes the sample data that is automatically seeded into the BudgetEase application for demonstration purposes.

## 🔑 Demo User Credentials

### Primary Demo User
- **Email:** demo@budgetease.com
- **Password:** Demo@123
- **Name:** John Doe
- **Role:** Event Owner

### Additional Demo Users
1. **Jane Smith**
   - Email: jane.smith@example.com
   - Password: Demo@123
   - Collaborator on Sarah & Tom's Wedding

2. **Michael Johnson**
   - Email: michael.johnson@example.com
   - Password: Demo@123
   - Owner of Annual Company Gala

## 📅 Demo Events

### 1. Sarah & Tom's Wedding
- **Type:** Wedding
- **Date:** 60 days from today
- **Venue:** Grand Ballroom Hotel, Downtown
- **Budget:** $50,000.00
- **Owner:** John Doe (demo@budgetease.com)
- **Status:** Active

#### Vendors (5)
1. **Grand Ballroom Hotel** - Venue
2. **Divine Delights Catering** - Catering
3. **Perfect Moments Photography** - Photography
4. **Elegant Blooms Florist** - Decoration
5. **Melody Makers DJ Services** - Entertainment

#### Expenses (7)
1. Venue rental - $15,000 (Paid)
2. Full course dinner - $12,000 (Partial)
3. Photography & videography - $5,000 (Pending)
4. Floral arrangements - $4,500 (Pending)
5. DJ services - $2,500 (Pending)
6. Wedding attire alterations - $1,850 (Paid)
7. Custom invitations - $750 (Paid)

**Total Spent:** $17,600 / $50,000 (35.2%)

---

### 2. Emma's 30th Birthday Bash
- **Type:** Birthday
- **Date:** 20 days from today
- **Venue:** Sunset Garden Restaurant
- **Budget:** $8,000.00
- **Owner:** John Doe (demo@budgetease.com)
- **Status:** Active

#### Vendors (3)
1. **Sunset Garden Restaurant** - Venue & Catering
2. **Party Time Entertainers** - Entertainment
3. **Sweet Dreams Bakery** - Cake & Desserts

#### Expenses (4)
1. Venue & dinner buffet - $4,500 (Partial)
2. Live band and DJ - $1,800 (Pending)
3. Custom cake & desserts - $600 (Pending)
4. Party decorations - $380 (Paid)

**Total Spent:** $380 / $8,000 (4.75%)

---

### 3. 25th Wedding Anniversary
- **Type:** Anniversary
- **Date:** 90 days from today
- **Venue:** Lakeside Country Club
- **Budget:** $15,000.00
- **Owner:** Jane Smith (jane.smith@example.com)
- **Status:** Active

#### Vendors (2)
1. **Lakeside Country Club** - Venue
2. **Gourmet Catering Co** - Catering

#### Expenses (3)
1. Country club venue rental - $6,000 (Pending)
2. Gourmet dinner service - $7,500 (Pending)
3. Professional photography - $1,500 (Pending)

**Total Spent:** $0 / $15,000 (0%)

---

### 4. Annual Company Gala
- **Type:** Corporate
- **Date:** 45 days from today
- **Venue:** Convention Center
- **Budget:** $75,000.00
- **Owner:** Michael Johnson (michael.johnson@example.com)
- **Status:** Active
- **Collaborator:** John Doe (Event Coordinator)

#### Vendors (0)
No vendors added yet.

#### Expenses (0)
No expenses added yet.

**Total Spent:** $0 / $75,000 (0%)

---

## 📊 Overall Statistics

- **Total Events:** 4
- **Total Users:** 3
- **Total Vendors:** 10
- **Total Expenses:** 14
- **Combined Budget:** $148,000.00
- **Combined Spent:** $17,980.00
- **Overall Budget Utilization:** 12.1%

## 🎭 Demo Scenarios

### Scenario 1: View All Events
1. Login as demo@budgetease.com
2. Navigate to Events page
3. View 2 events owned by John Doe (Wedding, Birthday)

### Scenario 2: Manage Expenses
1. Login as demo@budgetease.com
2. Open "Sarah & Tom's Wedding"
3. View 7 expenses with mixed payment statuses
4. Total spent: $17,600 out of $50,000 budget

### Scenario 3: Vendor Management
1. Login as demo@budgetease.com
2. Open "Sarah & Tom's Wedding"
3. View 5 vendors with contact information
4. Check upcoming reminders

### Scenario 4: Budget Tracking
1. Login as demo@budgetease.com
2. View Dashboard
3. See budget utilization across all events
4. Identify events nearing budget limits

### Scenario 5: Collaboration
1. Login as jane.smith@example.com
2. View "Sarah & Tom's Wedding" as a collaborator
3. Access event details and expenses

### Scenario 6: Fresh Event Planning
1. Login as michael.johnson@example.com
2. View "Annual Company Gala" with no vendors/expenses
3. Start planning from scratch

## 🔄 Data Refresh

To refresh the demo data:
1. Delete the database file: `src/BudgetEase.Api/budgetease.db`
2. Restart the API application
3. The database will be recreated and reseeded automatically

## 📝 Notes

- All demo data is created with realistic values and scenarios
- Payment statuses vary (Paid, Partial, Pending) to demonstrate different states
- Event dates are relative to the current date for realistic timelines
- Vendor reminders are set at various intervals
- Password for all demo users: **Demo@123**

## 🔒 Security Note

**Important:** This is demo data for development and testing purposes only. In production:
- Change all default passwords
- Remove or disable demo accounts
- Use strong, unique passwords
- Implement proper authentication and authorization

---

**Last Updated:** 2024
**Document Version:** 1.0
