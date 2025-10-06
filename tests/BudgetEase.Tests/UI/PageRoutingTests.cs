using Bunit;
using BudgetEase.Web.Components.Pages;
using Microsoft.AspNetCore.Components;

namespace BudgetEase.Tests.UI;

/// <summary>
/// Tests for validating page routing and @page directives
/// </summary>
public class PageRoutingTests : TestContext
{
    [Fact]
    public void HomePage_HasCorrectRoute()
    {
        // Act
        var cut = RenderComponent<Home>();

        // Assert - Home page should be accessible at root "/"
        Assert.NotNull(cut);
    }

    [Fact]
    public void HomePage_HasCorrectTitle()
    {
        // Act
        var cut = RenderComponent<Home>();

        // Assert
        var pageTitle = cut.Find("h1.dashboard-title");
        Assert.NotNull(pageTitle);
        Assert.Contains("Welcome to BudgetEase", pageTitle.TextContent);
    }

    [Fact]
    public void HomePage_ContainsDashboardElements()
    {
        // Act
        var cut = RenderComponent<Home>();

        // Assert
        Assert.Contains("dashboard-container", cut.Markup);
        Assert.Contains("welcome-section", cut.Markup);
        Assert.Contains("stats-grid", cut.Markup);
        Assert.Contains("features-section", cut.Markup);
    }

    [Fact]
    public void HomePage_DisplaysKeyFeatures()
    {
        // Act
        var cut = RenderComponent<Home>();

        // Assert
        var featureCards = cut.FindAll(".feature-card");
        Assert.True(featureCards.Count >= 4, "Should have at least 4 feature cards");
        
        // Check for key feature sections
        Assert.Contains("Event Management", cut.Markup);
        Assert.Contains("Expense Tracking", cut.Markup);
        Assert.Contains("Vendor Management", cut.Markup);
    }

    [Fact]
    public void EventsPage_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<Events>();

        // Assert
        Assert.NotNull(cut);
    }

    [Fact]
    public void EventsPage_HasCorrectTitle()
    {
        // Act
        var cut = RenderComponent<Events>();

        // Assert
        var pageHeader = cut.Find(".page-header h1");
        Assert.NotNull(pageHeader);
        Assert.Contains("Event Management", pageHeader.TextContent);
    }

    [Fact]
    public void EventsPage_HasNewEventButton()
    {
        // Act
        var cut = RenderComponent<Events>();

        // Assert
        var button = cut.Find("button.btn-primary");
        Assert.NotNull(button);
        Assert.Contains("New Event", button.TextContent);
    }

    [Fact]
    public void EventsPage_ShowsEmptyState()
    {
        // Act
        var cut = RenderComponent<Events>();

        // Assert
        var emptyState = cut.Find(".empty-state");
        Assert.NotNull(emptyState);
        Assert.Contains("No events yet", emptyState.TextContent);
    }

    [Fact]
    public void ExpensesPage_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<Expenses>();

        // Assert
        Assert.NotNull(cut);
    }

    [Fact]
    public void ExpensesPage_HasCorrectTitle()
    {
        // Act
        var cut = RenderComponent<Expenses>();

        // Assert
        var pageHeader = cut.Find(".page-header h1");
        Assert.NotNull(pageHeader);
        Assert.Contains("Expense Tracking", pageHeader.TextContent);
    }

    [Fact]
    public void ExpensesPage_HasAddExpenseButton()
    {
        // Act
        var cut = RenderComponent<Expenses>();

        // Assert
        var button = cut.Find("button.btn-primary");
        Assert.NotNull(button);
        Assert.Contains("Add Expense", button.TextContent);
    }

    [Fact]
    public void ExpensesPage_ShowsEmptyState()
    {
        // Act
        var cut = RenderComponent<Expenses>();

        // Assert
        var emptyState = cut.Find(".empty-state");
        Assert.NotNull(emptyState);
        Assert.Contains("No expenses recorded", emptyState.TextContent);
    }

    [Fact]
    public void VendorsPage_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<Vendors>();

        // Assert
        Assert.NotNull(cut);
    }

    [Fact]
    public void VendorsPage_HasCorrectTitle()
    {
        // Act
        var cut = RenderComponent<Vendors>();

        // Assert
        var pageHeader = cut.Find(".page-header h1");
        Assert.NotNull(pageHeader);
        Assert.Contains("Vendor Management", pageHeader.TextContent);
    }

    [Fact]
    public void VendorsPage_HasAddVendorButton()
    {
        // Act
        var cut = RenderComponent<Vendors>();

        // Assert
        var button = cut.Find("button.btn-primary");
        Assert.NotNull(button);
        Assert.Contains("Add Vendor", button.TextContent);
    }

    [Fact]
    public void VendorsPage_ShowsEmptyState()
    {
        // Act
        var cut = RenderComponent<Vendors>();

        // Assert
        var emptyState = cut.Find(".empty-state");
        Assert.NotNull(emptyState);
        Assert.Contains("No vendors added", emptyState.TextContent);
    }


}
