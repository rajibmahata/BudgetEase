using BudgetEase.Api.Controllers;
using BudgetEase.Core.DTOs;
using BudgetEase.Core.Entities;
using BudgetEase.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace BudgetEase.Tests.Controllers;

public class EventsControllerTests
{
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly Mock<IExpenseRepository> _mockExpenseRepository;
    private readonly EventsController _controller;

    public EventsControllerTests()
    {
        _mockEventRepository = new Mock<IEventRepository>();
        _mockExpenseRepository = new Mock<IExpenseRepository>();
        _controller = new EventsController(_mockEventRepository.Object, _mockExpenseRepository.Object);
        
        // Setup user context
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id")
        }));
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetEvents_ReturnsOkResultWithEvents()
    {
        // Arrange
        var events = new List<Event>
        {
            new Event { Id = 1, Name = "Test Event", Type = "Wedding", BudgetLimit = 10000, OwnerId = "test-user-id" }
        };
        _mockEventRepository.Setup(repo => repo.GetAllByUserIdAsync(It.IsAny<string>()))
            .ReturnsAsync(events);
        _mockExpenseRepository.Setup(repo => repo.GetTotalSpentByEventIdAsync(It.IsAny<int>()))
            .ReturnsAsync(5000);

        // Act
        var result = await _controller.GetEvents();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<EventDto>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetEvent_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var testEvent = new Event { Id = 1, Name = "Test Event", Type = "Wedding", BudgetLimit = 10000 };
        _mockEventRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(testEvent);
        _mockExpenseRepository.Setup(repo => repo.GetTotalSpentByEventIdAsync(1))
            .ReturnsAsync(5000);

        // Act
        var result = await _controller.GetEvent(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<EventDto>(okResult.Value);
        Assert.Equal("Test Event", returnValue.Name);
    }

    [Fact]
    public async Task GetEvent_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockEventRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Event?)null);

        // Act
        var result = await _controller.GetEvent(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateEvent_WithValidData_ReturnsCreatedResult()
    {
        // Arrange
        var createDto = new CreateEventDto
        {
            Name = "New Event",
            Type = "Birthday",
            EventDate = DateTime.UtcNow.AddDays(30),
            Venue = "Test Venue",
            BudgetLimit = 5000
        };

        _mockEventRepository.Setup(repo => repo.CreateAsync(It.IsAny<Event>()))
            .ReturnsAsync((Event e) => e);

        // Act
        var result = await _controller.CreateEvent(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<EventDto>(createdResult.Value);
        Assert.Equal("New Event", returnValue.Name);
    }

    [Fact]
    public async Task UpdateEvent_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var testEvent = new Event { Id = 1, Name = "Test Event", Type = "Wedding", BudgetLimit = 10000 };
        var updateDto = new UpdateEventDto { Name = "Updated Event" };

        _mockEventRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(testEvent);
        _mockEventRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Event>()))
            .ReturnsAsync((Event e) => e);
        _mockExpenseRepository.Setup(repo => repo.GetTotalSpentByEventIdAsync(1))
            .ReturnsAsync(5000);

        // Act
        var result = await _controller.UpdateEvent(1, updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<EventDto>(okResult.Value);
        Assert.Equal("Updated Event", returnValue.Name);
    }

    [Fact]
    public async Task UpdateEvent_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockEventRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Event?)null);

        // Act
        var result = await _controller.UpdateEvent(999, new UpdateEventDto());

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteEvent_WithValidId_ReturnsNoContent()
    {
        // Arrange
        _mockEventRepository.Setup(repo => repo.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteEvent(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteEvent_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockEventRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteEvent(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
