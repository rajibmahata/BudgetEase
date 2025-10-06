using BudgetEase.Api.Controllers;
using BudgetEase.Core.DTOs;
using BudgetEase.Core.Entities;
using BudgetEase.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BudgetEase.Tests.Controllers;

public class ExpensesControllerTests
{
    private readonly Mock<IExpenseRepository> _mockExpenseRepository;
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly ExpensesController _controller;

    public ExpensesControllerTests()
    {
        _mockExpenseRepository = new Mock<IExpenseRepository>();
        _mockEventRepository = new Mock<IEventRepository>();
        _controller = new ExpensesController(_mockExpenseRepository.Object, _mockEventRepository.Object);
    }

    [Fact]
    public async Task GetExpensesByEvent_ReturnsOkResultWithExpenses()
    {
        // Arrange
        var expenses = new List<Expense>
        {
            new Expense { Id = 1, EventId = 1, Category = "Catering", Description = "Food", EstimatedCost = 1000, ActualCost = 950 }
        };
        _mockExpenseRepository.Setup(repo => repo.GetAllByEventIdAsync(1))
            .ReturnsAsync(expenses);

        // Act
        var result = await _controller.GetExpensesByEvent(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<ExpenseDto>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetExpense_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var expense = new Expense { Id = 1, EventId = 1, Category = "Catering", Description = "Food", EstimatedCost = 1000 };
        _mockExpenseRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(expense);

        // Act
        var result = await _controller.GetExpense(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<ExpenseDto>(okResult.Value);
        Assert.Equal("Catering", returnValue.Category);
    }

    [Fact]
    public async Task GetExpense_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockExpenseRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Expense?)null);

        // Act
        var result = await _controller.GetExpense(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateExpense_WithValidData_ReturnsCreatedResult()
    {
        // Arrange
        var createDto = new CreateExpenseDto
        {
            EventId = 1,
            Category = "Decoration",
            Description = "Flowers",
            EstimatedCost = 500,
            ActualCost = 450,
            PaymentStatus = "Paid"
        };

        _mockEventRepository.Setup(repo => repo.ExistsAsync(1))
            .ReturnsAsync(true);
        _mockExpenseRepository.Setup(repo => repo.CreateAsync(It.IsAny<Expense>()))
            .ReturnsAsync((Expense e) => e);

        // Act
        var result = await _controller.CreateExpense(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<ExpenseDto>(createdResult.Value);
        Assert.Equal("Decoration", returnValue.Category);
    }

    [Fact]
    public async Task CreateExpense_WithInvalidEventId_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreateExpenseDto
        {
            EventId = 999,
            Category = "Decoration",
            Description = "Flowers",
            EstimatedCost = 500
        };

        _mockEventRepository.Setup(repo => repo.ExistsAsync(999))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.CreateExpense(createDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Event not found", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateExpense_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var expense = new Expense { Id = 1, EventId = 1, Category = "Catering", Description = "Food" };
        var updateDto = new UpdateExpenseDto { Description = "Updated Food" };

        _mockExpenseRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(expense);
        _mockExpenseRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Expense>()))
            .ReturnsAsync((Expense e) => e);

        // Act
        var result = await _controller.UpdateExpense(1, updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<ExpenseDto>(okResult.Value);
        Assert.Equal("Updated Food", returnValue.Description);
    }

    [Fact]
    public async Task UpdateExpense_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockExpenseRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Expense?)null);

        // Act
        var result = await _controller.UpdateExpense(999, new UpdateExpenseDto());

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteExpense_WithValidId_ReturnsNoContent()
    {
        // Arrange
        _mockExpenseRepository.Setup(repo => repo.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteExpense(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteExpense_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockExpenseRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteExpense(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
