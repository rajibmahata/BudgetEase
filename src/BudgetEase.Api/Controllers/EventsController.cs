using BudgetEase.Core.DTOs;
using BudgetEase.Core.Entities;
using BudgetEase.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetEase.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EventsController : ControllerBase
{
    private readonly IEventRepository _eventRepository;
    private readonly IExpenseRepository _expenseRepository;

    public EventsController(IEventRepository eventRepository, IExpenseRepository expenseRepository)
    {
        _eventRepository = eventRepository;
        _expenseRepository = expenseRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "default-user";
        var events = await _eventRepository.GetAllByUserIdAsync(userId);
        
        var eventDtos = new List<EventDto>();
        foreach (var @event in events)
        {
            var totalSpent = await _expenseRepository.GetTotalSpentByEventIdAsync(@event.Id);
            eventDtos.Add(new EventDto
            {
                Id = @event.Id,
                Name = @event.Name,
                Type = @event.Type,
                EventDate = @event.EventDate,
                Venue = @event.Venue,
                BudgetLimit = @event.BudgetLimit,
                TotalSpent = totalSpent,
                RemainingBudget = @event.BudgetLimit - totalSpent
            });
        }

        return Ok(eventDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetEvent(int id)
    {
        var @event = await _eventRepository.GetByIdAsync(id);
        if (@event == null)
            return NotFound();

        var totalSpent = await _expenseRepository.GetTotalSpentByEventIdAsync(@event.Id);
        var eventDto = new EventDto
        {
            Id = @event.Id,
            Name = @event.Name,
            Type = @event.Type,
            EventDate = @event.EventDate,
            Venue = @event.Venue,
            BudgetLimit = @event.BudgetLimit,
            TotalSpent = totalSpent,
            RemainingBudget = @event.BudgetLimit - totalSpent
        };

        return Ok(eventDto);
    }

    [HttpPost]
    public async Task<ActionResult<EventDto>> CreateEvent(CreateEventDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "default-user";
        
        var @event = new Event
        {
            Name = dto.Name,
            Type = dto.Type,
            EventDate = dto.EventDate,
            Venue = dto.Venue,
            BudgetLimit = dto.BudgetLimit,
            OwnerId = userId
        };

        await _eventRepository.CreateAsync(@event);

        var eventDto = new EventDto
        {
            Id = @event.Id,
            Name = @event.Name,
            Type = @event.Type,
            EventDate = @event.EventDate,
            Venue = @event.Venue,
            BudgetLimit = @event.BudgetLimit,
            TotalSpent = 0,
            RemainingBudget = @event.BudgetLimit
        };

        return CreatedAtAction(nameof(GetEvent), new { id = @event.Id }, eventDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EventDto>> UpdateEvent(int id, UpdateEventDto dto)
    {
        var @event = await _eventRepository.GetByIdAsync(id);
        if (@event == null)
            return NotFound();

        if (dto.Name != null) @event.Name = dto.Name;
        if (dto.Type != null) @event.Type = dto.Type;
        if (dto.EventDate.HasValue) @event.EventDate = dto.EventDate.Value;
        if (dto.Venue != null) @event.Venue = dto.Venue;
        if (dto.BudgetLimit.HasValue) @event.BudgetLimit = dto.BudgetLimit.Value;

        await _eventRepository.UpdateAsync(@event);

        var totalSpent = await _expenseRepository.GetTotalSpentByEventIdAsync(@event.Id);
        var eventDto = new EventDto
        {
            Id = @event.Id,
            Name = @event.Name,
            Type = @event.Type,
            EventDate = @event.EventDate,
            Venue = @event.Venue,
            BudgetLimit = @event.BudgetLimit,
            TotalSpent = totalSpent,
            RemainingBudget = @event.BudgetLimit - totalSpent
        };

        return Ok(eventDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var result = await _eventRepository.DeleteAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
}
