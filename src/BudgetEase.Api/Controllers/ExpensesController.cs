using BudgetEase.Core.DTOs;
using BudgetEase.Core.Entities;
using BudgetEase.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetEase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IEventRepository _eventRepository;

    public ExpensesController(IExpenseRepository expenseRepository, IEventRepository eventRepository)
    {
        _expenseRepository = expenseRepository;
        _eventRepository = eventRepository;
    }

    [HttpGet("event/{eventId}")]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetExpensesByEvent(int eventId)
    {
        var expenses = await _expenseRepository.GetAllByEventIdAsync(eventId);
        var expenseDtos = expenses.Select(e => new ExpenseDto
        {
            Id = e.Id,
            EventId = e.EventId,
            Category = e.Category,
            Description = e.Description,
            VendorId = e.VendorId,
            VendorName = e.Vendor?.Name,
            EstimatedCost = e.EstimatedCost,
            ActualCost = e.ActualCost,
            PaymentStatus = e.PaymentStatus,
            PaymentDueDate = e.PaymentDueDate
        });

        return Ok(expenseDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseDto>> GetExpense(int id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense == null)
            return NotFound();

        var expenseDto = new ExpenseDto
        {
            Id = expense.Id,
            EventId = expense.EventId,
            Category = expense.Category,
            Description = expense.Description,
            VendorId = expense.VendorId,
            VendorName = expense.Vendor?.Name,
            EstimatedCost = expense.EstimatedCost,
            ActualCost = expense.ActualCost,
            PaymentStatus = expense.PaymentStatus,
            PaymentDueDate = expense.PaymentDueDate
        };

        return Ok(expenseDto);
    }

    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> CreateExpense(CreateExpenseDto dto)
    {
        var eventExists = await _eventRepository.ExistsAsync(dto.EventId);
        if (!eventExists)
            return BadRequest("Event not found");

        var expense = new Expense
        {
            EventId = dto.EventId,
            Category = dto.Category,
            Description = dto.Description,
            VendorId = dto.VendorId,
            EstimatedCost = dto.EstimatedCost,
            ActualCost = dto.ActualCost,
            PaymentStatus = dto.PaymentStatus,
            PaymentDueDate = dto.PaymentDueDate
        };

        await _expenseRepository.CreateAsync(expense);

        var expenseDto = new ExpenseDto
        {
            Id = expense.Id,
            EventId = expense.EventId,
            Category = expense.Category,
            Description = expense.Description,
            VendorId = expense.VendorId,
            EstimatedCost = expense.EstimatedCost,
            ActualCost = expense.ActualCost,
            PaymentStatus = expense.PaymentStatus,
            PaymentDueDate = expense.PaymentDueDate
        };

        return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expenseDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ExpenseDto>> UpdateExpense(int id, UpdateExpenseDto dto)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense == null)
            return NotFound();

        if (dto.Category != null) expense.Category = dto.Category;
        if (dto.Description != null) expense.Description = dto.Description;
        if (dto.VendorId.HasValue) expense.VendorId = dto.VendorId;
        if (dto.EstimatedCost.HasValue) expense.EstimatedCost = dto.EstimatedCost.Value;
        if (dto.ActualCost.HasValue) expense.ActualCost = dto.ActualCost.Value;
        if (dto.PaymentStatus != null) expense.PaymentStatus = dto.PaymentStatus;
        if (dto.PaymentDueDate.HasValue) expense.PaymentDueDate = dto.PaymentDueDate;

        await _expenseRepository.UpdateAsync(expense);

        var expenseDto = new ExpenseDto
        {
            Id = expense.Id,
            EventId = expense.EventId,
            Category = expense.Category,
            Description = expense.Description,
            VendorId = expense.VendorId,
            VendorName = expense.Vendor?.Name,
            EstimatedCost = expense.EstimatedCost,
            ActualCost = expense.ActualCost,
            PaymentStatus = expense.PaymentStatus,
            PaymentDueDate = expense.PaymentDueDate
        };

        return Ok(expenseDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExpense(int id)
    {
        var result = await _expenseRepository.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
