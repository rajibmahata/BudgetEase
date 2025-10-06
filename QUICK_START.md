# BudgetEase Quick Start Guide

Get BudgetEase up and running in 5 minutes with pre-loaded demo data!

## 🚀 Quick Setup

### 1. Clone and Build
```bash
git clone <repository-url>
cd BudgetEase
dotnet build
```

### 2. Start the API
```bash
cd src/BudgetEase.Api
dotnet run
```
✅ API running at: http://localhost:5108
✅ Database automatically created and seeded with sample data!

### 3. Start the Web UI (Optional)
Open a new terminal:
```bash
cd src/BudgetEase.Web
dotnet run
```
✅ Web UI running at: https://localhost:7001

## 🔐 Demo Login

**Email:** demo@budgetease.com  
**Password:** Demo@123

## 📊 What's Pre-loaded?

The application comes with realistic demo data:

| Item | Count | Details |
|------|-------|---------|
| **Users** | 3 | John Doe, Jane Smith, Michael Johnson |
| **Events** | 4 | Wedding, Birthday, Anniversary, Corporate |
| **Expenses** | 14 | Various categories and payment statuses |
| **Vendors** | 10 | Contact info, reminders, payment terms |
| **Budget** | $148K | Combined across all events |

## 🎯 Quick Test

Verify everything is working:
```bash
# From project root
./test_api_endpoints.sh
```

Or manually test an endpoint:
```bash
curl http://localhost:5108/api/events/1
```

## 📱 Try These Features

### View Events
```bash
curl http://localhost:5108/api/events/1 | jq
```

### Check Expenses
```bash
curl http://localhost:5108/api/expenses/event/1 | jq
```

### See Vendors
```bash
curl http://localhost:5108/api/vendors/event/1 | jq
```

## 🔄 Reset Demo Data

To start fresh:
```bash
# Stop the API (Ctrl+C)
rm src/BudgetEase.Api/budgetease.db
# Restart API - data will be reseeded automatically
dotnet run
```

## 📚 Next Steps

- **[DEMO_DATA.md](DEMO_DATA.md)** - Complete data reference
- **[END_TO_END_TESTING.md](END_TO_END_TESTING.md)** - Testing guide
- **[README.md](README.md)** - Full documentation
- **[USER_FLOW.md](USER_FLOW.md)** - User flows and API docs

## 🎬 Demo Scenarios

### Scenario 1: Wedding Event
1. Login as demo@budgetease.com
2. View "Sarah & Tom's Wedding"
3. See $50K budget with $17.6K spent
4. 7 expenses, 5 vendors

### Scenario 2: Multiple Users
- **John Doe**: Owns Wedding + Birthday
- **Jane Smith**: Owns Anniversary, collaborates on Wedding
- **Michael Johnson**: Owns Corporate event

### Scenario 3: Budget Tracking
- View various payment statuses (Paid, Partial, Pending)
- Track expenses by category
- Monitor vendor reminders

## 💡 Pro Tips

1. **Database Location**: `src/BudgetEase.Api/budgetease.db`
2. **All passwords**: Demo@123
3. **API Port**: 5108 (configurable in launchSettings.json)
4. **Web Port**: 7001 (configurable in launchSettings.json)

## 🆘 Troubleshooting

**Problem**: API won't start
- Check if port 5108 is available
- Verify .NET 9.0 SDK is installed

**Problem**: No data appears
- Delete budgetease.db and restart
- Check console for seeding messages

**Problem**: Web UI can't connect
- Ensure API is running first
- Check HttpClient base address in Web/Program.cs

## ✅ Validation Checklist

After starting, verify:
- [ ] API responds at http://localhost:5108/api/health
- [ ] 4 events in database
- [ ] 3 users can login
- [ ] 14 expenses created
- [ ] 10 vendors with contact info
- [ ] All API endpoints return 200 OK

## 📞 Support

See [README.md](README.md) for detailed information or open an issue.

---

**Ready in:** < 5 minutes  
**Demo Data:** ✅ Pre-loaded  
**Documentation:** ✅ Complete  
**Testing:** ✅ Automated  

**Happy Event Planning! 🎉**
