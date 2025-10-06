using Bunit;
using BudgetEase.Web.Components.Layout;
using Microsoft.AspNetCore.Components;

namespace BudgetEase.Tests.UI;

/// <summary>
/// Tests for the NavMenu component to validate navigation links and structure
/// </summary>
public class NavMenuTests : TestContext
{
    [Fact]
    public void NavMenu_RendersCorrectly()
    {
        // Act
        var cut = RenderComponent<NavMenu>();

        // Assert
        Assert.NotNull(cut);
        
        // Verify key elements are present
        Assert.Contains("top-row", cut.Markup);
        Assert.Contains("navbar-brand", cut.Markup);
        Assert.Contains("BudgetEase", cut.Markup);
        Assert.Contains("nav-scrollable", cut.Markup);
        Assert.Contains("Dashboard", cut.Markup);
        Assert.Contains("Events", cut.Markup);
        Assert.Contains("Expenses", cut.Markup);
        Assert.Contains("Vendors", cut.Markup);
    }

    [Fact]
    public void NavMenu_HasBrandingLink()
    {
        // Act
        var cut = RenderComponent<NavMenu>();

        // Assert
        var brandLink = cut.Find(".navbar-brand");
        Assert.NotNull(brandLink);
        Assert.Contains("BudgetEase", brandLink.TextContent);
        Assert.Equal(string.Empty, brandLink.GetAttribute("href"));
    }

    [Fact]
    public void NavMenu_HasDashboardLink()
    {
        // Act
        var cut = RenderComponent<NavMenu>();

        // Assert
        var links = cut.FindAll("a.nav-link");
        var dashboardLink = links.FirstOrDefault(l => l.TextContent.Contains("Dashboard"));
        
        Assert.NotNull(dashboardLink);
        Assert.Equal(string.Empty, dashboardLink.GetAttribute("href"));
    }

    [Fact]
    public void NavMenu_HasEventsLink()
    {
        // Act
        var cut = RenderComponent<NavMenu>();

        // Assert
        var links = cut.FindAll("a.nav-link");
        var eventsLink = links.FirstOrDefault(l => l.TextContent.Contains("Events"));
        
        Assert.NotNull(eventsLink);
        Assert.Equal("events", eventsLink.GetAttribute("href"));
    }

    [Fact]
    public void NavMenu_HasExpensesLink()
    {
        // Act
        var cut = RenderComponent<NavMenu>();

        // Assert
        var links = cut.FindAll("a.nav-link");
        var expensesLink = links.FirstOrDefault(l => l.TextContent.Contains("Expenses"));
        
        Assert.NotNull(expensesLink);
        Assert.Equal("expenses", expensesLink.GetAttribute("href"));
    }

    [Fact]
    public void NavMenu_HasVendorsLink()
    {
        // Act
        var cut = RenderComponent<NavMenu>();

        // Assert
        var links = cut.FindAll("a.nav-link");
        var vendorsLink = links.FirstOrDefault(l => l.TextContent.Contains("Vendors"));
        
        Assert.NotNull(vendorsLink);
        Assert.Equal("vendors", vendorsLink.GetAttribute("href"));
    }

    [Fact]
    public void NavMenu_AllLinksHaveIcons()
    {
        // Act
        var cut = RenderComponent<NavMenu>();

        // Assert
        var navLinks = cut.FindAll("a.nav-link");
        
        foreach (var link in navLinks)
        {
            var icon = link.QuerySelector("span[class*='bi']");
            Assert.NotNull(icon);
        }
    }

    [Fact]
    public void NavMenu_HasCorrectNumberOfNavigationItems()
    {
        // Act
        var cut = RenderComponent<NavMenu>();

        // Assert
        var navItems = cut.FindAll(".nav-item");
        Assert.Equal(4, navItems.Count); // Dashboard, Events, Expenses, Vendors
    }

    [Fact]
    public void NavMenu_HasMobileToggleButton()
    {
        // Act
        var cut = RenderComponent<NavMenu>();

        // Assert
        var toggleButton = cut.Find(".navbar-toggler");
        Assert.NotNull(toggleButton);
        Assert.Equal("checkbox", toggleButton.GetAttribute("type"));
        Assert.Equal("Navigation menu", toggleButton.GetAttribute("title"));
    }

    [Fact]
    public void NavMenu_NavigationLinksAreAccessible()
    {
        // Act
        var cut = RenderComponent<NavMenu>();

        // Assert
        var navLinks = cut.FindAll("a.nav-link");
        
        // Each link should have visible text or aria label
        foreach (var link in navLinks)
        {
            var hasText = !string.IsNullOrWhiteSpace(link.TextContent);
            var hasAriaLabel = !string.IsNullOrWhiteSpace(link.GetAttribute("aria-label"));
            
            Assert.True(hasText || hasAriaLabel, "Navigation link should have text or aria-label for accessibility");
        }
    }
}
