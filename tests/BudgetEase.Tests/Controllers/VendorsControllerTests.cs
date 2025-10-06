using BudgetEase.Api.Controllers;
using BudgetEase.Core.DTOs;
using BudgetEase.Core.Entities;
using BudgetEase.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BudgetEase.Tests.Controllers;

public class VendorsControllerTests
{
    private readonly Mock<IVendorRepository> _mockVendorRepository;
    private readonly Mock<IEventRepository> _mockEventRepository;
    private readonly VendorsController _controller;

    public VendorsControllerTests()
    {
        _mockVendorRepository = new Mock<IVendorRepository>();
        _mockEventRepository = new Mock<IEventRepository>();
        _controller = new VendorsController(_mockVendorRepository.Object, _mockEventRepository.Object);
    }

    [Fact]
    public async Task GetVendorsByEvent_ReturnsOkResultWithVendors()
    {
        // Arrange
        var vendors = new List<Vendor>
        {
            new Vendor { Id = 1, EventId = 1, Name = "Test Vendor", ServiceType = "Catering", Email = "test@vendor.com" }
        };
        _mockVendorRepository.Setup(repo => repo.GetAllByEventIdAsync(1))
            .ReturnsAsync(vendors);

        // Act
        var result = await _controller.GetVendorsByEvent(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<VendorDto>>(okResult.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetVendor_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var vendor = new Vendor { Id = 1, EventId = 1, Name = "Test Vendor", ServiceType = "Catering" };
        _mockVendorRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(vendor);

        // Act
        var result = await _controller.GetVendor(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<VendorDto>(okResult.Value);
        Assert.Equal("Test Vendor", returnValue.Name);
    }

    [Fact]
    public async Task GetVendor_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockVendorRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Vendor?)null);

        // Act
        var result = await _controller.GetVendor(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateVendor_WithValidData_ReturnsCreatedResult()
    {
        // Arrange
        var createDto = new CreateVendorDto
        {
            EventId = 1,
            Name = "New Vendor",
            ServiceType = "Photography",
            ContactNumber = "123-456-7890",
            Email = "newvendor@test.com",
            PaymentTerms = "50% advance"
        };

        _mockEventRepository.Setup(repo => repo.ExistsAsync(1))
            .ReturnsAsync(true);
        _mockVendorRepository.Setup(repo => repo.CreateAsync(It.IsAny<Vendor>()))
            .ReturnsAsync((Vendor v) => v);

        // Act
        var result = await _controller.CreateVendor(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<VendorDto>(createdResult.Value);
        Assert.Equal("New Vendor", returnValue.Name);
    }

    [Fact]
    public async Task CreateVendor_WithInvalidEventId_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreateVendorDto
        {
            EventId = 999,
            Name = "New Vendor",
            ServiceType = "Photography"
        };

        _mockEventRepository.Setup(repo => repo.ExistsAsync(999))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.CreateVendor(createDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Event not found", badRequestResult.Value);
    }

    [Fact]
    public async Task UpdateVendor_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var vendor = new Vendor { Id = 1, EventId = 1, Name = "Test Vendor", ServiceType = "Catering" };
        var updateDto = new UpdateVendorDto { Name = "Updated Vendor" };

        _mockVendorRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(vendor);
        _mockVendorRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Vendor>()))
            .ReturnsAsync((Vendor v) => v);

        // Act
        var result = await _controller.UpdateVendor(1, updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<VendorDto>(okResult.Value);
        Assert.Equal("Updated Vendor", returnValue.Name);
    }

    [Fact]
    public async Task UpdateVendor_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockVendorRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Vendor?)null);

        // Act
        var result = await _controller.UpdateVendor(999, new UpdateVendorDto());

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteVendor_WithValidId_ReturnsNoContent()
    {
        // Arrange
        _mockVendorRepository.Setup(repo => repo.DeleteAsync(1))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteVendor(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteVendor_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockVendorRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.DeleteVendor(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetUpcomingReminders_ReturnsOkResultWithVendors()
    {
        // Arrange
        var vendors = new List<Vendor>
        {
            new Vendor 
            { 
                Id = 1, 
                EventId = 1, 
                Name = "Vendor with Reminder", 
                ServiceType = "Catering",
                NextReminderDate = DateTime.UtcNow.AddDays(1)
            }
        };
        _mockVendorRepository.Setup(repo => repo.GetVendorsWithUpcomingRemindersAsync())
            .ReturnsAsync(vendors);

        // Act
        var result = await _controller.GetUpcomingReminders();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<VendorDto>>(okResult.Value);
        Assert.Single(returnValue);
    }
}
