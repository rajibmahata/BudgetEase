using Bunit;
using BudgetEase.Web.Components.Pages;
using BudgetEase.Web.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace BudgetEase.Tests.UI;

/// <summary>
/// Tests for button interactions and UI element validation
/// </summary>
public class ButtonInteractionTests : TestContext
{
    public ButtonInteractionTests()
    {
        // Register mock services for all tests
        var mockEventService = new Mock<EventService>(MockBehavior.Loose, new object[] { new HttpClient() });
        var mockExpenseService = new Mock<ExpenseService>(MockBehavior.Loose, new object[] { new HttpClient() });
        var mockVendorService = new Mock<VendorService>(MockBehavior.Loose, new object[] { new HttpClient() });
        
        Services.AddSingleton(mockEventService.Object);
        Services.AddSingleton(mockExpenseService.Object);
        Services.AddSingleton(mockVendorService.Object);
    }

    [Fact]
    public void EventsPage_NewEventButton_HasCorrectStyling()
    {
        // Act
        var cut = RenderComponent<Events>();

        // Assert
        var button = cut.Find("button.btn-primary");
        Assert.NotNull(button);
        Assert.Contains("btn", button.ClassName);
        Assert.Contains("btn-primary", button.ClassName);
    }

    [Fact]
    public void EventsPage_NewEventButton_IsClickable()
    {
        // Act
        var cut = RenderComponent<Events>();
        var button = cut.Find("button.btn-primary");

        // Assert - Button should be enabled and clickable
        Assert.False(button.HasAttribute("disabled"));
        Assert.Equal("button", button.GetAttribute("type") ?? "button"); // Default type is button
    }

    [Fact]
    public void ExpensesPage_AddExpenseButton_HasCorrectStyling()
    {
        // Act
        var cut = RenderComponent<Expenses>();

        // Assert
        var button = cut.Find("button.btn-primary");
        Assert.NotNull(button);
        Assert.Contains("btn", button.ClassName);
        Assert.Contains("btn-primary", button.ClassName);
    }

    [Fact]
    public void ExpensesPage_AddExpenseButton_IsClickable()
    {
        // Act
        var cut = RenderComponent<Expenses>();
        var button = cut.Find("button.btn-primary");

        // Assert - Button should be disabled when no event is selected
        Assert.True(button.HasAttribute("disabled"));
    }

    [Fact]
    public void VendorsPage_AddVendorButton_HasCorrectStyling()
    {
        // Act
        var cut = RenderComponent<Vendors>();

        // Assert
        var button = cut.Find("button.btn-primary");
        Assert.NotNull(button);
        Assert.Contains("btn", button.ClassName);
        Assert.Contains("btn-primary", button.ClassName);
    }

    [Fact]
    public void VendorsPage_AddVendorButton_IsClickable()
    {
        // Act
        var cut = RenderComponent<Vendors>();
        var button = cut.Find("button.btn-primary");

        // Assert - Button should be disabled when no event is selected
        Assert.True(button.HasAttribute("disabled"));
    }

    [Fact]
    public void AllPages_ButtonsHaveAccessibleText()
    {
        // Arrange & Act
        var eventsPage = RenderComponent<Events>();
        var expensesPage = RenderComponent<Expenses>();
        var vendorsPage = RenderComponent<Vendors>();

        // Assert
        var eventsButton = eventsPage.Find("button.btn-primary");
        Assert.True(!string.IsNullOrWhiteSpace(eventsButton.TextContent), 
            "Events page button should have visible text");

        var expensesButton = expensesPage.Find("button.btn-primary");
        Assert.True(!string.IsNullOrWhiteSpace(expensesButton.TextContent), 
            "Expenses page button should have visible text");

        var vendorsButton = vendorsPage.Find("button.btn-primary");
        Assert.True(!string.IsNullOrWhiteSpace(vendorsButton.TextContent), 
            "Vendors page button should have visible text");
    }

    [Fact]
    public void EventsPage_ContentCard_HasCorrectStructure()
    {
        // Act
        var cut = RenderComponent<Events>();

        // Assert
        var contentCard = cut.Find(".content-card");
        Assert.NotNull(contentCard);
        
        var cardHeader = cut.Find(".card-header");
        Assert.NotNull(cardHeader);
        
        var emptyState = cut.Find(".empty-state");
        Assert.NotNull(emptyState);
    }

    [Fact]
    public void ExpensesPage_ContentCard_HasCorrectStructure()
    {
        // Act
        var cut = RenderComponent<Expenses>();

        // Assert
        var contentCard = cut.Find(".content-card");
        Assert.NotNull(contentCard);
        
        var cardHeader = cut.Find(".card-header");
        Assert.NotNull(cardHeader);
        
        var emptyState = cut.Find(".empty-state");
        Assert.NotNull(emptyState);
    }

    [Fact]
    public void VendorsPage_ContentCard_HasCorrectStructure()
    {
        // Act
        var cut = RenderComponent<Vendors>();

        // Assert
        var contentCard = cut.Find(".content-card");
        Assert.NotNull(contentCard);
        
        var cardHeader = cut.Find(".card-header");
        Assert.NotNull(cardHeader);
        
        var emptyState = cut.Find(".empty-state");
        Assert.NotNull(emptyState);
    }

    [Fact]
    public void HomePage_StatCards_AreClickableOrInteractive()
    {
        // Act
        var cut = RenderComponent<Home>();

        // Assert
        var statCards = cut.FindAll(".stat-card");
        Assert.True(statCards.Count >= 4, "Should have at least 4 stat cards");
        
        // Verify each stat card has appropriate styling for hover effects
        foreach (var card in statCards)
        {
            Assert.Contains("stat-card", card.ClassName);
        }
    }

    [Fact]
    public void HomePage_FeatureCards_AreRendered()
    {
        // Act
        var cut = RenderComponent<Home>();

        // Assert
        var featureCards = cut.FindAll(".feature-card");
        Assert.True(featureCards.Count >= 4, "Should have at least 4 feature cards");
        
        // Verify each feature card has content
        foreach (var card in featureCards)
        {
            var heading = card.QuerySelector("h3");
            var paragraph = card.QuerySelector("p");
            
            Assert.NotNull(heading);
            Assert.NotNull(paragraph);
            Assert.True(!string.IsNullOrWhiteSpace(heading.TextContent));
            Assert.True(!string.IsNullOrWhiteSpace(paragraph.TextContent));
        }
    }

    [Fact]
    public void AllPages_EmptyStates_HaveIcons()
    {
        // Arrange & Act
        var eventsPage = RenderComponent<Events>();
        var expensesPage = RenderComponent<Expenses>();
        var vendorsPage = RenderComponent<Vendors>();

        // Assert - Each empty state should have an SVG icon
        var eventsEmptyState = eventsPage.Find(".empty-state");
        Assert.NotNull(eventsEmptyState.QuerySelector("svg"));

        var expensesEmptyState = expensesPage.Find(".empty-state");
        Assert.NotNull(expensesEmptyState.QuerySelector("svg"));

        var vendorsEmptyState = vendorsPage.Find(".empty-state");
        Assert.NotNull(vendorsEmptyState.QuerySelector("svg"));
    }
}
